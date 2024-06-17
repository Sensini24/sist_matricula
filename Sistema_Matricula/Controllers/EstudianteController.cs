using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_Matricula.Models;
using Sistema_Matricula.Validaciones;

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
        public ActionResult ListarEstudiantes()
        {
            var estudiantes = db.Estudiantes.ToList();
            return View(estudiantes);
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
        public ActionResult Details(int id)
        {
            return View();
        }

        

        // POST: EstudianteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EstudianteController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EstudianteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EstudianteController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EstudianteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
