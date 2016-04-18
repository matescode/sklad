using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SkladCore.Model;

namespace SkladCore.Utilities
{
	/// <summary>
	/// Record comparer
	/// </summary>
    public class RecordComparer : IComparer<Record>
    {
		/// <summary>
		/// Compares the specified x with specified y.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns></returns>
        public int Compare(Record x, Record y)
        {
            return x.Date.CompareTo(y.Date);
        }
    }
}
