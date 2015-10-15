namespace Falcor.Server
{
    public static class RoutingHelpers
    {
        public static IntegersPatternMatcher Integers(string name) => new IntegersPatternMatcher(name);
        public static RangesPatternMatcher Range(string name) => new RangesPatternMatcher(name);
        public static KeysPatternMatcher Keys(string name) => new KeysPatternMatcher(name);
    }
}