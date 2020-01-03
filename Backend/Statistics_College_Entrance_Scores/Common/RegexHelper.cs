using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Statistics_College_Entrance_Scores.Common
{
	public class RegexHelper
	{
        public static bool getRegexGroupCodeIsOnlyChar(string groupCode)
        {
            return execGroupCodeRegex(@"\b\w\b", groupCode);
        }

        public static bool getRegexGroupCodeIsCharDigit(string groupCode)
        {
            return execGroupCodeRegex(@"\b[A-Z]\d\b", groupCode);
        }

        private static bool execGroupCodeRegex(string regex,string groupCode)
        {
            Regex rx = new Regex(regex, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection matches = rx.Matches(groupCode);
            if (matches.Count > 0)
            {
                return true;
            }
            return false;
        }

		public static string getRegexLikeGroupCode(string groupCode)
		{
            string sqlLike = "";

            if (getRegexGroupCodeIsCharDigit(groupCode))
            {
                var charGroupCode = groupCode.Substring(0, 1);
                var numGroupCode = groupCode.Substring(1, 1);
                sqlLike = "\"groupCode\" like '%" + charGroupCode + "0" + numGroupCode + "%' ";
            }
            if (getRegexGroupCodeIsOnlyChar(groupCode))
            {
                var charGroupCode = groupCode.Substring(0, 1);
                sqlLike = "(\"groupCode\" like '%" + charGroupCode + "00%' " +
                   "OR \"groupCode\" like '%" + charGroupCode + ",%' " +
                   "OR \"groupCode\" like '%" + charGroupCode + "')";
            }
            else
            {
                sqlLike = "\"groupCode\" like '%" + groupCode + "%'";
            }
            return sqlLike;
        }
	}
}
