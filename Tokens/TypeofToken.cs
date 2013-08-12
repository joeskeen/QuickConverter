﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.CSharp.RuntimeBinder;

namespace QuickConverter.Tokens
{
	public class TypeofToken : TokenBase
	{
		internal TypeofToken()
		{
		}

		private Type type;
		internal override bool TryGetToken(ref string text, out TokenBase token)
		{
			token = null;
			if (!text.StartsWith("typeof"))
				return false;
			string temp = text.Substring(6).TrimStart();
			if (temp.Length < 3 || temp[0] != '(')
				return false;
			var name = GetNameMatches(temp.Substring(1), null, null).FirstOrDefault(tuple => tuple.Item1 is Type && tuple.Item2.TrimStart().StartsWith(")"));
			if (name == null)
				return false;
			text = name.Item2.TrimStart().Substring(1);
			token = new TypeofToken() { type = name.Item1 as Type };
			return true;
		}

		public override Expression GetExpression(List<ParameterExpression> parameters, Type dynamicContext = null)
		{
			return Expression.Constant(type, typeof(object));
		}
	}
}