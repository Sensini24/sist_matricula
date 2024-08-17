using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Sistema_Matricula.Models;
using Sistema_Matricula.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sistema_Matricula.Controllers
{
    public class AuthController : Controller
    {
        List<string> _tokenBlacklist = new List<string>();

        private readonly IConfiguration configuration;
        private readonly DbMatNotaHorarioContext db;

        public AuthController(DbMatNotaHorarioContext _db, IConfiguration _configuration)
        {
            db = _db;
            configuration = _configuration;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(ViewModelUsuario usuarioviewmodel)
        {
            UtilAuthentication util = new UtilAuthentication(db);
            Usuario usuarioActual = util.buscarUsuario(usuarioviewmodel.Email, usuarioviewmodel.Password);

            if (usuarioActual != null)
            {
                var token = GenerateJwtToken(usuarioActual);

                // Configurar la cookie
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddDays(int.Parse(configuration["Jwt:ExpireDays"])),
                    Secure = true,
                    SameSite = SameSiteMode.Lax
                };

                Response.Cookies.Append("AuthToken", token, cookieOptions);

                
                if(ObtenerClaimsInfo.GetUserRole(User) == "Docente")
                {
                    return RedirectToAction("Dashboard", "DashboardDocente");
                }

                if (ObtenerClaimsInfo.GetUserRole(User) == "Estudiante")
                {
                    return RedirectToAction("Dashboard", "DashboardEstudiante");
                }

                if (ObtenerClaimsInfo.GetUserRole(User) == "Administrador")
                {
                    return RedirectToAction("Dashboard", "DashboardAdministrador");
                }
            }

            TempData["ErrorAutenticacion"] = "Usuario o contraseña incorrecta";
            return View(usuarioviewmodel);
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();
            var consulta = from u in db.Usuarios
                           where u.Email == usuario.Email
                           select new
                           {
                               u.Nombre,
                               u.IdUsuario
                           };

            var nombre = consulta.ToArray()[0].Nombre;
            var idUsuario = consulta.ToArray()[0].IdUsuario;
            var rolesUsuario = from u in db.Usuarios
                               join Ur in db.UsuarioRols on u.IdUsuario equals Ur.IdUsuario
                               join r in db.Rols on Ur.IdRol equals r.IdRol
                               where u.IdUsuario == idUsuario
                               select new
                               {
                                   r.Nombre
                               };
            var roles = rolesUsuario.ToArray()[0].Nombre;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Name, nombre.ToString()),
                new Claim(ClaimTypes.NameIdentifier, idUsuario.ToString()),
                new Claim(ClaimTypes.Role, roles.ToString())
            };


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(int.Parse(configuration["Jwt:ExpireDays"])),
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public IActionResult Salir()
        {
            var token = HttpContext.Request.Cookies["AuthToken"];
            if (!string.IsNullOrEmpty(token))
            {
                // Agrega el token a la lista negra
                _tokenBlacklist.Add(token);

                // Elimina la cookie
                Response.Cookies.Delete("AuthToken");
                Console.WriteLine("Logout exitoso");
            }
            return RedirectToAction("Login");
        }

        public IActionResult ObtenerDatosCompletos()
        {
            var userId = ObtenerClaimsInfo.GetUserId(User);
            var nombreUsuario = ObtenerClaimsInfo.GetUserName(User);
            var rolUsuario = ObtenerClaimsInfo.GetUserRole(User);
            var allClaims = ObtenerClaimsInfo.GetAllClaims(User);

            return Ok(new
            {
                userId,
                nombreUsuario,
                rolUsuario,
                Claims = allClaims.Select(c => new { c.Type, c.Value })
            });
        }
    }
}
