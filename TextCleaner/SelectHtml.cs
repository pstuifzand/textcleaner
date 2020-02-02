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
using AngleSharp;
using AngleSharp.Html.Parser;
using AngleSharp.Text;

namespace TextCleaner
{
    public class SelectHtml : Operation
    {
        public OperationResult Process(string text, string[] args)
        {
            var result = new OperationResult();
            if (args[0].Length == 0) return result;

            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            var parser = context.GetService<IHtmlParser>();
            var document = parser.ParseDocument(text);

            var tags = document.QuerySelectorAll(args[0]);
            if (tags == null) {
                result.Text = text;
                result.Keep = true;
                return result;
            }

            var cmds = args[1].SplitWithTrimming('|');

            var sb = new StringBuilder();
            foreach (var tag in tags)
                if (cmds.Length > 0) {
                    foreach (var cmd in cmds) {
                        if (cmd == "outer")
                            sb.AppendLine(tag.OuterHtml);
                        else if (cmd == "inner")
                            sb.AppendLine(tag.InnerHtml);
                        else if (cmd == "text")
                            sb.AppendLine(tag.TextContent);
                        else if (cmd.StartsWith("attr:"))
                            sb.AppendLine(tag.GetAttribute(cmd.SplitWithTrimming(':')[1]));
                    }
                } else {
                    sb.AppendLine(tag.OuterHtml);
                }

            result.Text = sb.ToString();
            result.Keep = true;
            return result;
        }

        public string Name => "Select HTML";

        public string[] ArgNames => new[] {"CSS Selectors", "Result"};
    }
}
