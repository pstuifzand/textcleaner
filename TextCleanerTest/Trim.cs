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

using NUnit.Framework;
using TextCleaner;

namespace TextCleanerTest
{
    public class TrimTests
    {
        private Operation operation;

        [SetUp]
        public void Setup()
        {
            operation = new Trim();
        }

        [Test]
        public void TestTrimLeft()
        {
            var result = operation.Process("   test", new[] {"", ""});
            Assert.IsTrue(result.Keep);
            Assert.AreEqual("test", result.Text);
        }

        [Test]
        public void TestTrimRight()
        {
            var result = operation.Process("test    ", new[] {"", ""});
            Assert.IsTrue(result.Keep);
            Assert.AreEqual("test", result.Text);
        }

        [Test]
        public void TestTrimBoth()
        {
            var result = operation.Process("      test    ", new[] {"", ""});
            Assert.IsTrue(result.Keep);
            Assert.AreEqual("test", result.Text);
        }

        [Test]
        public void TestTrimCutsetLeft()
        {
            var result = operation.Process("****test    ", new[] {"*", ""});
            Assert.IsTrue(result.Keep);
            Assert.AreEqual("test    ", result.Text);
        }

        [Test]
        public void TestTrimCutsetRight()
        {
            var result = operation.Process("    test****", new[] {"*", "*"});
            Assert.IsTrue(result.Keep);
            Assert.AreEqual("    test", result.Text);
        }
    }
}
