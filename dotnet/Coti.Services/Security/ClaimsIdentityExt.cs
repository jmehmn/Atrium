using System.Security.Claims;

namespace Coti.Services.Security
{
    public static class ClaimsIdentityExt
    {
        public static string TENANTID = "Coti.TenantId";

        public static void AddTenantId(this ClaimsIdentity claims, object tenantId)
        {
            claims.AddClaim(new Claim(TENANTID, tenantId?.ToString(), null, "Coti"));
        }

        public static bool IsTenantIdClaim(this ClaimsIdentity claims, string claimName)
        {
            return claimName == TENANTID;
        }
    }
}