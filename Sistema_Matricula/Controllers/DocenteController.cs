using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_Matricula.Models;
using Sistema_Matricula.ViewsModels;

namespace Sistema_Matricula.Controllers
{
    public class DocenteController : Controller
    {
        private readonly DbMatNotaHorarioContext db;

        public DocenteController(DbMatNotaHorarioContext _db)
        {
            db = _db;
        }

        public ActionResult ListarDocente()
        {
            var docentes = db.Docentes.ToList();
            return View(docentes);
        }

        [HttpGet]
        public ActionResult AgregarDocente()
        {
            ViewBag.Especialidades = new SelectList(db.Especialidads, "IdEspecialidad", "Especialidad1").ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AgregarDocente(Docente docente)
        {
            if (!ModelState.IsValid)
            {
                return View(docente);
            }
            db.Docentes.Add(docente);
            db.SaveChanges();

            ViewBag.Especialidades = new SelectList(db.Especialidads, "IdEspecialidad", "Especialidad1").ToList();
            return RedirectToAction("ListarDocente");
        }

        [HttpGet]
        public ActionResult AgregarDocenteVM()
        {
            List<string> sexo = new List<string> {
                "Masculino", "Femenino"
            };
            var sexos = sexo.ToList();
            ViewBag.Sexo = new SelectList(sexos);
            ViewBag.Especialidad = new SelectList(db.Especialidads, "IdEspecialidad", "Especialidad1").ToList();
            var docenteviewmodel = new RegistroDocenteViewModel();
            return View(docenteviewmodel);
        }

        [HttpPost]
        public ActionResult AgregarDocenteVM(RegistroDocenteViewModel docenteviewmodel)
        {
            ViewBag.Especialidades = new SelectList(db.Especialidads, "IdEspecialidad", "Especialidad1").ToList();

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var usuario = new Usuario
                    {
                        Nombre = docenteviewmodel.NombreUsuario,
                        Email = docenteviewmodel.EmailUsuario,
                        PasswordHash = docenteviewmodel.PasswordHash,
                    };

                    db.Usuarios.Add(usuario);
                    db.SaveChanges();

                    var docente = new Docente
                    {
                        IdUsuario = usuario.IdUsuario,
                        Nombre = docenteviewmodel.Nombre,
                        Apellido = docenteviewmodel.Apellido,
                        Edad = docenteviewmodel.Edad,
                        Sexo = docenteviewmodel.Sexo,
                        Telefono = docenteviewmodel.Telefono,
                        FechNacimiento = docenteviewmodel.FechNacimiento,
                        IdEspecialidad = docenteviewmodel.IdEspecialidad,
                        Estado = "Pendiente",
                    };
                    
                    var usuarioRol = new UsuarioRol
                    {
                        IdUsuario = usuario.IdUsuario,
                        IdRol = 2,
                    };

                    db.Docentes.Add(docente);
                    db.UsuarioRols.Add(usuarioRol);
                    db.SaveChanges();


                    TempData["RegistroDocenteSuccess"] = $"Docente {docente.Nombre} - {docente.Apellido} registrado correctamente";
                    transaction.Commit();
                    return RedirectToAction("ListarDocente", "Docente");
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    // Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError(string.Empty, "No se pudo registrar el usuario. Inténtelo de nuevo.");

                    // Configurar ViewBag.Sexo nuevamente antes de devolver la vista
                    List<string> sexo = new List<string> {
                        "Masculino", "Femenino"
                    };
                    var sexos = sexo.ToList();
                    ViewBag.Sexo = new SelectList(sexos);
                    ViewBag.Especialidad = new SelectList(db.Docentes, "IdEspecialidad", "Especialidad1").ToList();

                    return View(docenteviewmodel);
                }
            }
        }


        [HttpGet]
        public ActionResult EditarDocente(int iddocente)
        {
            return ViewComponent("EditarDocenteVC", new { iddocente });
        }

        [HttpPost]
        public ActionResult EditarDocente(Docente docente)
        {
            if (!ModelState.IsValid)
            {
                return View(docente);
            }
            var sexos = new List<SelectListItem>
            {
                new SelectListItem { Value = "Masculino", Text = "Masculino" },
                new SelectListItem { Value = "Femenino", Text = "Femenino" }
            };
            
            List<SelectListItem> estados = new List<SelectListItem>
            {
                new SelectListItem {Value = "Activo", Text= "Activo"},
                new SelectListItem {Value = "Inactivo", Text= "Inactivo"}
            };
            ViewBag.Especialidades = new SelectList(db.Especialidads, "IdEspecialidad", "Especialidad1").ToList();
            ViewBag.Sexos = sexos;
            ViewBag.Estados = estados;

            TempData["DocenteEditado"] = $"Docente {docente.Nombre} - {docente.Apellido} editado correctamente";
            db.Docentes.Update(docente);
            db.SaveChanges();
            return RedirectToAction("ListarDocente");
        }

        public async Task<IActionResult> EliminarDocente(int id)
        {
            
            if (ModelState.IsValid)
            {
                Docente docente = await db.Docentes.Where(d => d.IdDocente == id).FirstOrDefaultAsync();

                if (docente != null)
                {
                    docente.Estado = "Inactivo";
                    db.Docentes.Update(docente);
                    await db.SaveChangesAsync();
                    return RedirectToAction("ListarDocente");
                }
                else
                {
                    return RedirectToAction("ListarDocente");
                }
            }

            return RedirectToAction("ListarDocente");
        }

        [HttpGet]
        public List<Docente> listarDocentes()
        {
            return db.Docentes.ToList();
        }
    }
}
