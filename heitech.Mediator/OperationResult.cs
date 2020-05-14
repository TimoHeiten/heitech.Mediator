using System;

namespace heitech.Mediator
{
    ///<summary>
    /// Object that describes the result of an operation.
    ///</summary>
    public class OperationResult
    {
        ///<summary>
        /// The Wrapped Result
        ///</summary>
        public object Result { get; protected internal set; }
        ///<summary>
        /// Tells if the operation was successfull
        ///</summary>
        public bool IsSuccess { get; protected internal set; }
        ///<summary>
        /// If not successfull, contains the exception
        ///</summary>
        public Exception Error { get; protected internal set; }
        ///<summary>
        /// Marks the Result in an error code style, similar to HttpCodes
        ///</summary>
        public ResultType ResultType { get; protected internal set; }

        public bool TryGet<T>(out T obj)
        {
            obj = default(T);
            if (Result != null && Result.GetType() == typeof(T))
            {
                obj = (T)Result;
                return true;
            } 
            return false;
        }

        protected internal OperationResult()
        { }

        ///<summary>
        /// Static factory method: Indicating Success of operation
        ///</summary>
        public static OperationResult Success(object obj)
        {
            var result = Create(ResultType.Ok, obj);
            result.IsSuccess = true;
            return result;
        }


        private static OperationResult Create(ResultType t, object o = null, Exception e = null)
            => new OperationResult
            {
                Error = e,
                Result = o,
                ResultType = t
            };

        ///<summary>
        /// Static factory method: Indicating Failure without an exception
        ///</summary>
        public static OperationResult Failure(ResultType type, object obj)
            => Create(type, obj);

        ///<summary>
        /// Static factory method: Indicating Failure with an exception
        ///</summary>
        public static OperationResult Failure(ResultType type, Exception ex)
            => Create(type, e: ex);

        ///<summary>
        /// Static factory method: Indicating Failure with an exception and a describing object
        ///</summary>
        public static OperationResult Failure(ResultType type, Exception ex, object obj)
            => Create(type, obj, ex);
    }

    public enum ResultType
    {
        BadRequest,
        NoContent,
        NotFound,
        InternalError,
        Created,
        Forbidden,
        Unauthorized,
        Ok
    }

    ///<summary>
    ///Generic version of the OperationResult
    ///</summary>
    public class OperationResult<T> : OperationResult
        where T : class
    {
        public T Value => (T)Result;

        ///<summary>
        /// Static factory method: Indicating Success of operation
        ///</summary>
        public static OperationResult<T> Success(T obj)
        {
            var success = Create(ResultType.Ok, obj);
            success.IsSuccess = true;
            return success;
        }

        private static OperationResult<T> Create(ResultType t, object o = null, Exception e = null)
            => new OperationResult<T>
            {
                Error = e,
                Result = o,
                ResultType = t
            };

        ///<summary>
        /// Static factory method: Indicating Failure without an exception
        ///</summary>
        public static OperationResult<T> Failed(ResultType type)
            => Create(type, new { Message = "failed: " + typeof(T).Name});
    }
}