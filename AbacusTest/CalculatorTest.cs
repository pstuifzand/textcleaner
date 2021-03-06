/*  Abacus - a calculator that calculates as you type
    Copyright (C) 2012  Peter Stuifzand

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using Abacus;
using NUnit.Framework;

namespace AbacusTest
{
    [TestFixture()]
    public class CalculatorTest
    {
        [Test()]
        public void TestLiterals()
        {
            string[] tests = {
                "0", "0",
                "1", "1",
                "2", "2",
                "10", "10",
                "12", "12",
                "909", "909",
                "-10", "-10",
                "1.0", "1",
                "2.0", "2",
                "-2.0", "-2",
                "-99.99", "-99.99",
                ".5", "0.5",
            };


            for (int i = 0; i < tests.Length; i += 2) {
                ParseResult expr = Expression.parse(tests[i]);
                Assert.AreEqual(tests[i + 1], expr.Expr.ToString());
            }
        }

        [Test()]
        public void TestAdd()
        {
            string[] tests = {
                "0+0", "0",
                "1+0", "1",
                "1+1", "2",
                "1+2", "3",
                "-1+2", "1",
                "9+1", "10",
                "1+9", "10",
                "1+10", "11",
                "10+1", "11",
                "10 + 1", "11",
                "10 + 11", "21",
                "    80 + 11", "91",
                "1.0 + 2.0", "3",
                "1+2+3+4+5", "15",
                "1+2+3+4 +5", "15",
            };

            TestCalculations(tests);
        }

        [Test()]
        public void TestMul()
        {
            string[] tests = {
                "1*0", "0",
                "1*1", "1",
                "1*2", "2",
                "-1*2", "-2",
                "9*1", "9",
                "1*9", "9",
                "1*10", "10",
                "10*1", "10",
                "10 * 1", "10",
                "10 * 11", "110",
                "    80 * 11", "880",
                "1.0 * 2.0", "2",
                "1.1 * 1.1", "1.2100000000000002",
                "1*2*3*4 *5", "120",
            };
            TestCalculations(tests);
        }

        [Test()]
        public void TestSubtract()
        {
            string[] tests = {
                "0-0", "0",
                "1-1", "0",
                "2-1", "1",
                "2-3", "-1",
                "10000-9999", "1",
                "1000000000000000-999999999999999", "1",
            };
            TestCalculations(tests);
        }

        [Test()]
        public void TestDivison()
        {
            string[] tests = {
                "200/119*19", "31.932773109243698",
                "2/2*10", "10",
            };
            TestCalculations(tests);
        }

        [Test()]
        public void TestIntDivison()
        {
            string[] tests = {
                "1//1", "1",
                "2//2", "1",
                "20//2", "10",
                "18//3", "6",
                "19//3", "6",
                "20//3", "6",
            };
            TestCalculations(tests);
        }

        [Test()]
        public void TestParentheses()
        {
            string[] tests = {
                "200/(2*50)", "2",
                "4*(2+5)", "28",
                "( 1 +1)", "2",
            };
            TestCalculations(tests);
        }

        [Test()]
        public void TestVariables()
        {
            string[] tests = {
                // "x=1",   "1",
                // "1+x",   "2",
                // "benzine=60", "60",
                // "ritten=8", "8",
                // "kosten=benzine/ritten", "7.5",
                //"x=1", "x = 1",
            };
            TestCalculations(tests);
        }

        private void TestCalculations(string[] tests)
        {
            Binding binding = new Binding();
            Calculator calc = new Calculator(binding);

            for (int i = 0; i < tests.Length; i += 2) {
                ParseResult expr = Expression.parse(tests[i]);
                Assert.AreEqual(tests[i + 1], calc.calculate(expr.Expr).ToString(), tests[i]);
            }
        }
    }
}
