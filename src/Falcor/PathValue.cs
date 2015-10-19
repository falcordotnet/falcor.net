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
    }
}