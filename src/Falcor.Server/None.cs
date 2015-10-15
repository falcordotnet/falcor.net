using System;

namespace Falcor.Server
{
    internal sealed class None<T> : AbstractOption<T>
    {
        public override bool IsEmpty => true;

        public override T Get()
        {
            throw new InvalidOperationException("Cannot get value from None.");
        }
    }
}