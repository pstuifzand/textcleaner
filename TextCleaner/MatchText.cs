/*
 * Text Cleaner - A utility to cleanup text
 * Copyright (C) 2020 Peter Stuifzand <peter@p83.nl>
 *
 * This file is part of Text Cleaner.
 *
 * Text Cleaner is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Text Cleaner is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Text Cleaner.  If not, see <http://www.gnu.org/licenses/>.
 */

using System.Text;
using System.Text.RegularExpressions;

namespace TextCleaner
{
    public class MatchText : Operation
    {
        public string Name => "Find Matches";
        public string[] ArgNames => new[] {"Regex", "Regex options [msi]"};

        public OperationResult Process(string text, string[] args)
        {
            RegexOptions opt = 0;

            foreach (var c in args[1].ToCharArray(0, args[1].Length)) {
                switch (c) {
                    case 's':
                        opt |= RegexOptions.Singleline;
                        break;
                    case 'm':
                        opt |= RegexOptions.Multiline;
                        break;
                    case 'i':
                        opt |= RegexOptions.IgnoreCase;
                        break;
                }
            }

            var re = new Regex(args[0], opt);
            if (args[0].Length == 0) return new OperationResult(text);

            var coll = re.Matches(text);

            if (coll.Count == 0) return new OperationResult();

            var result = new OperationResult();

            var sb = new StringBuilder();

            foreach (Match m in coll) {
                sb.AppendLine(m.Value);
                var hl = new Highlight();
                var capture = m.Captures[0];
                hl.Start = capture.Index;
                hl.End = capture.Index + capture.Length;
                result.InputHighlights.Add(hl);
            }

            result.Text = sb.ToString();
            result.Keep = true;
            return result;
        }
    }
}
