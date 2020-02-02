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

namespace TextCleaner
{
    public class Trim : Operation
    {
        public OperationResult Process(string text, string[] args)
        {
            if (args[0].Length == 0 && args[1].Length == 0) return new OperationResult(text.Trim());

            var result = text;

            if (args[0].Length > 0) result = result.TrimStart(args[0].ToCharArray());
            if (args[1].Length > 0) result = result.TrimEnd(args[1].ToCharArray());

            return new OperationResult(result);
        }

        public string Name => "Trim";

        public string[] ArgNames {
            get { return new[] {"Cutset Left", "Cutset Right"}; }
        }
    }
}
