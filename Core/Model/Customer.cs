using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using SkladCore.Interfaces;
using SkladCore.Utilities;

namespace SkladCore.Model
{
	/// <summary>
	/// Customer class
	/// </summary>
	public class Customer : ISaveable
	{
		#region ATTRIBUTES

		private List<CustomerRecord> customerRecords;
		private CustomerRecordComparer comparer;

		#endregion ATTRIBUTES

		/// <summary>
		/// Initializes a new instance of the <see cref="Customer"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="phone">The phone.</param>
		/// <param name="address">The address.</param>
		/// <param name="furnace">The furnace.</param>
		/// <param name="date">The date.</param>
		public Customer(string name, string phone, string address, string furnace, string date)
		{
			this.Init();
			this.Name = name;
			this.Phone = phone;
			this.Address = address;
			this.Furnace = furnace;
			this.Date = date;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Customer"/> class.
		/// </summary>
		/// <param name="data">The data.</param>
		public Customer(XmlElement data)
		{
			this.Init();
			this.Load(data);
		}

		private void Init()
		{
			this.customerRecords = new List<CustomerRecord>();
			this.comparer = new CustomerRecordComparer();
		}

		#region PROPERTIES

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the phone.
		/// </summary>
		/// <value>The phone.</value>
		public string Phone
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the address.
		/// </summary>
		/// <value>The address.</value>
		public string Address
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the furnace.
		/// </summary>
		/// <value>The furnace.</value>
		public string Furnace
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the date.
		/// </summary>
		/// <value>The date.</value>
		public string Date
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the customer records.
		/// </summary>
		/// <value>The customer records.</value>
		public List<CustomerRecord> CustomerRecords
		{
			get
			{
				List<CustomerRecord> result = this.customerRecords.ToList();
				result.Sort(this.comparer);
				return result;
			}
		}

		#endregion PROPERTIES

		/// <summary>
		/// Adds the customer record.
		/// </summary>
		/// <param name="record">The record.</param>
		public void AddCustomerRecord(CustomerRecord record)
		{
			if (!this.customerRecords.Contains(record))
			{
				this.customerRecords.Add(record);
			}
		}

		/// <summary>
		/// Deletes the customer record.
		/// </summary>
		/// <param name="record">The record.</param>
		public void DeleteCustomerRecord(CustomerRecord record)
		{
			if (this.customerRecords.Contains(record))
			{
				this.customerRecords.Remove(record);
			}
		}

		#region ISaveable Members

		/// <summary>
		/// Saves instance into the specified document.
		/// </summary>
		/// <param name="document">The document.</param>
		/// <returns>XMLElement of object to be saved</returns>
		public XmlElement Save(XmlDocument document)
		{
			XmlElement customerElem = document.CreateElement("customer");

			XmlElement nameElem = document.CreateElement("name");
			nameElem.AppendChild(document.CreateTextNode(this.Name));
			customerElem.AppendChild(nameElem);

			XmlElement phoneElem = document.CreateElement("phone");
			phoneElem.AppendChild(document.CreateTextNode(this.Phone));
			customerElem.AppendChild(phoneElem);

			XmlElement addressElem = document.CreateElement("address");
			addressElem.AppendChild(document.CreateTextNode(this.Address));
			customerElem.AppendChild(addressElem);

			XmlElement furnaceElem = document.CreateElement("furnace");
			furnaceElem.AppendChild(document.CreateTextNode(this.Furnace));
			customerElem.AppendChild(furnaceElem);

			XmlElement dateElem = document.CreateElement("date");
			if (this.Date != null)
			{
				dateElem.AppendChild(document.CreateTextNode(this.Date));
			}
			customerElem.AppendChild(dateElem);
			
			XmlElement recordsElem = document.CreateElement("records");
			foreach (var record in this.customerRecords)
			{
				recordsElem.AppendChild(record.Save(document));
			}
			customerElem.AppendChild(recordsElem);

			return customerElem;
		}

		/// <summary>
		/// Loads instance from the specified element.
		/// </summary>
		/// <param name="data">The data element.</param>
		public void Load(XmlElement data)
		{
			this.Name = data["name"].InnerText;
			this.Phone = data["phone"].InnerText;
			this.Address = data["address"].InnerText;
			this.Furnace = data["furnace"].InnerText;
			this.Date = data["date"].InnerText;

			XmlNodeList recordList = data["records"].GetElementsByTagName("record");
			for (int i = 0; i < recordList.Count; ++i)
			{
				this.customerRecords.Add(new CustomerRecord(recordList[i] as XmlElement));
			}
		}

		#endregion
	}
}
