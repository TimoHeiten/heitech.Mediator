using System.Collections.Generic;

namespace heitech.Mediator
{
    public static class Extensions
    {
        ///<summary>
        /// In case you used more than one Outlet for an OperationResult with the same Key or type
        ///</summary>
        public static IEnumerable<OperationResult> AsCollection(this OperationResult result)
        {
            if (result.TryGet<OperationResult[]>(out OperationResult[] opResult))
            {
                return opResult;
            }
            return new [] { result };
        }

        ///<summary>
        /// In case you used more than one Outlet for an OperationResult with the same Key or type
        ///</summary>
        public static IEnumerable<OperationResult<T>> AsCollection<T>(this OperationResult<T> result)
            where T : class
        {
            if (result.TryGet<OperationResult<T>[]>(out OperationResult<T>[] opResult))
            {
                return opResult;
            }
            return new [] { result };
        }
    }
}