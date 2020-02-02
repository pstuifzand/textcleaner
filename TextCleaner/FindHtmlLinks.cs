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
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace TextCleaner
{
    public class FindHtmlLinks : Operation
    {
        public OperationResult Process(string text, string[] args)
        {
            var result = new OperationResult();

            var doc = new HtmlDocument();
            doc.LoadHtml(text);

            var tags = doc.DocumentNode.SelectNodes("//a[@href]");

            if (tags == null) return result;

            var sb = new StringBuilder();
            var re = new Regex("\\s+");

            var baseUri = args[1].Length > 0 ? new Uri(args[1]) : null;

            foreach (var tag in tags) {
                var atxt = re.Replace(tag.InnerText, " ").Trim();
                var tagHref = tag.GetAttributeValue("href", "");
                var href = tagHref;
                if (tagHref != "" && baseUri != null)
                    try {
                        href = new Uri(baseUri, tagHref).ToString();
                    } catch (UriFormatException) {
                        href = tagHref;
                    }

                var cls = re.Replace(tag.GetAttributeValue("class", ""), " ").Trim();

                if (args[0].Length > 0) {
                    sb.AppendFormat(args[0], atxt, href, cls);
                    sb.Append("\n");
                } else {
                    sb.Append(atxt);
                    sb.Append("|");
                    sb.AppendLine(href);
                }
            }

            result.Text = sb.ToString();
            result.Keep = true;
            return result;
        }

        public string Name => "Find HTML Links";

        public string[] ArgNames => new[] {"Format (optional)", "Base Uri"};
    }
}
