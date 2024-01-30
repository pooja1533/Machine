namespace Hutech.Core.ApiResponse
{
    public  class GenericExecutionResult<T> : ExecutionResult
    {

        public GenericExecutionResult(T result)
            : this((ExecutionResult)null)
        {
            Value = result;
        }

        public GenericExecutionResult(T result, InfoMessage message)
            : this((ExecutionResult)null)
        {
            Value = result;
            Messages.Add(message);
        }

        public GenericExecutionResult(ExecutionResult result)
            : base(result)
        {
            var r = result as GenericExecutionResult<T>;
            if (r != null)
                Value = r.Value;
        }

        public GenericExecutionResult(ErrorInfo error)
            : this(new[] { error })
        {
        }

        public GenericExecutionResult(InfoMessage message)
            : this(new[] { message })
        {
        }

        public GenericExecutionResult(IEnumerable<ErrorInfo> errors)
            : this((ExecutionResult)null)
        {
            foreach (var errorInfo in errors)
                Errors.Add(errorInfo);
        }

        public GenericExecutionResult(IEnumerable<InfoMessage> messages)
            : this((ExecutionResult)null)
        {
            foreach (var message in messages)
                Messages.Add(message);
        }

        public T Value { get; set; }
    }
}
