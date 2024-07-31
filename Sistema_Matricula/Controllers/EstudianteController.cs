using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Sistema_Matricula.Models;
using ClosedXML.Excel;
using Microsoft.IdentityModel.Tokens;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Drawing;

namespace Sistema_Matricula.Controllers
{

    public class EstudianteController : Controller
    {
        private readonly DbMatNotaHorarioContext db;
        private readonly IValidator<Estudiante> validadorEstudiante;

        public EstudianteController(DbMatNotaHorarioContext _context, IValidator<Estudiante> _validadorEstudiante)
        {
            db = _context;
            validadorEstudiante = _validadorEstudiante;
        }


        // GET: EstudianteController
        [HttpGet]
        public async Task <ActionResult> ListarEstudiantes(string palabra, int codigo, string estado)
        {
            List<Apoderado> apoderados =  db.Apoderados.ToList();

            List<Estudiante> estudiantes;
            if (!string.IsNullOrEmpty(palabra) && codigo != 0)
            {
                estudiantes = await db.Estudiantes
                                            .Where(e => e.Nombre.StartsWith(palabra) || e.IdEstudiante == codigo)
                                            .ToListAsync();

            }
            else if (!string.IsNullOrEmpty(palabra))
            {
                estudiantes = await db.Estudiantes
                                            .Where(e => e.Nombre.StartsWith(palabra))
                                            .ToListAsync();
            }
            else if (codigo != 0)
            {
                estudiantes = await db.Estudiantes
                                            .Where(e => e.IdEstudiante == codigo)
                                            .ToListAsync();
            }
            else if (estado == "Activo")
            {
                var estudiantesActivos = db.Estudiantes.Where(e => e.Estado == "Activo").ToList();
                return View(estudiantesActivos);

            }
            else if (estado == "Inactivo")
            {
                var estudiantesInactivos = db.Estudiantes.Where(e => e.Estado == "Inactivo").ToList();
                return View(estudiantesInactivos);
            }
            else if (estado == "Pendiente")
            {
                var estudiantesPendientes = db.Estudiantes.Where(e => e.Estado == "Pendiente").ToList();
                return View(estudiantesPendientes);
            }
            else if(estado == "Todos")
            {
                estudiantes = db.Estudiantes.ToList();
            }
            else
            {
                estudiantes = db.Estudiantes.ToList();
            }
            List<SelectListItem> estados = new List<SelectListItem>
            {
                new SelectListItem {Value = "Activo", Text= "Activo"},
                new SelectListItem {Value = "Inactivo", Text= "Inactivo"}
            };
            ViewBag.Estados = estados;

            ViewBag.Apoderados = apoderados;
            ViewBag.Palabra = palabra;
            ViewBag.Codigo = codigo;
            return View(estudiantes);
        }
        public IActionResult ListarApoderadosPartial()
        {
            var apoderados = db.Apoderados.ToList();
            return PartialView("_apoderado", apoderados);
        }


        [HttpGet]
        public ActionResult AgregarEstudiante()
        {
            List<SelectListItem> estados = new List<SelectListItem>
            {
                new SelectListItem { Value = "Activo", Text = "Activo" },
                new SelectListItem { Value = "Inactivo", Text = "Inactivo" }
            };
            ViewBag.Estados = estados;
            return View();
        }

        [HttpPost]
        public ActionResult AgregarEstudiante(Estudiante estudiante)
        {
            /*
             * Uso de validator
             */
            
            var validaciones = validadorEstudiante.Validate(estudiante);
            if (!validaciones.IsValid)
            {
                foreach (var error in validaciones.Errors)
                {
                    Console.WriteLine(error.PropertyName + ": " + error.ErrorMessage);
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                validaciones.AddToModelState(this.ModelState);
            }
            
            /*
             * Creacion de Items de estados
             */
            List<SelectListItem> estados = new List<SelectListItem>
            {
                new SelectListItem { Value = "Activo", Text = "Activo" },
                new SelectListItem { Value = "Inactivo", Text = "Inactivo" }
            };
            ViewBag.Estados = estados;

            
            if (ModelState.IsValid)
            {
                db.Estudiantes.Add(estudiante);
                db.SaveChanges();
                return RedirectToAction("ListarEstudiantes", "Estudiante");
            }
            return View(estudiante);
        }

        // GET: EstudianteController/Details/5
        public ActionResult EstudianteDetalles(int id)
        {
            if (ModelState.IsValid)
            {
                Estudiante estudiante = db.Estudiantes.Where(e => e.IdEstudiante == id).FirstOrDefault();
                return View(estudiante);
            }
            return RedirectToAction("ListarEstudiantes", "Estudiante");
        }

        // GET: EstudianteController/Edit/5
        [HttpGet]
        public IActionResult CargarEditarEstudiante(int idEstudiante)
        {
            return ViewComponent("EditarCursoVC", new { idEstudiante });
        }


        // POST: EstudianteController/Edit/5
        [HttpPost]
        public ActionResult EditarEstudiante(int id, Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                Estudiante estudiActual = db.Estudiantes.Where(e => e.IdEstudiante == id).FirstOrDefault();
                if (estudiActual != null)
                {
                    estudiActual.Nombre = estudiante.Nombre;
                    estudiActual.Apellido = estudiante.Apellido;
                    estudiActual.Sexo = estudiante.Sexo;
                    estudiActual.FechNacimiento = estudiante.FechNacimiento;
                    estudiActual.Direccion = estudiante.Direccion;
                    estudiActual.Estado = estudiante.Estado;
                    estudiActual.Dni = estudiante.Dni;
                    estudiActual.FechNacimiento = estudiante.FechNacimiento;

                    TempData["EditarEstudianteSuccess"] = $"Estudiante {estudiante.Nombre} - {estudiante.Apellido} editado correctamente";
                    db.SaveChanges();
                    return RedirectToAction("ListarEstudiantes");
                }
                else
                {
                    return View(estudiante);
                }
            }
            return View(estudiante);
        }


        public async Task<IActionResult> EliminarEstudiante(int id)
        {

            if (ModelState.IsValid)
            {
                Estudiante estudiante = await db.Estudiantes.Where(d => d.IdEstudiante == id).FirstOrDefaultAsync();

                if (estudiante != null)
                {
                    estudiante.Estado = "Inactivo";
                    db.Estudiantes.Update(estudiante);
                    await db.SaveChangesAsync();
                    return RedirectToAction("ListarEstudiantes");
                }
                else
                {
                    return RedirectToAction("ListarEstudiantes");
                }
            }

            return RedirectToAction("ListarEstudiantes");
        }

        public async Task<IActionResult> ActivarEstudiante(int id)
        {
            

            if (ModelState.IsValid)
            {
                Estudiante estudiante = await db.Estudiantes.Where(e => e.IdEstudiante == id).FirstOrDefaultAsync();

                if (estudiante != null)
                {
                    estudiante.Estado = "Activo";
                    db.Estudiantes.Update(estudiante);
                    db.SaveChangesAsync();
                    return RedirectToAction("ListarEstudiantes");
                }
              else
                {
                    return RedirectToAction("ListarEstudiantes");
                }
            }

            return RedirectToAction("ListarEstudiantes");
        }
        

        public IActionResult PasarTablaExcel()
        {
            var estudiantes = db.Estudiantes.ToList();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Estudiante");
                var currentRow = 1;
                worksheet.Cell("A1").Value = "IdEstudiante";
                worksheet.Cell("B1").Value = "Nombre";
                worksheet.Cell("C1").Value = "Apellido";
                worksheet.Cell("D1").Value = "FechNacimiento";
                worksheet.Cell("G1").Value = "Direccion";
                worksheet.Cell("H1").Value = "Estado";
                worksheet.Cell("I1").Value = "DNI";

                foreach (var estudiante in db.Estudiantes.ToList())
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = estudiante.IdEstudiante;
                    worksheet.Cell(currentRow, 2).Value = estudiante.Nombre;
                    worksheet.Cell(currentRow, 3).Value = estudiante.Apellido;
                    worksheet.Cell(currentRow, 4).Value = estudiante.FechNacimiento;
                    worksheet.Cell(currentRow, 7).Value = estudiante.Direccion;
                    worksheet.Cell(currentRow, 8).Value = estudiante.Estado;
                    worksheet.Cell(currentRow, 9).Value = estudiante.Dni;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ListaEstudiantes.xlsx");
                }
            }
        }
    }
}
