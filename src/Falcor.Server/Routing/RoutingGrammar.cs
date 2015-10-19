using System.Collections.Generic;
using System.Linq;
using Sprache;

namespace Falcor.Server.Routing
{
    public static class RoutingGrammar
    {
        public static readonly Parser<char> Comma = Parse.Char(',');
        public static readonly Parser<char> Dot = Parse.Char('.');
        public static readonly Parser<char> OpeningBracket = Parse.Char('[');
        public static readonly Parser<char> ClosingBracket = Parse.Char(']');
        public static readonly Parser<char> OpeningBrace = Parse.Char('{');
        public static readonly Parser<char> ClosingBrace = Parse.Char('}');
        public static readonly Parser<char> SingleQuote = Parse.Char('\'');
        public static readonly Parser<char> DoubleQuote = Parse.Char('"');

        public static readonly Parser<string> SingleQuotedString =
            from openQuote in SingleQuote
            from content in Parse.LetterOrDigit.Except(SingleQuote).Many().Text()
            from closeQuote in SingleQuote
            select content;

        public static readonly Parser<string> DoubleQuotedString =
            from openQuote in DoubleQuote
            from content in Parse.LetterOrDigit.Except(DoubleQuote).Many().Text()
            from closeQuote in DoubleQuote
            select content;

        public static readonly Parser<PathMatcher> StringKeyMatcher =
            from key in Parse.Letter.AtLeastOnce().Text()
            select PathMatchers.StringKey(key);

        public static readonly Parser<IEnumerable<string>> CommaSeperatedSingleQuotedStrings =
            from first in SingleQuotedString.Token()
            from rest in Comma.Then(_ => SingleQuotedString.Token()).Many()
            select Cons(first, rest);

        public static readonly Parser<PathMatcher> KeySet =
            from openingBracket in OpeningBracket
            from keys in CommaSeperatedSingleQuotedStrings
            from closingBracket in ClosingBracket
            select PathMatchers.StringKeys(keys.ToList());

        public static readonly Parser<IEnumerable<PathMatcher>> Route =
            from first in StringKeyMatcher
            from rest in DotKey.Or(PatternMatcher).Or(KeySet).Many()
            select Cons(first, rest);

        public static readonly Parser<PathMatcher> DotKey =
            from dot in Dot
            from key in StringKeyMatcher
            select key;

        public static readonly Parser<PathMatcher> PatternMatcher =
            from openingBracket in OpeningBracket
            from openingBrace in OpeningBrace
            from pattern in Parse.Letter.AtLeastOnce().Text()
            from seperator in Parse.Char(':').Optional()
            from maybeName in Parse.Letter.AtLeastOnce().Text().Optional()
            from closingBrace in ClosingBrace
            from closingBracket in ClosingBracket
            let name = maybeName.GetOrElse(null)
            select pattern == "ranges" ? PathMatchers.RangesPattern(name)
                : pattern == "integers" ? PathMatchers.IntegersPattern(name)
                : pattern == "keys" ? PathMatchers.KeysPattern(name) : null;

        public static object Brackets { get; set; }

        static IEnumerable<T> Cons<T>(T head, IEnumerable<T> rest)
        {
            yield return head;
            foreach (var item in rest)
                yield return item;
        }
    }
}
