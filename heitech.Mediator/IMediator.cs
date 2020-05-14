using System.Threading.Tasks;

namespace heitech.Mediator
{
    ///<summary>
    /// Interface of mediator, to reach all outlets. Outlets with same Types are always addressed like in broadcasting messages
    ///</summary>
    public interface IMediator
    {

        ///<summary>
        /// Call by Key
        ///</summary>
        Task<OperationResult> ExecuteOutletAsync(string key);

        ///<summary>
        /// Execute the Outlet which takes Type T as input and gives untyped Output of OperationResult
        ///</summary>
        Task<OperationResult> ExecuteOutletAsync<T>(T outletInput);

        ///<summary>
        /// Execute the Outlet which takes Type T as input and gives Typed OutputResult
        ///</summary>
        Task<OperationResult<R>> ExecuteOutletAsync<T, R>(T outletInput)
            where R : class;
    }
}