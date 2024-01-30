namespace Hutech.Core.ApiResponse
{
    public class ExecutionResult
    {
        private bool? _success;

        public ExecutionResult()
            : this((ExecutionResult)null)
        {
        }

        public ExecutionResult(ErrorInfo error)
            : this(new[] { error })
        {
        }

        public ExecutionResult(InfoMessage message)
            : this(new[] { message })
        {
        }

        public ExecutionResult(IEnumerable<ErrorInfo> errors)
            : this((ExecutionResult)null)
        {
            foreach (var errorInfo in errors)
                Errors.Add(errorInfo);
        }

        public ExecutionResult(IEnumerable<InfoMessage> messages)
            : this((ExecutionResult)null)
        {
            foreach (var message in messages)
                Messages.Add(message);
        }

        public ExecutionResult(ExecutionResult result)
        {
            if (result != null)
            {
                Success = result.Success;
                Errors = new List<ErrorInfo>(result.Errors);
                Messages = new List<InfoMessage>(result.Messages);
            }
            else
            {
                Errors = new List<ErrorInfo>();
                Messages = new List<InfoMessage>();
            }
        }

        /// <summary>
        ///     Indicates if result is successful.
        /// </summary>
        public bool Success
        {
            get => _success ?? Errors.Count == 0;
            set => _success = value;
        }

        /// <summary>
        ///     Gets a list of errors.
        /// </summary>
        public IList<ErrorInfo> Errors { get; }

        /// <summary>
        /// Informmation messages
        /// </summary>
        public IList<InfoMessage> Messages { get; }
    }


    /// <summary>
    ///     Represents result of an action that returns any value
    /// </summary>
    /// <typeparam name="T">Type of value to be returned with action</typeparam>
    public class ExecutionResult<T> : ExecutionResult
    {
        public ExecutionResult()
            : this((ExecutionResult)null)
        {
        }

        public ExecutionResult(T result)
            : this((ExecutionResult)null)
        {
            Value = result;
        }

        public ExecutionResult(T result, InfoMessage message)
            : this((ExecutionResult)null)
        {
            Value = result;
            Messages.Add(message);
        }

        public ExecutionResult(ExecutionResult result)
            : base(result)
        {
            var r = result as ExecutionResult<T>;
            if (r != null)
                Value = r.Value;
        }

        public ExecutionResult(ErrorInfo error)
            : this(new[] { error })
        {
        }

        public ExecutionResult(InfoMessage message)
            : this(new[] { message })
        {
        }

        public ExecutionResult(IEnumerable<ErrorInfo> errors)
            : this((ExecutionResult)null)
        {
            foreach (var errorInfo in errors)
                Errors.Add(errorInfo);
        }

        public ExecutionResult(IEnumerable<InfoMessage> messages)
            : this((ExecutionResult)null)
        {
            foreach (var message in messages)
                Messages.Add(message);
        }

        public T Value { get; set; }
    }
}
