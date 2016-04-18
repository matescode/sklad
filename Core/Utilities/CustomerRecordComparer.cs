using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SkladCore.Model;

namespace SkladCore.Utilities
{
	/// <summary>
	/// Customer-record comparer
	/// </summary>
	public class CustomerRecordComparer : IComparer<CustomerRecord>
	{
		#region IComparer<CustomerRecord> Members

		/// <summary>
		/// Compares the specified x with specified y.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns></returns>
		public int Compare(CustomerRecord x, CustomerRecord y)
		{
			return x.Date.CompareTo(y.Date);
		}

		#endregion
	}
}
