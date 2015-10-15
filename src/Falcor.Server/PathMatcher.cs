namespace Falcor.Server
{
    public delegate IMatching<TRequest> PathMatcher<TRequest>(FalcorPath matched, FalcorPath unmatched);
}