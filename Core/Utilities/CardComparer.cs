using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SkladCore.Model;

namespace SkladCore.Utilities
{
	/// <summary>
	/// Cards comparer
	/// </summary>
	public class CardComparer : IComparer<Card>
	{
		#region IComparer<Karta> Members

		/// <summary>
		/// Compares the specified x with specified y.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns></returns>
		public int Compare(Card x, Card y)
		{
			return x.Name.CompareTo(y.Name);
		}

		#endregion
	}
}
