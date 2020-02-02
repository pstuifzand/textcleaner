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
using Abacus;

namespace TextCleaner
{
    public class Calculate : Operation
    {
        private readonly Binding binding = new Binding();

        public OperationResult Process(string text, string[] args)
        {
            var result = new OperationResult {Keep = true};

            var calc = new Calculator(binding);

            var sb = new StringBuilder();

            var start = 0;

            while (true) {
                var parseResult = Expression.parse(text, start);
                if (parseResult.Expr == null) {
                    // copy text from after last calculation
                    sb.Append(text.Substring(start));
                    break;
                }

                sb.Append(text.Substring(start, parseResult.Start - start)); // copy text before calculation
                sb.Append(calc.calculate(parseResult.Expr)); // copy result of calculation
                start = parseResult.End;
                result.InputHighlights.Add(new Highlight(parseResult.Start, parseResult.End));
            }

            result.Text = sb.ToString();

            return result;
        }

        public string Name => "Calculate";

        public string[] ArgNames {
            get { return new string[] { }; }
        }
    }
}
