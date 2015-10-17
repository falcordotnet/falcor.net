namespace Falcor.Server
{
    internal sealed class Some<T> : Optional<T>
    {
        private readonly T _value;

        public Some(T value)
        {
            _value = value;
        }

        public override bool IsEmpty => false;

        public override T Get() => _value;
    }
}