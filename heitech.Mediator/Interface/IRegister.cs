namespace heitech.Mediator.Interface
{
    public interface IRegister
    {
        bool IsRegistered<T>() where T : class;
        void Register<T>(T mediatable) where T : class;
        void Unregister<T>(T mediatable) where T : class;

        T Get<T>();

        IMediator Mediator { get; }
    }
}
