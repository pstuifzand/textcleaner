using System;

namespace Abacus
{
	public class ParseResult
	{
		public Expression Expr {
			get;
			set;
		}

		public int Start {
			get;
			set;
		}

		public int End {
			get;
			set;
		}

		public ParseResult (Expression expr, int start, int end)
		{
			this.Expr = expr;
			this.Start = start;
			this.End = end;
		}
	}
}
