using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SkladCore.Model;

namespace SkladCore.Utilities
{
	/// <summary>
	/// Customer comparer
	/// </summary>
	public class CustomerComparer : IComparer<Customer>
	{
		#region IComparer<Customer> Members

		/// <summary>
		/// Compares the specified x with specified y.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns></returns>
		public int Compare(Customer x, Customer y)
		{
			return x.Name.CompareTo(y.Name);
		}

		#endregion
	}
}
