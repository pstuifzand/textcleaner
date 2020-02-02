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

using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TextCleaner
{
    public class SelectJson : Operation
    {
        public string Name => "Select JSON";
        public string[] ArgNames => new[] {"JSONPath"};

        public OperationResult Process(string text, string[] args)
        {
            var reader = new JsonTextReader(new StringReader(text)) {SupportMultipleContent = true};
            var sb = new StringBuilder();

            while (true) {
                if (!reader.Read()) break;
                var token = JToken.Load(reader);
                if (args[0].Length > 0) {
                    foreach (var parsed in token.SelectTokens(args[0]))
                        if (parsed != null)
                            sb.AppendLine(parsed.ToString());
                } else {
                    sb.AppendLine(token.ToString());
                }
            }

            return new OperationResult(sb.ToString());
        }
    }
}
