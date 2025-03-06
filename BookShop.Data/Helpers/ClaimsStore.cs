using System.Security.Claims;

namespace BookShop.DataAccess.Helpers
{
    public class ClaimsStore
    {
        public static List<Claim> claims = new()
        {
            //new Claim("Create Book","false"),
            //new Claim("Edit Book","false"),
            //new Claim("Delete Book","false"),
        };
    }
}
