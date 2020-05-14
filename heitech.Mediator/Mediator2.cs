using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace heitech.Mediator
{
    ///<summary>
    /// Implementation Mediator
    ///</summary>
    internal class Mediator2 : IMediator
    {
        private readonly List<IHasMediatorKey> _keylets;
        public Mediator2(IEnumerable<IHasMediatorKey> outlet)
        {
            var dic = BuildKeys(outlet);
            _keylets = dic.Select(x => x.Value).ToList();
        }

        #region FillMap
        private Dictionary<string, IHasMediatorKey> BuildKeys(IEnumerable<IHasMediatorKey> keys)
        {
            var map = new Dictionary<string, IHasMediatorKey>();

            Action<string, IHasMediatorKey> addToMap = (key, item) => 
            {
                // Guard
                if (key == null) 
                    return;

                if (!map.ContainsKey(key))
                {
                    map.Add(key, item);
                }
            };

            foreach (var item in keys)
            {
               addToMap(FromOutlet(item), item);
               addToMap(FromTypedOutlet(item), item);
               addToMap(FromTypedResultOutlet(item), item);
            }
            return map;
        }

        private string FromOutlet(IHasMediatorKey key)
        {
            var o = key as IOutlet;
            return o == null ? null : o.OutletKey;
        }

        private string FindInterfaceGenericTypeArguments(IHasMediatorKey key, int compareTo = 1, Func<Type[], string> converter = null)
        {
            var interfaces = key.GetType().GetInterfaces();
            var allAvailable = interfaces.Select(x => x.GetGenericArguments());

            var types = allAvailable.FirstOrDefault(x => 
            {
                return x.Length == compareTo;
            });

            if (converter == null)
                converter = t => t.First().Name;

            return types == null ? null : converter(types);
        }

        private string FromTypedOutlet(IHasMediatorKey key)
        {
            return FindInterfaceGenericTypeArguments(key);
        }

        private string FromTypedResultOutlet(IHasMediatorKey key)
        {
            Func<Type[], string> converter = (types) =>  $"{types.First().Name}-{types.Last().Name}";
            return FindInterfaceGenericTypeArguments(key, compareTo: 2, converter: converter);
        }
        #endregion

        public async Task<OperationResult> ExecuteOutletAsync(string key)
        {
            var outlet = _keylets.FirstOrDefault(x => FromOutlet(x) == key && x is IOutlet);
            if (outlet == null)
            {
                return OperationResult.Failure
                (
                    ResultType.InternalError,
                    new 
                    {
                        Message = "Did not find any Outlet with Key " + key
                    }
                );
            }
            else
            {
                var o = outlet as IOutlet;
                return await o.ExecuteCommandAsync();
            }
        }

        public async Task<OperationResult> ExecuteOutletAsync<T>(T outletInput)
        {
            var first = _keylets.FirstOrDefault(x => 
            {
                string key = FromTypedOutlet(x);
                return key == outletInput.GetType().Name;
            }
            );
            var outlet = first as IOutlet<T>;
            if (outlet != null)
            {
                var result = await outlet.ExecuteOperationAsync(outletInput);
                return result;
            }
            else return OperationResult.Failure
            (
                ResultType.InternalError, 
                new 
                {
                    Message = "no outlet found for " + typeof(T).FullName
                }
            );
        }

        public async Task<OperationResult<R>> ExecuteOutletAsync<T, R>(T outletInput) 
            where R : class
        {
            var first = _keylets.FirstOrDefault
            (
                x => FromTypedResultOutlet(x) == $"{outletInput.GetType().Name}-{typeof(R).Name}"
            );
            var outlet = first as IOutlet<T,R>;
            if (outlet != null)
            {
                return await outlet.ExecuteFunctionAsync(outletInput);
            }
            else 
            {
                return OperationResult<R>.Failed
                (
                    ResultType.InternalError
                );
            }
        }

        
    }
}