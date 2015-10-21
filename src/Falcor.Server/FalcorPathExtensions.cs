namespace Falcor.Server
{
    public static class FalcorPathExtensions
    {
        public static PathValue Value(this FalcorPath path, object value) => new PathValue(path, value);

        public static PathValue Error(this FalcorPath path, string error) => new PathValue(path, new Error(error));

        public static PathValue Undefined(this FalcorPath path) => new PathValue(path, null);
    }
}