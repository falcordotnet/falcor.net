namespace Falcor.Server
{
    public static class FalcorPathExtensions
    {
        public static PathValue Atom(this FalcorPath path, object value, int? size = null, int? expires = null) => new PathValue(path, new Atom(value, size, expires));
        public static PathValue Ref(this FalcorPath path, params KeySegment[] keys) => new PathValue(path, new Ref(new FalcorPath(keys)));
        public static PathValue Error(this FalcorPath path, string error) => new PathValue(path, new Error(error));
        public static PathValue Undefined(this FalcorPath path) => new PathValue(path, null);
    }
}