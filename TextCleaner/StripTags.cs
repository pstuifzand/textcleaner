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

using System.Net;
using HtmlAgilityPack;

namespace TextCleaner
{
    public class StripTags : Operation
    {
        public string Name => "Strip tags";

        public string[] ArgNames => new[] {"Tag names (comma separated)"};

        public OperationResult Process(string text, string[] args)
        {
            var result = new OperationResult();

            var doc = new HtmlDocument();
            doc.LoadHtml(text);

            var tags = doc.DocumentNode.SelectNodes("//script");
            if (tags != null)
                foreach (var script in tags)
                    script.Remove();

            tags = doc.DocumentNode.SelectNodes("//style");
            if (tags != null)
                foreach (var style in tags)
                    style.Remove();

            result.Text = WebUtility.HtmlDecode(doc.DocumentNode.InnerText);
            result.Keep = true;
            return result;
        }
    }
}
