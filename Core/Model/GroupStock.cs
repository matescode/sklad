using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkladCore.Model
{
	/// <summary>
	/// Type of stock status
	/// </summary>
	public enum StockStatusType
	{
		/// <summary>
		/// Current stock
		/// </summary>
		Current,
		/// <summary>
		/// Stock for selected year
		/// </summary>
		ForYear,
		/// <summary>
		/// Stock after selected year
		/// </summary>
		AfterYear
	}

	/// <summary>
	/// Class of group-stock
	/// </summary>
	public class GroupStock
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GroupStock"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="price">The price.</param>
		/// <param name="count">The count.</param>
		public GroupStock(string name, double price, int count)
		{
			this.Name = name;
			this.Price = price;
			this.Count = count;
		}

		#region PROPERTIES

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the price.
		/// </summary>
		/// <value>The price.</value>
		public double Price
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the count.
		/// </summary>
		/// <value>The count.</value>
		public int Count
		{
			get;
			private set;
		}

		#endregion PROPERTIES
	}
}
