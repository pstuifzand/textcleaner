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

using System.Text.RegularExpressions;

namespace TextCleaner
{
    public class ReplaceText : Operation
    {
        public string Name => "Replace Matching Text";
        public string[] ArgNames => new[] {"Regex", "Replacement"};

        public OperationResult Process(string text, string[] args)
        {
            var re = new Regex(args[0]);

            args[1] = args[1].Replace("\\r\\n", "\n");
            args[1] = args[1].Replace("\\n", "\n");

            var result = new OperationResult();
            var resultText = re.Replace(text, delegate(Match match) {
                var capture = match.Captures[0];

                result.InputHighlights.Add(new Highlight(capture.Index, capture.Index + capture.Length));

                return match.Result(args[1]);
            });

            result.Text = resultText;
            result.Keep = true;

            return result;
        }
    }
}
