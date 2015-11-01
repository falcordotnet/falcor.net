namespace Falcor
{
    public class PathValue
    {
        public PathValue(FalcorPath path, object value)
        {
            Path = path;
            Value = value;
        }

        public FalcorPath Path { get; }
        //public FalcorValue Value { get; }
        public object Value { get; }

        protected bool Equals(PathValue other)
        {
            return Equals(Path, other.Path) && Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((PathValue) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Path != null ? Path.GetHashCode() : 0)*397) ^ (Value != null ? Value.GetHashCode() : 0);
            }
        }
    }
}