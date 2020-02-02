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

namespace Abacus
{
    public class OperatorExpression : Expression
    {
        private string op;
        public Expression Left { get; }
        public Expression Right { get; }

        public OperatorExpression(string op, Expression a, Expression b)
        {
            this.op = op;
            Left = a;
            Right = b;
        }

        public override ReturnValue Value(Binding binding)
        {
            ReturnValue ra = Left.Value(binding);
            ReturnValue rb = Right.Value(binding);

            if (!ra.Defined() || !rb.Defined()) {
                return new ReturnValue();
            }

            if (op == "+") {
                return new ReturnValue(ra.Value() + rb.Value());
            }

            if (op == "-") {
                return new ReturnValue(ra.Value() - rb.Value());
            }

            if (op == "*") {
                return new ReturnValue(ra.Value() * rb.Value());
            }

            if (op == "/") {
                return new ReturnValue(ra.Value() / rb.Value());
            }

            if (op == "//") {
                return new ReturnValue((int) (ra.Value() / rb.Value()));
            }

            throw new ParserException("Unknown operator: " + op);
        }
    }
}
