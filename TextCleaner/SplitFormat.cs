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
using System.Text;

namespace TextCleaner
{
    public class SplitFormat : Operation
    {
        public string Name => "Split and Format";

        public string[] ArgNames => new[] {"Separator", "Format"};

        public OperationResult Process(string text, string[] args)
        {
            var parts = text.Split(args[0]);
            var sb = new StringBuilder();
            try {
                sb.AppendFormat(args[1], parts);
            } catch (Exception) {
                return new OperationResult();
            }

            return new OperationResult(sb.ToString());
        }
    }
}
