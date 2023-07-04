using System.Security.Claims;

namespace ShoeShop
{
    public static class ClaimsPrincipalExtensions
    {
        public static string getUserId(this ClaimsPrincipal claims)
        {
            return claims.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
        public static string getUsername(this ClaimsPrincipal claims)
        {
            return claims.FindFirst(ClaimTypes.Name).Value;
        }

    }
}
