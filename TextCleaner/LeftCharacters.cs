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
using static System.Int32;

namespace TextCleaner
{
    public class LeftCharacters : Operation
    {
        public OperationResult Process(string text, string[] args)
        {
            return TryParse(args[0], out var count)
                ? new OperationResult(text.Substring(0, Math.Min(text.Length, count)))
                : new OperationResult(text);
        }

        public string Name => "Get Left Characters";

        public string[] ArgNames => new[] {"Count"};
    }
}
