namespace Falcor.Server
{
    internal abstract class Optional<T> : IOption<T>
    {
        public abstract bool IsEmpty { get; }

        public bool IsDefined => !IsEmpty;

        public T GetOrDefault() => IsEmpty ? default(T) : Get();

        public abstract T Get();
    }

    public static class Optional
    {
        public static IOption<TOption> None<TOption>() => new None<TOption>();
        public static IOption<TOption> Some<TOption>(TOption value) => new Some<TOption>(value);
    }
}