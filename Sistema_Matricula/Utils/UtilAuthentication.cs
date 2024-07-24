using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Sistema_Matricula.Models;

namespace Sistema_Matricula.Utils
{
    public class UtilAuthentication
    {
        private readonly DbMatNotaHorarioContext db;

        public UtilAuthentication(DbMatNotaHorarioContext _db)
        {
            db = _db;
        }

        public Usuario buscarUsuario(string email, string contrasena)
        {
            Usuario usuario =  db.Usuarios.FirstOrDefault(u => u.Email == email && u.PasswordHash == contrasena);
            return usuario;
        }
    }
}
