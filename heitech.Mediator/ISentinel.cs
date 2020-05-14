using System.Security.Principal;
using System.Threading.Tasks;

namespace heitech.Mediator
{
    ///<summary>
    /// Interface for Claim watching sentinel
    ///</summary>
    public interface ISentinel
    {
        Task<bool> HasUserRightsAsync(IPrincipal principal, string[] mustHaverights);
        Task<bool> HasUserClaimsAsync(IPrincipal principal, System.Security.Claims.Claim[] mustHaveClaims);
    }
}