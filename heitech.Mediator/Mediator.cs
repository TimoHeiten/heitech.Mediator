using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace heitech.Mediator
{
    ///<summary>
    /// Implementation Mediator
    ///</summary>
    internal class Mediator : IMediator
    {
        private readonly List<IHasMediatorKey> _keylets;
        public Mediator(IEnumerable<IHasMediatorKey> outlet)
        {
            _keylets = outlet.ToList();
        }

        private string AvoidAccidentalClash() => Guid.NewGuid().ToString("N");

        public async Task<OperationResult> ExecuteOutletAsync<T>(T outletInput)
        {
            var obj = await Run<IOutlet<T>, OperationResult>
            (
                typeof(T).FullName,
                outletKey: (_outlet) => 
                { 
                    if (_outlet is IOutlet<T> o)
                    {
                        return (typeof(T)).FullName;
                    }
                    // make sure no accidental match happens
                    return AvoidAccidentalClash();
                },
                action: outlet => outlet.ExecuteOperationAsync(outletInput),
                onError: "Did not find any Outlets for Type [" + typeof(T).FullName + "]"
            );
            var result = obj as OperationResult;

            return result != null 
                   ? result 
                   : OperationResult.Failure
                   (
                       ResultType.InternalError,
                       new
                       {
                           Message = "Failed to Run the Outlet"
                       }
                   );
        }

        public async Task<OperationResult<R>> ExecuteOutletAsync<T, R>(T outletInput) where R : class
        {
            var obj = await Run<IOutlet<T, R>, OperationResult<R>>
            (
                typeof(T).FullName,
                outletKey: (_outlet) => 
                { 
                    if (_outlet is IOutlet<T, R> o)
                    {
                        return (typeof(T)).FullName + typeof(R).FullName;
                    }
                    // make sure no accidental match happens
                    return AvoidAccidentalClash();
                },
                action: outlet => outlet.ExecuteFunctionAsync(outletInput),
                onError: "Did not find any Outlets for Type " 
                         + "[" + typeof(T).FullName + "]"
                         + " & Result "
                         + "[" + typeof(R).FullName + "]"
            );
            var result = obj as OperationResult<R>;
            return null;
        }

        public async Task<OperationResult> ExecuteOutletAsync(string key)
        {
            var obj = await Run<IOutlet, OperationResult>
            (
                key: key,
                outletKey: (_outlet) => 
                {
                    if (_outlet is IOutlet o)
                    {
                        return o.OutletKey;
                    }
                    // make sure no accidental match happens
                    return AvoidAccidentalClash();
                },
                action: outlet => outlet.ExecuteCommandAsync(),
                onError: "Did not find any Outlets for key: [" + key+ "]" 
            );
            var result =  obj as OperationResult;

            return result != null 
                   ? result 
                   : OperationResult.Failure
                   (
                       ResultType.InternalError,
                       new
                       {
                           Message = "Failed to Run the Outlet"
                       }
                   );
        }

        private async Task<object> Run<T, O>(string key, Func<T, Task<O>> action, 
                                                      Func<IHasMediatorKey, string> outletKey, string onError)
            where T : IHasMediatorKey
            where O : OperationResult
        {
            var (can, casted) = CastTo<T>(key, outletKey);
            if (can)
            {
                return OperationResult.Success(await action(casted));
            }
            else return OperationResult.Failure
            (
                ResultType.InternalError, 
                new 
                {
                    Message = onError
                }
            );
        }

        private (bool canCast, T result) CastTo<T>(string key, Func<IHasMediatorKey, string> outletKey)
        {
            var outlet = _keylets.FirstOrDefault(x => key == outletKey(x));

            bool canCast = outlet != null;

            return (canCast, canCast ? (T)outlet : default(T));
        }
    }
}