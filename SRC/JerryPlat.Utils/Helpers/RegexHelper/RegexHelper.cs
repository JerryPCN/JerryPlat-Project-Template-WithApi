using System;
using System.Text.RegularExpressions;

namespace JerryPlat.Utils.Helpers
{
    public static class RegexHelper
    {
        public static Regex RegexReplace = new Regex(@"{{\w+}}", RegexOptions.Compiled);
        public static Regex RegexRouteParam = new Regex(@"{[^}]+}", RegexOptions.Compiled);

        public static MatchCollection Matches(string strTemplate, string strRegex)
        {
            return Regex.Matches(strTemplate, strRegex);
        }

        public static void Matches(string strTemplate, string strRegex, Action<Match> callback)
        {
            MatchCollection matches = Matches(strTemplate, strRegex);
            foreach (Match match in matches)
            {
                callback(match);
            }
        }

        public static string[] SplitCamelCase(string input)
        {
            return Regex.Replace(input, "([A-Z])", " $1", RegexOptions.Compiled).Trim().Split(' ');
        }
    }
}