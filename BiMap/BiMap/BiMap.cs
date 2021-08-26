using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BiMap
{
	public class BiMap<TLeft, TRight> : IBiMap<TLeft, TRight>
	{
		private Dictionary<TLeft, TRight> LeftToRight;
		private Dictionary<TRight, TLeft> RightToLeft;

		public int Count
		{
			get
			{
				return this.LeftToRight.Count;
			}
		}

		public BiMap(IEqualityComparer<TLeft> leftComp = null, IEqualityComparer<TRight> rightComp = null)
		{
			this.LeftToRight = new Dictionary<TLeft, TRight>(leftComp);
			this.RightToLeft = new Dictionary<TRight, TLeft>(rightComp);
		}

		public bool AddPair(TLeft left, TRight right)
		{
			if (LeftToRight.ContainsKey(left) || RightToLeft.ContainsKey(right))
			{
				return false;
			}

			LeftToRight.Add(left, right);
			RightToLeft.Add(right, left);

			return true;
		}

		public IEnumerable<KeyValuePair<TLeft, TRight>> EnumerateLeftToRight()
		{
			return LeftToRight;
		}

		public IEnumerable<KeyValuePair<TRight, TLeft>> EnumerateRightToLeft()
		{
			return RightToLeft;
		}

		public TRight GetLeftToRight(TLeft key)
		{
			TRight result;

			if (!this.TryGetLeftToRight(key, out result))
				throw new KeyNotFoundException("Key: " + key.ToString() + " not found in left-to-right collection");

			return result;
		}

		public bool TryGetLeftToRight(TLeft key, out TRight value)
		{
			return this.LeftToRight.TryGetValue(key, out value);
		}

		public TLeft GetRightToLeft(TRight key)
		{
			TLeft result;

			if (!this.TryGetRightToLeft(key, out result))
				throw new KeyNotFoundException("Key: " + key.ToString() + " not found in right-to-left collection");

			return result;
		}

		public bool TryGetRightToLeft(TRight key, out TLeft value)
		{
			return this.RightToLeft.TryGetValue(key, out value);
		}
	}
}
