namespace heitech.MediatorMessenger.Implementation.Messages
{
    public class NullableResponse<T>
       where T : class
    {
        public T WrappedResponse { get; }
        public bool HasValue => WrappedResponse != null;

        public NullableResponse(T wrappedResponse) => WrappedResponse = wrappedResponse;
    }
}
