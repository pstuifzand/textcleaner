﻿/*
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
    public class Highlight
    {
        public readonly int Line;
        public int Start;
        public int End;

        public Highlight()
        {
            Line = -1;
        }

        public Highlight(int s, int e)
        {
            Line = -1;
            Start = s;
            End = e;
        }

        public Highlight(int line, int s, int e)
        {
            Line = line;
            Start = s;
            End = e;
        }
    }
}
