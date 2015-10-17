using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using static System.String;

namespace Falcor.Server
{
    public delegate Matching<TValue> PathMatcher<TValue>(FalcorPath matched, FalcorPath unmatched);

    public delegate MatchResult PathMatcher(KeySegment key);

    public abstract class MatchResult
    {
        public abstract bool IsMatched { get; }
        public abstract bool HasName { get; }
        public abstract string Name { get; }
    }

    public class UnmatchedResult : MatchResult
    {
        public override bool IsMatched => false;
        public override bool HasName => false;
        public override string Name { get { throw new InvalidOperationException(); } }
    }

    public class MatchingResult : MatchResult
    {
        public MatchingResult(string name = null)
        {
            Name = name;
            HasName = name != null;
        }

        public override bool IsMatched => true;
        public override bool HasName { get; }
        public override string Name { get; }
    }

    public static class PathMatchers
    {

        public static PathMatcher<bool> BooleanKey = Match(k => k.IsBoolean(), k => k.AsBoolean());
        public static PathMatcher<string> StringKey = Match(k => k.IsString(), k => k.ToString());
        public static PathMatcher<long> NumberKey = Match(k => k.IsNumber(), k => k.AsLong());
        public static PathMatcher<KeySet> KeySet = Match(k => k.IsKeySet(), k => k.AsKeySet());


        public static PathMatcher<ISet<string>> StringSet = KeySet.SelectMany(keySet =>
            keySet.Any(key => !key.IsString()) ? Optional.None<ISet<string>>()
            : Optional.Some(new HashSet<string>(keySet.Select(k => k.ToString()))));

        public static PathMatcher<NumberRange> NumberRange = Match(k => k.IsRange(), k => k.AsRange());
        public static PathMatcher<NumericSet> NumericSet = Match(k => k.IsNumericSet(), k => k.AsNumericSet());

        public static PathMatcher<bool> Key(bool value) => BooleanKey.Where(v => v == value);
        public static PathMatcher<string> Key(string value) => StringKey.Where(v => v == value);



        public static PathMatcher<TKey> Match<TKey>(Predicate<KeySegment> predicate, Func<KeySegment, TKey> getValue)
        {
            return (matched, unmatched) =>
            {
                KeySegment head;
                if (unmatched.Any() && predicate(head = unmatched.Head))
                {
                    var value = getValue(head);
                    return new Match<TKey>(matched.Append(head), unmatched.Tail, value);
                }
                return NoMatch<TKey>.Instance;
            };
        }
    }
}