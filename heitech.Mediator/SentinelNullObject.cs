using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace heitech.Mediator
{
    ///<summary>
    /// If no Sentinel should be used this is the go to way
    ///</summary>
    public class SentinelNullObject : ISentinel
    {
        public Task<bool> HasUserClaimsAsync(IPrincipal principal, Claim[] mustHaveClaims)
        {
            return Task.FromResult(true);
        }

        public Task<bool> HasUserRightsAsync(IPrincipal principal, string[] mustHaverights)
        {
            return Task.FromResult(true);
        }
    }
}