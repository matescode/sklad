using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SkladCore.Model;

namespace SkladCore.Utilities
{
	/// <summary>
	/// Group comparer
	/// </summary>
	public class GroupComparer : IComparer<Group>
	{
		#region IComparer<Typ> Members

		/// <summary>
		/// Compares the specified x with specified y.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns></returns>
		public int Compare(Group x, Group y)
		{
			return x.Name.CompareTo(y.Name);
		}

		#endregion
	}
}
