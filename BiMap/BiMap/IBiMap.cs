using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BiMap
{
	public interface IBiMap<TLeft, TRight>
	{
		IEnumerable<KeyValuePair<TLeft, TRight>> EnumerateLeftToRight();
		IEnumerable<KeyValuePair<TRight, TLeft>> EnumerateRightToLeft();

		TRight GetLeftToRight(TLeft key);
		bool TryGetLeftToRight(TLeft key, out TRight value);
		TLeft GetRightToLeft(TRight key);
		bool TryGetRightToLeft(TRight key, out TLeft value);

		int Count { get; }
	}
}
