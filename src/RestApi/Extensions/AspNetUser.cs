using DevIO.Business.Intefaces;
using System.Security.Claims;

namespace DevIO.Api.Extensions
{
    public class AspNetUser : IUser     //Representação do Usuário / referencia a camada de negócios - Interface IUser
    {
        private readonly IHttpContextAccessor _accessor;//Acessar o HTTPContext, porém ela pertence ao asp.net core. Não é bom injetá-la diretamente na camada de negócio 

        public AspNetUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string Name => _accessor.HttpContext.User.Identity.Name;//Aqui posso Usar pra pegar outras infos do HttpContext além do user para usar em outras camdas

        public Guid GetUserId()
        {
            return IsAuthenticated() ? Guid.Parse(_accessor.HttpContext.User.GetUserId()) : Guid.Empty;
        }

        public string GetUserEmail()
        {
            return IsAuthenticated() ? _accessor.HttpContext.User.GetUserEmail() : "";
        }

        public bool IsAuthenticated()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public bool IsInRole(string role)
        {
            return _accessor.HttpContext.User.IsInRole(role);
        }

        public IEnumerable<Claim> GetClaimsIdentity()
        {
            return _accessor.HttpContext.User.Claims;
        }
    }

    public static class ClaimsPrincipalExtensions //Extensions Methods do ClaimsPrincipal - Metodos especificos que o HTTPCOntext não da suporte tão facilitado, acessando então dessa forma
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value;
        }

        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst(ClaimTypes.Email);
            return claim?.Value;
        }
    }
}