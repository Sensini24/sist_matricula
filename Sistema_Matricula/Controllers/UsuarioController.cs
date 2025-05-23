﻿using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_Matricula.Models;
using Sistema_Matricula.ViewsModels;
using System.Security.Claims;
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

        public IActionResult GetUserDetails()
        {
            var username = User.Identity.Name;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;  // Obtener el rol

            return Ok(new { Username = username, UserId = userId, UserRole = userRole });
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
                    var usuario = new Usuario
                    {
                        Nombre = usuarioViewModel.NombreUsuario,
                        Email = usuarioViewModel.EmailUsuario,
                        PasswordHash = usuarioViewModel.PasswordHash,
                    };

                    db.Usuarios.Add(usuario);
                    db.SaveChanges();


                    var estudiante = new Estudiante
                    {
                        Nombre = usuarioViewModel.Nombre,
                        Apellido = usuarioViewModel.Apellido,
                        Sexo = usuarioViewModel.SexoEstudiante,
                        FechNacimiento = usuarioViewModel.FechNacimiento,
                        Direccion = usuarioViewModel.Direccion,
                        Estado = "Pendiente",
                        Dni = usuarioViewModel.Dni,
                        IdUsuario = usuario.IdUsuario,
                    };

                    var apoderado = new Apoderado
                    {
                        Nombre = usuarioViewModel.NombreApoderado,
                        Apellido = usuarioViewModel.ApellidoApoderado,
                        FechNacimiento = usuarioViewModel.FechNacimientoApoderado,
                        Sexo = usuarioViewModel.Sexo,
                        Ocupacion = usuarioViewModel.Ocupacion,
                        Telefono = usuarioViewModel.TelefonoApoderado,
                        Direccion = usuarioViewModel.DireccionApoderado,
                    };

                    

                    var usuarioRol = new UsuarioRol
                    {
                        IdUsuario = usuario.IdUsuario,
                        IdRol = 3,
                    };

                    db.Estudiantes.Add(estudiante);
                    db.Apoderados.Add(apoderado);
                    db.UsuarioRols.Add(usuarioRol);
                    db.SaveChanges();

                    var estudianteAPoderado = new ApoderadoAlumno
                    {
                        IdApoderado = apoderado.IdApoderado,
                        IdEstudiante = estudiante.IdEstudiante,
                        Parentesco = usuarioViewModel.Parentesco,
                    };

                    db.ApoderadoAlumnos.Add(estudianteAPoderado);
                    db.SaveChanges();
                    TempData["RegistroEstudianteSuccess"] = $"Estudiante {estudiante.Nombre} - {estudiante.Apellido} registrado correctamente";
                    transaction.Commit();
                    return RedirectToAction("ListarEstudiantes", "Estudiante");
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
