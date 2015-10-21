using System.Collections.Generic;
using System.Linq;
using Sprache;

namespace Falcor.Server.Routing.PathParser
{
    internal static class PathGrammar
    {
        public static readonly Parser<char> Colon = Parse.Char(':');
        public static readonly Parser<char> Comma = Parse.Char(',');
        public static readonly Parser<char> Dot = Parse.Char('.');
        public static readonly Parser<char> OpeningBracket = Parse.Char('[');
        public static readonly Parser<char> ClosingBracket = Parse.Char(']');
        public static readonly Parser<char> OpeningBrace = Parse.Char('{');
        public static readonly Parser<char> ClosingBrace = Parse.Char('}');
        public static readonly Parser<char> SingleQuote = Parse.Char('\'');
        public static readonly Parser<char> DoubleQuote = Parse.Char('"');

        public static readonly Parser<bool> Boolean =
            from booleanString in Text("true").Or(Text("false"))
            select bool.Parse(booleanString);

        public static readonly Parser<int> Number = Parse.Digit.Many().Select(chs => int.Parse(new string(chs.ToArray())));
        public static readonly Parser<string> SingleQuotedString = Parse.LetterOrDigit.Many().Text().SingleQuoted();
        public static readonly Parser<string> DoubleQuotedString = Parse.LetterOrDigit.Many().Text().DoubleQuoted();
        public static readonly Parser<string> QuotedString = SingleQuotedString.Or(DoubleQuotedString);

        // Helpers
        public static Parser<string> Text(string s) => Parse.String(s).Text();

        public static Parser<T> Bracketed<T>(this Parser<T> parser) =>
            from openBracket in OpeningBracket
            from item in parser
            from closeBracket in ClosingBracket
            select item;

        public static Parser<T> Braced<T>(this Parser<T> parser) =>
            from openBrace in OpeningBrace
            from item in parser
            from closeBrace in ClosingBrace
            select item;

        public static Parser<T> DoubleQuoted<T>(this Parser<T> parser) =>
            from openQuote in DoubleQuote
            from item in parser.Except(DoubleQuote)
            from closeQuote in DoubleQuote
            select item;

        public static Parser<T> SingleQuoted<T>(this Parser<T> parser) =>
            from openQuote in SingleQuote
            from item in parser.Except(SingleQuote)
            from closeQuote in SingleQuote
            select item;


        static IEnumerable<T> Cons<T>(T head, IEnumerable<T> rest)
        {
            yield return head;
            foreach (var item in rest)
                yield return item;
        }

        // General Parsers
        public static readonly Parser<IEnumerable<string>> CommaSeperatedQuotedStrings =
            from first in QuotedString.Token()
            from rest in Comma.Token().Then(_ => QuotedString.Token()).Many()
            select Cons(first, rest);

        // Path Parsers
        public static readonly Parser<NumberRange> NumberRange =
            from openingBrace in OpeningBrace
            from fromKey in Parse.String("from").Text().DoubleQuoted().Token()
            from fromColon in Colon.Token()
            from fromValue in Number.Token()
            from comma in Comma.Token()
            from toKey in Parse.String("to").Text().DoubleQuoted().Token()
            from toColon in Colon.Token()
            from toValue in Number.Token()
            from closingBrace in ClosingBrace
            select new NumberRange(fromValue, toValue);

        public static readonly Parser<SimpleKey> NullKey =
            from value in Text("null")
            select Falcor.NullKey.Instance;

        public static readonly Parser<SimpleKey> StringKey =
            from value in Parse.Letter.AtLeastOnce().Text().DoubleQuoted()
            select new StringKey(value);

        public static readonly Parser<NumberKey> NumberKey =
            from value in Number
            select new NumberKey(value);

        public static readonly Parser<SimpleKey> BooleanKey =
            from value in Boolean
            select new BooleanKey(value);

        public static readonly Parser<SimpleKey> Key = StringKey.Or(BooleanKey).Or(NullKey);

        public static readonly Parser<KeySegment> KeySet =
            from openingBracket in OpeningBracket
            from first in Key
            from rest in Comma.Token().Then(_ => Key).Many()
            from closingBracket in ClosingBracket
            select new KeySet(Cons(first, rest));

        public static readonly Parser<KeySegment> NumericSet =
            from openingBracket in OpeningBracket
            from first in Number.Token()
            from rest in Comma.Token().Then(_ => Number.Token()).Many()
            from closingBracket in ClosingBracket
            select new NumericSet(Cons(first, rest));

        public static readonly Parser<FalcorPath> Path =
            from openingBracket in OpeningBracket
            from first in StringKey.Token()
            from rest in Comma.Token().Then(_ => Key.Or(KeySet).Or(NumberRange).Or(NumericSet)).Many()
            from closingBracket in ClosingBracket
            select new FalcorPath(Cons(first, rest));

        public static readonly Parser<IEnumerable<FalcorPath>> Paths =
            from openingBracket in OpeningBracket
            from first in Path
            from rest in Comma.Token().Then(_ => Path).Many()
            from closingBracket in ClosingBracket
            select Cons(first, rest);

        // RouteMatcher Parsers
        public static readonly Parser<PathMatcher> StringKeyMatcher =
            from key in Parse.Letter.AtLeastOnce().Text()
            select PathMatchers.StringKey(key);

        public static readonly Parser<PathMatcher> KeySetMatcher =
            from keys in CommaSeperatedQuotedStrings.Bracketed()
            select PathMatchers.KeySet(keys.ToList());

        public static readonly Parser<IEnumerable<PathMatcher>> RouteMatcher =
            from first in StringKeyMatcher
            from rest in DotKeyMatcher.Or(PatternMatcher).Or(KeySetMatcher).Many()
            select Cons(first, rest);

        public static readonly Parser<PathMatcher> DotKeyMatcher =
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
    }
}
