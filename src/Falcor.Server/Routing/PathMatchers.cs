using System;
using System.Collections.Generic;
using System.Linq;

namespace Falcor.Server.Routing
{
    public static class PathMatchers
    {
        private static readonly MatchResult Matching = new MatchingResult();
        private static readonly MatchResult Unmatched = new UnmatchedResult();

        private static PathMatcher PathMatcher<TValue>(Predicate<KeySegment> keyTest, Func<KeySegment, TValue> getValue, Predicate<TValue> valueTest) =>
            key => keyTest(key) && valueTest(getValue(key)) ? Matching : Unmatched;

        private static PathMatcher PatternPathMatcher<TValue>(Predicate<KeySegment> keyTest, Func<KeySegment, TValue> getValue, string name) =>
            key => keyTest(key) ? new MatchingResult(name, getValue(key)) : Unmatched;

        public static PathMatcher BooleanTrue = PathMatcher(k => k.IsBoolean(), k => k.AsBoolean(), b => b);
        public static PathMatcher BooleanFalse = PathMatcher(k => k.IsBoolean(), k => k.AsBoolean(), b => !b);
        public static PathMatcher StringKey(string value) => PathMatcher(k => k.IsString(), k => k.ToString(), s => s == value);
        public static PathMatcher KeySet(params string[] keys) => KeySet(keys.ToList());
        public static PathMatcher KeySet(List<string> keys) => PathMatcher(k => k.IsString(), k => k.ToString(), keys.Contains);
        public static PathMatcher RangesPattern(string name = null) => PatternPathMatcher(k => k.IsRange(), k => k.AsRange(), name);
        public static PathMatcher IntegersPattern(string name = null) => PatternPathMatcher(k => k.IsNumericSet(), k => k.AsNumericSet(), name);
        public static PathMatcher KeysPattern(string name = null) => PatternPathMatcher(k => k.IsKeySet(), k => k.AsKeySet(), name);
    }
}