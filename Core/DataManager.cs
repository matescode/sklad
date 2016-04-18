using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using SkladCore.Model;
using SkladCore.Utilities;
using SkladCore.Interfaces;

namespace SkladCore
{
	/// <summary>
	/// Data manager
	/// </summary>
	public class DataManager : ISaveable
	{
		#region CONSTANTS

		/// <summary>
		/// Path to data folder
		/// </summary>
		public const string DATA_PATH = "Data";

		/// <summary>
		/// Path to data file
		/// </summary>
		public const string DATA_FILE = "data.xml";

		#endregion CONSTANTS

		#region ATTRIBUTES

		private static DataManager instance = null;

		private List<Group> groups;

		private GroupComparer comparer;

		private List<Customer> customers;

		private CustomerComparer customerComparer;

		#endregion ATTRIBUTES

		/// <summary>
		/// Initializes a new instance of the <see cref="DataManager"/> class.
		/// </summary>
		private DataManager()
		{
			this.groups = new List<Group>();
			this.comparer = new GroupComparer();
			this.customers = new List<Customer>();
			this.customerComparer = new CustomerComparer();
		}

		#region PROPERTIES

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <value>The instance.</value>
		public static DataManager Instance
		{
			get
			{
				if (DataManager.instance == null)
				{
					DataManager.instance = new DataManager();
				}
				return DataManager.instance;
			}
		}

		/// <summary>
		/// Gets the data file.
		/// </summary>
		/// <value>The data file.</value>
		private string DataFile
		{
			get
			{
				return Path.Combine(DATA_PATH, DATA_FILE);
			}
		}

		/// <summary>
		/// Gets the groups.
		/// </summary>
		/// <value>The groups.</value>
		public List<Group> Groups
		{
			get
			{
				List<Group> result = this.groups.ToList();
				result.Sort(this.comparer);
				return result;
			}
		}

		/// <summary>
		/// Gets the customers.
		/// </summary>
		/// <value>The customers.</value>
		public List<Customer> Customers
		{
			get
			{
				List<Customer> result = this.customers.ToList();
				result.Sort(this.customerComparer);
				return result;
			}
		}

		#endregion PROPERTIES

		/// <summary>
		/// Adds the group.
		/// </summary>
		/// <param name="group">The group.</param>
		public void AddGroup(Group group)
		{
			if (!this.groups.Contains(group))
			{
				this.groups.Add(group);
			}
		}

		/// <summary>
		/// Deletes the group.
		/// </summary>
		/// <param name="group">The group.</param>
		public void DeleteGroup(Group group)
		{
			if (this.groups.Contains(group))
			{
				this.groups.Remove(group);
			}
		}

		/// <summary>
		/// Adds the customer.
		/// </summary>
		/// <param name="customer">The customer.</param>
		public void AddCustomer(Customer customer)
		{
			if (!this.customers.Contains(customer))
			{
				this.customers.Add(customer);
			}
		}

		/// <summary>
		/// Deletes the customer.
		/// </summary>
		/// <param name="customer">The customer.</param>
		public void DeleteCustomer(Customer customer)
		{
			if (this.customers.Contains(customer))
			{
				this.customers.Remove(customer);
			}
		}

		/// <summary>
		/// Saves this instance.
		/// </summary>
		public void Save()
		{
			XmlDocument document = new XmlDocument();
			XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", null, null);
			document.AppendChild(declaration);
			document.AppendChild(this.Save(document));
			document.Save(Path.Combine("Data", "data.xml"));
		}

		/// <summary>
		/// Loads this instance.
		/// </summary>
		public void Load()
		{
			XmlDocument document = new XmlDocument();
			document.Load(this.DataFile);
			this.Load(document["storage"]);
		}

		/// <summary>
		/// Gets the group stock.
		/// </summary>
		/// <param name="stockType">Type of the stock.</param>
		/// <param name="year">The year.</param>
		/// <returns></returns>
		public List<GroupStock> GetGroupStock(StockStatusType stockType, int year)
		{
			List<GroupStock> stock = new List<GroupStock>();
			foreach (var group in this.Groups)
			{
				stock.Add(new GroupStock(group.Name, group.GetPriceStock(stockType, year), group.GetCountStock(stockType, year)));
			}
			return stock;
		}

		/// <summary>
		/// Gets the summary stock.
		/// </summary>
		/// <param name="stockType">Type of the stock.</param>
		/// <param name="year">The year.</param>
		/// <returns></returns>
		public GroupStock GetSummaryStock(StockStatusType stockType, int year)
		{
			int count = 0;
			double price = 0;
			foreach (var group in this.Groups)
			{
				count += group.GetCountStock(stockType, year);
				price += group.GetPriceStock(stockType, year);
			}

			return new GroupStock("Celkem:", price, count);
		}

		#region ISaveable Members

		/// <summary>
		/// Saves instance into the specified document.
		/// </summary>
		/// <param name="document">The document.</param>
		/// <returns>XMLElement of object to be saved</returns>
		public XmlElement Save(XmlDocument document)
		{
			XmlElement rootElem = document.CreateElement("storage");
			
			XmlElement groupsElem = document.CreateElement("groups");
			rootElem.AppendChild(groupsElem);
			foreach (var group in this.groups)
			{
				groupsElem.AppendChild(group.Save(document));
			}

			XmlElement customersElem = document.CreateElement("customers");
			rootElem.AppendChild(customersElem);
			foreach (var customer in this.customers)
			{
				customersElem.AppendChild(customer.Save(document));
			}

			return rootElem;
		}

		/// <summary>
		/// Loads instance from the specified element.
		/// </summary>
		/// <param name="data">The data element.</param>
		public void Load(XmlElement data)
		{
			XmlNodeList groupList = data["groups"].GetElementsByTagName("group");
			for (int i = 0; i < groupList.Count; ++i)
			{
				this.groups.Add(new Group(groupList[i] as XmlElement));
			}

			XmlNodeList customerList = data["customers"].GetElementsByTagName("customer");
			for (int i = 0; i < customerList.Count; ++i)
			{
				this.customers.Add(new Customer(customerList[i] as XmlElement));
			}
		}

		#endregion
	}
}
