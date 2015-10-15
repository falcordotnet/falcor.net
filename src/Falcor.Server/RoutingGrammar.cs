using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Xml;
using Sprache;
using static Sprache.Parse;

namespace Falcor.Server
{
    public static class RoutingGrammar
    {
        static readonly Parser<char> Comma = Char(',');
        static readonly Parser<char> Dot = Char('.');
        public static readonly Parser<char> OpeningBracket = Char('[');
        public static readonly Parser<char> ClosingBracket = Char(']');
        public static readonly Parser<char> OpeningBrace = Char('{');
        public static readonly Parser<char> ClosingBrace = Char('}');
        public static readonly Parser<char> SingleQuote = Char('\'');
        public static readonly Parser<char> DoubleQuote = Char('"');

        public static readonly Parser<string> SingleQuotedString =
            from openQuote in SingleQuote
            from content in LetterOrDigit.Except(SingleQuote).Many().Text()
            from closeQuote in SingleQuote
            select content;

        public static readonly Parser<string> DoubleQuotedString =
            from openQuote in DoubleQuote
            from content in LetterOrDigit.Except(DoubleQuote).Many().Text()
            from closeQuote in DoubleQuote
            select content;

        public static readonly Parser<StringKeyMatcher> KeyMatcher =
            from key in Letter.AtLeastOnce().Text()
            select new StringKeyMatcher(key);

        //public static readonly Parser<PathMatcher> Keys =
        //    from open in Char('[')
        //    from collectionContent in SingleQuotedString.Many()
        //    select new StringKeyMatcher("");

        public static readonly Parser<IEnumerable<string>> CommaSeperatedSingleQuotedStrings =
            from first in SingleQuotedString.Token()
            from rest in Comma.Then(_ => SingleQuotedString.Token()).Many()
            select Cons(first, rest);

        public static readonly Parser<PathMatcher> KeySet =
            from openingBracket in OpeningBracket
            from keys in CommaSeperatedSingleQuotedStrings
            from closingBracket in ClosingBracket 
            select new KeySetMatcher(new KeySet(keys.Select(k => new StringKey(k))));

        public static readonly Parser<IEnumerable<PathMatcher>> Route =
            from first in KeyMatcher
            from rest in DotKey.Or(PatternMatcher).Or(KeySet).Many()
            select Cons(first, rest);

        // key
        //  nothing
        //  dot key
        //  patterns
        //  keys

        // patterns
        //   nothing
        //   dot key
        //   keys

        // keys
        //  nothing

        public static readonly Parser<StringKeyMatcher> DotKey =
            from dot in Dot
            from key in KeyMatcher
            select key;

        public static readonly Parser<PathMatcher> PatternMatcher =
            from openingBracket in OpeningBracket
            from openingBrace in OpeningBrace
            from pattern in Letter.AtLeastOnce().Text()
            from seperator in Char(':').Optional()
            from name in Letter.AtLeastOnce().Text().Optional()
            from closingBrace in ClosingBrace
            from closingBracket in ClosingBracket
            select pattern == "ranges" ? new RangesPatternMatcher(name.GetOrElse(null))
                : pattern == "integers" ? (PathMatcher)new IntegersPatternMatcher(name.GetOrElse(null))
                : pattern == "keys" ? new KeysPatternMatcher(name.GetOrElse(null)) : null;

        static IEnumerable<T> Cons<T>(T head, IEnumerable<T> rest)
        {
            yield return head;
            foreach (var item in rest)
                yield return item;
        }
    }
}
