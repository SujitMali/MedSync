using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace MedSync_API.Controllers
{
    public class BaseApiController : ApiController
    {
        private ClaimsIdentity IdentityClaims => User?.Identity as ClaimsIdentity;

        protected int CurrentUserId
        {
            get
            {
                var claim = IdentityClaims?.FindFirst(ClaimTypes.NameIdentifier);
                if (claim != null && int.TryParse(claim.Value, out var id))
                    return id;
                return 0;
            }
        }

        protected int CurrentDoctorId
        {
            get
            {
                var claim = IdentityClaims?.Claims
                    ?.FirstOrDefault(c => c.Type.Equals("DoctorID", System.StringComparison.OrdinalIgnoreCase));

                if (claim != null && int.TryParse(claim.Value, out var id))
                    return id;

                return 0;
            }
        }

        protected string CurrentEmail
        {
            get
            {
                var claim = IdentityClaims?.FindFirst(ClaimTypes.Email);
                return claim?.Value;
            }
        }

        protected string CurrentRole
        {
            get
            {
                var claim = IdentityClaims?.FindFirst(ClaimTypes.Role);
                return claim?.Value;
            }
        }
    
    }
}
