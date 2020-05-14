using System.Threading.Tasks;

namespace heitech.Mediator
{
    ///<summary>
    /// Outlet as target for Mediator calls. Implement to automatically scan for those outlets
    ///</summary>
    public interface IOutlet : IHasMediatorKey
    {
        string OutletKey { get; }
        ///<summary>
        /// Outlet command without any parameters
        ///</summary>
        Task<OperationResult> ExecuteCommandAsync();
    }

    ///<summary>
    /// Marker interface 
    ///</summary>
    public interface IHasMediatorKey
    { }

    ///<summary>
    /// Typed version of the Outlet.  Implement to automatically scan for those outlets
    ///</summary>
    public interface IOutlet<T> : IHasMediatorKey
    {
        ///<summary>
        /// Outlet execute with the Message of Type T
        ///</summary>
        Task<OperationResult> ExecuteOperationAsync(T obj);
    }

    public interface IOutlet<T, R> : IHasMediatorKey
        where R : class
    {
        ///<summary>
        /// Outlet execute with the Message of Type T and Result of R
        ///</summary>
        Task<OperationResult<R>> ExecuteFunctionAsync(T obj);
    }
}