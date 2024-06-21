using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_Matricula.Models;

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
        public async Task <ActionResult> ListarEstudiantes(string palabra)
        {
            List<Apoderado> apoderados =  db.Apoderados.ToList();
            List<Estudiante> estudiantes;
            if (!string.IsNullOrEmpty(palabra))
            {
                estudiantes = db.Estudiantes.Where(i => i.Nombre.StartsWith(palabra)).ToList();
            }
            else
            {
                estudiantes = db.Estudiantes.ToList();
            }

            ViewBag.Apoderados = apoderados;
            ViewBag.Palabra = palabra;
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
                new SelectListItem { Value = "activo", Text = "Activo" },
                new SelectListItem { Value = "inactivo", Text = "Inactivo" }
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
                new SelectListItem { Value = "activo", Text = "Activo" },
                new SelectListItem { Value = "inactivo", Text = "Inactivo" }
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
        public ActionResult EditarEstudiante(int id)
        {

            Estudiante estudiante = db.Estudiantes.Where(e => e.IdEstudiante == id).FirstOrDefault();
            if(estudiante != null)
            {
                return View(estudiante);
                
            }
            return RedirectToAction("ListarEstudiantes", "Estudiante");
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
                    estudiActual.FechNacimiento = estudiante.FechNacimiento;
                    estudiActual.Email = estudiante.Email;
                    estudiActual.Telefono = estudiante.Telefono;
                    estudiActual.Direccion = estudiante.Direccion;
                    estudiActual.Estado = estudiante.Estado;
                    estudiActual.Dni = estudiante.Dni;
                    estudiActual.FechNacimiento = estudiante.FechNacimiento;

                    db.SaveChanges();

                    return RedirectToAction("ListarEstudiantes", "Estudiante");
                }else {
                    return View(estudiante);
                }
            }
            return View(estudiante);
        }
    }
}
