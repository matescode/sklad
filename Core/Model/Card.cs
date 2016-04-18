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
	/// Card class
	/// </summary>
	public class Card : ISaveable
	{
		#region ATTRIBUTES

		private List<Record> records;

        private RecordComparer comparer;

		#endregion ATTRIBUTES

		/// <summary>
		/// Initializes a new instance of the <see cref="Card"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="codeJK">The code JK.</param>
		public Card(string name, string codeJK)
		{
			this.Init();
			this.Name = name;
			this.CodeJK = codeJK;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Card"/> class.
		/// </summary>
		/// <param name="data">The data.</param>
		public Card(XmlElement data)
		{
			this.Init();
			this.Load(data);
		}

		/// <summary>
		/// Inits this instance.
		/// </summary>
		private void Init()
		{
			this.records = new List<Record>();
            this.comparer = new RecordComparer();
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
		/// Gets or sets the code JK.
		/// </summary>
		/// <value>The code JK.</value>
		public string CodeJK
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the stock count.
		/// </summary>
		/// <value>The stock count.</value>
		public int StockCount
		{
			get
			{
				int result = 0;
				foreach (var record in this.Records)
				{
					result += record.AmountChange;
				}
				return result;
			}
		}

		/// <summary>
		/// Gets the stock price.
		/// </summary>
		/// <value>The stock price.</value>
        public double StockPrice
        {
            get
            {
                double result = 0;
                foreach (var record in this.Records)
                {
                    result += record.PriceChange;
                }
                return result;
            }
        }

		/// <summary>
		/// Gets the records.
		/// </summary>
		/// <value>The records.</value>
		public List<Record> Records
		{
			get
			{
                List<Record> result = this.records.ToList();
                result.Sort(this.comparer);
                return result;
			}
		}

		#endregion PROPERTIES

		/// <summary>
		/// Adds the record.
		/// </summary>
		/// <param name="record">The record.</param>
        public void AddRecord(Record record)
        {
            if (!this.records.Contains(record))
            {
                this.records.Add(record);
                this.CountCurrentRecordStock();
            }
        }

		/// <summary>
		/// Deletes the record.
		/// </summary>
		/// <param name="record">The record.</param>
        public void DeleteRecord(Record record)
        {
            if (this.records.Contains(record))
            {
                this.records.Remove(record);
                this.CountCurrentRecordStock();
            }
        }

		/// <summary>
		/// Counts the current record stock.
		/// </summary>
		public void CountCurrentRecordStock()
		{
			foreach (var record in this.records)
			{
				this.CountCurrentRecordStock(record);
			}
		}

		/// <summary>
		/// Counts the current record stock.
		/// </summary>
		/// <param name="record">The record.</param>
		private void CountCurrentRecordStock(Record record)
		{
            double priceResult = 0;
			int result = 0;
			foreach (var r in this.records.Where(item => item.Date < record.Date))
			{
				result += r.AmountChange;
                priceResult += r.PriceChange;
			}
			result += record.AmountChange;
            priceResult += record.PriceChange;
			record.CurrentStockCount = result;
            record.CurrentStockPrice = Math.Round(priceResult, 2);
		}

		/// <summary>
		/// Gets the price stock.
		/// </summary>
		/// <param name="stockType">Type of the stock.</param>
		/// <param name="year">The year.</param>
		/// <returns></returns>
		public double GetPriceStock(StockStatusType stockType, int year)
		{
			double result = 0;

			foreach (var record in this.Records)
			{
				switch (stockType)
				{
					case StockStatusType.AfterYear:
						if (record.Date.Year <= year)
						{
							result += record.PriceChange;
						}
						break;

					case StockStatusType.Current:
						result += record.PriceChange;
						break;

					case StockStatusType.ForYear:
						if (record.Date.Year == year)
						{
							result += record.PriceChange;
						}
						break;
				}
			}
			return result;
		}

		/// <summary>
		/// Gets the count stock.
		/// </summary>
		/// <param name="stockType">Type of the stock.</param>
		/// <param name="year">The year.</param>
		/// <returns></returns>
		public int GetCountStock(StockStatusType stockType, int year)
		{
			int result = 0;

			foreach (var record in this.Records)
			{
				switch (stockType)
				{
					case StockStatusType.AfterYear:
						if (record.Date.Year <= year)
						{
							result += record.AmountChange;
						}
						break;

					case StockStatusType.Current:
						result += record.AmountChange;
						break;

					case StockStatusType.ForYear:
						if (record.Date.Year == year)
						{
							result += record.AmountChange;
						}
						break;
				}
			}
			return result;
		}

		#region ISaveable Members

		/// <summary>
		/// Saves instance into the specified document.
		/// </summary>
		/// <param name="document">The document.</param>
		/// <returns>XMLElement of object to be saved</returns>
		public XmlElement Save(XmlDocument document)
		{
			XmlElement cardElem = document.CreateElement("card");

			XmlAttribute codeJKAttr = document.CreateAttribute("codeJK");
			codeJKAttr.Value = this.CodeJK;
			cardElem.Attributes.Append(codeJKAttr);

			XmlElement nameElem = document.CreateElement("name");
			nameElem.AppendChild(document.CreateTextNode(this.Name));
			cardElem.AppendChild(nameElem);

			XmlElement recordsElem = document.CreateElement("records");
			foreach (var record in this.records)
			{
				recordsElem.AppendChild(record.Save(document));
			}
			cardElem.AppendChild(recordsElem);

			return cardElem;
		}

		/// <summary>
		/// Loads instance from the specified element.
		/// </summary>
		/// <param name="data">The data element.</param>
		public void Load(XmlElement data)
		{
			XmlNode nameNode = data["name"];
			this.Name = nameNode.InnerText;
			this.CodeJK = data.Attributes["codeJK"].Value;

			XmlNodeList recordList = data["records"].GetElementsByTagName("record");
			for (int i = 0; i < recordList.Count; ++i)
			{
				Record record = new Record(recordList[i] as XmlElement);
				this.records.Add(record);
			}

			this.CountCurrentRecordStock();
		}

		#endregion
	}
}
