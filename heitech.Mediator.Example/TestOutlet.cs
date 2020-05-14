using System.Threading.Tasks;

namespace heitech.Mediator.Example
{
    public class TestOutlet : IOutlet, 
                              IOutlet<OutletType>,
                              IOutlet<OutletType, OutletResult>
    {
        public async Task<OperationResult> ExecuteCommandAsync()
        {
            await Task.Delay(10);

            return OperationResult.Success(new { Message = "Outlet one is working fine "});
        }

        public Task<OperationResult> ExecuteOperationAsync(OutletType obj)
        {
            System.Console.WriteLine("Outlet with type also works");
            return Task.FromResult<OperationResult>(OperationResult.Success(obj));
        }

        public Task<OperationResult<OutletResult>> ExecuteFunctionAsync(OutletType obj)
        {
            System.Console.WriteLine("calling typed operationresult");
            return Task.FromResult
            (
                OperationResult<OutletResult>.Success
                (
                    new OutletResult 
                    {
                         ResultMessage = "result from typed operationresult"
                    }
                )
            );

        }

        string IOutlet.OutletKey => "test";

    }

    public class BroadcastedTo : IOutlet, IOutlet<string>
    {
        public string OutletKey => "BroadCastTo";

        public Task<OperationResult> ExecuteCommandAsync()
        {
            return Task.FromResult
            (
                OperationResult.Success
                (
                    new 
                    { 
                        Message = "Broadcast is working fine "
                    }
                )
            );
        }

        public Task<OperationResult> ExecuteOperationAsync(string obj)
        {
             System.Console.WriteLine("Broadcast with type also works");
            return Task.FromResult<OperationResult>
            (
                OperationResult.Success(obj)
            );
        }
    }

    public class OutletType 
    { 
        public string Message { get; set; } 
        public override string ToString()
        {
            return $"{this.GetType().Name} - Message [" + Message + "]";
        }
    } 

    public class OutletResult
    { 
        public string ResultMessage { get; set; } 
        public override string ToString()
        {
            return $"{this.GetType().Name} - Message [" + ResultMessage + "]";
        }
    } 
}