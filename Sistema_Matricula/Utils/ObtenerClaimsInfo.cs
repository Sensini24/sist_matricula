using System.Security.Claims;

namespace Sistema_Matricula.Utils
{
    public class ObtenerClaimsInfo
    {
        public static string GetUserId(ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public static string GetUserName(ClaimsPrincipal user)
        {
            return user.Identity.Name;
        }

        public static string GetUserRole(ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Role)?.Value;
        }

        public static IEnumerable<Claim> GetAllClaims(ClaimsPrincipal user)
        {
            return user.Claims;
        }
    }
}
