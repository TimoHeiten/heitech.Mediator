namespace heitech.Mediator.Interface
{
    ///<summary>
    /// Register Messenger for the associated Mediator
    ///</summary>
    public interface IRegister
    {
        ///<summary>
        /// Does the Messenger Exist
        ///</summary>
        bool IsRegistered<T>() where T : class;
        ///<summary>
        /// Register a Messenger for this Mediator
        ///</summary>
        void Register<T>(T mediatable) where T : class;
        ///<summary>
        /// Remove a Messenger from this Mediator
        ///</summary>
        void Unregister<T>(T mediatable) where T : class;

        ///<summary>
        /// Find the requested Messenger
        ///</summary>
        T Get<T>();

        IMediator Mediator { get; }
    }
}
