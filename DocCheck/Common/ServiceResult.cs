using System.Diagnostics.CodeAnalysis;

namespace DocCheck.Common
{
    public class ServiceResult<T>
    {
        // Success constructor
        private ServiceResult(T value)
        {
            IsSuccess = true;
            Value = value;
            Error = null;
        }

        // Failure constructor
        private ServiceResult(Error error)
        {
            IsSuccess = false;
            Value = default;
            Error = error;
        }

        [MemberNotNullWhen(true, nameof(Value))]
        [MemberNotNullWhen(false, nameof(Error))]
        public bool IsSuccess { get; }
        public T? Value { get; }
        public Error? Error { get; }

        // Helper methods for constructing the `Result<T>`
        public static ServiceResult<T> Success(T value) => new(value);
        public static ServiceResult<T> Fail(Error error) => new(error);

        // Allow converting a T directly into Result<T>
        public static implicit operator ServiceResult<T>(T value) => Success(value);
        public static implicit operator ServiceResult<T>(Error error) => Fail(error);
    }

}
