using System;
using System.Collections;

namespace Domain.IRepository
{
    public interface IRepository<T> : IDisposable where T : class
    {

        T Get(int id);

        bool Exists(int id);

        void Create(T item);

        void Update(T item);

        void Delete(int id);

    }

    public class Result
    {
        public bool Success { get; }
        public string Error { get; }
        public bool Failure => !Success;

        protected Result(bool success, string error)
        {
            if ((success) != (error != string.Empty))
                throw new InvalidOperationException();

            Success = success;
            Error = error;
        }

        public static Result Fail(string mes)
        {
            return new Result(false, mes);
        }

        public static Result Ok()
        {
            return new Result(true, string.Empty);
        }

        public static Result<Type> Fail<Type>(string mes)
        {
            return new Result<Type>(default(Type), false, mes);
        }

        public static Result<Type> Ok<Type>(Type value)
        {
            return new Result<Type>(value, true, string.Empty);
        }
    }

    public class Result<Type> : Result
    {
        public Type Value { get; set; }

        protected internal Result(Type value, bool success, string error) : base(success, error)
        {
            Value = value;
        }
    }
}