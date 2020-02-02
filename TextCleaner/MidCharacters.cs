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

using System;

namespace TextCleaner
{
    public class MidCharacters : Operation
    {
        public OperationResult Process(string text, string[] args)
        {
            int count;
            int position;

            if (int.TryParse(args[0], out position) && int.TryParse(args[1], out count)) {
                position = Math.Min(position, text.Length);
                count = Math.Min(count, text.Length - position);
                var value = text.Substring(position, count);
                return new OperationResult(value);
            }

            return new OperationResult(text);
        }

        public string Name => "Get Middle Characters";

        public string[] ArgNames => new[] {"Position", "Count"};
    }
}
