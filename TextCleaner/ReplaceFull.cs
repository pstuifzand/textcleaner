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
    public class ReplaceFull : Operation
    {
        public string Name => "Replace Full Text";
        public string[] ArgNames => new[] {"Regex", "Replacement"};

        public OperationResult Process(string text, string[] args)
        {
            var re = new Regex(args[0]);

            args[1] = args[1].Replace("\\r\\n", "\n");
            args[1] = args[1].Replace("\\n", "\n");

            var result = new OperationResult();

            var sb = new StringBuilder();

            text = text.TrimEnd();

            var m = re.Match(text);
            var count = 0;
            while (m.Success) {
                var capture = m.Captures[0];
                result.InputHighlights.Add(new Highlight(capture.Index, capture.Index + capture.Length));
                var resultText = m.Result(args[1]);
                sb.AppendLine(resultText);
                m = m.NextMatch();
                count++;
            }

            result.Text = sb.ToString();
            result.Keep = count > 0;
            return result;
        }
    }
}
