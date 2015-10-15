namespace Falcor
{
    public sealed class NullKey : SimpleKey
    {
        public override KeyType KeyType { get; } = KeyType.Null;
    }
}