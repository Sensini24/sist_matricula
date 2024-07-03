using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_Matricula.Models;
using Sistema_Matricula.ViewsModels;
using System.Transactions;

namespace Sistema_Matricula.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly DbMatNotaHorarioContext db;
        private readonly IValidator<RegistroEstudianteViewModel> validadorRegistroEstudiante;

        public UsuarioController(DbMatNotaHorarioContext _db, IValidator<RegistroEstudianteViewModel> _validadorRegistroEstudiante)
        {
            db = _db;
            validadorRegistroEstudiante = _validadorRegistroEstudiante;
        }

        public IActionResult ListarUsuarios()
        {
            var usuarios = db.Usuarios.ToList();
            return View(usuarios);
        }

        public IActionResult RegistrarUsuario()
        {
            //List<SelectListItem> sexo = new List<SelectListItem>{

            //    new SelectListItem { Text = "Masculino", Value = "Masculino" },
            //    new SelectListItem { Text = "Femenino", Value = "Femenino"}
            //};
            //ViewBag.Sexo = sexo;
            List<string> sexo = new List<string> {
                "Masculino", "Femenino"
            };
            var sexos = sexo.ToList();
            ViewBag.Sexo = new SelectList(sexos);
            var UsuarioViewModel = new RegistroEstudianteViewModel();
            return View(UsuarioViewModel);
        }

        [HttpPost]
        public IActionResult RegistrarUsuario(RegistroEstudianteViewModel usuarioViewModel)
        {
            ModelState.Remove(nameof(usuarioViewModel.Estado));

            if (!ModelState.IsValid)
            {
                List<string> sexo = new List<string> {
                    "Masculino", "Femenino"
                };
                var sexos = sexo.ToList();
                ViewBag.Sexo = new SelectList(sexos);
                return View(usuarioViewModel);
            }
            /*
             * Uso de validator
             */

            var validaciones = validadorRegistroEstudiante.Validate(usuarioViewModel);
            if (!validaciones.IsValid)
            {
                foreach (var error in validaciones.Errors)
                {
                    Console.WriteLine(error.PropertyName + ": " + error.ErrorMessage);
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                validaciones.AddToModelState(this.ModelState);
            }
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var estudiante = new Estudiante
                    {
                        Nombre = usuarioViewModel.Nombre,
                        Apellido = usuarioViewModel.Apellido,
                        FechNacimiento = usuarioViewModel.FechNacimiento,
                        Email = usuarioViewModel.Email,
                        Telefono = usuarioViewModel.Telefono,
                        Direccion = usuarioViewModel.Direccion,
                        Estado = "Pendiente",
                        Dni = usuarioViewModel.Dni,
                    };

                    var apoderado = new Apoderado
                    {
                        Nombre = usuarioViewModel.NombreApoderado,
                        Apellido = usuarioViewModel.ApellidoApoderado,
                        Edad = usuarioViewModel.Edad,
                        Sexo = usuarioViewModel.Sexo,
                        Ocupacion = usuarioViewModel.Ocupacion,
                        Telefono = usuarioViewModel.TelefonoApoderado,
                        Direccion = usuarioViewModel.DireccionApoderado,
                    };

                    var usuario = new Usuario
                    {
                        Nombre = usuarioViewModel.NombreUsuario,
                        Email = usuarioViewModel.EmailUsuario,
                        PasswordHash = usuarioViewModel.PasswordHash,
                    };

                    usuario.Email = estudiante.Email;

                    db.Estudiantes.Add(estudiante);
                    db.Apoderados.Add(apoderado);
                    db.Usuarios.Add(usuario);
                    db.SaveChanges();

                    var estudianteAPoderado = new ApoderadoAlumno
                    {
                        IdApoderado = apoderado.IdApoderado,
                        IdEstudiante = estudiante.IdEstudiante,
                        Parentesco = usuarioViewModel.Parentesco,
                    };

                    db.ApoderadoAlumnos.Add(estudianteAPoderado);
                    db.SaveChanges();

                    transaction.Commit();
                    return RedirectToAction("Index", "Home");
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

                    return View(usuarioViewModel);
                }
            }
        }

    }
}
