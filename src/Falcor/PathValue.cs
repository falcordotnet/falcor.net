namespace Falcor
{
    public class PathValue
    {
        public PathValue(FalcorPath path, FalcorValue value)
        {
            Path = path;
            Value = value;
        }

        public FalcorPath Path { get; }
        public FalcorValue Value { get; }
    }
}