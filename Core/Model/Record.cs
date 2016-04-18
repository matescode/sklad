using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using SkladCore.Interfaces;

namespace SkladCore.Model
{
	/// <summary>
	/// Record type
	/// </summary>
	public enum RecordType
	{
		/// <summary>
		/// Taking
		/// </summary>
		Taking,
		/// <summary>
		/// Issue
		/// </summary>
		Issue
	}

	/// <summary>
	/// Record class
	/// </summary>
	public class Record : ISaveable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Record"/> class.
		/// </summary>
		/// <param name="recordType">Type of the record.</param>
		/// <param name="date">The date.</param>
		/// <param name="price">The price.</param>
		/// <param name="amount">The amount.</param>
		/// <param name="note">The note.</param>
		public Record(RecordType recordType, DateTime date, double price, int amount, string note)
		{
			this.RecordType = recordType;
			this.Date = date;
			this.Price = price;
			this.Amount = amount;
			this.Note = note;
			this.CurrentStockCount = 0;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Record"/> class.
		/// </summary>
		/// <param name="data">The data.</param>
		public Record(XmlElement data)
		{
			this.Load(data);
		}

		#region PROPERTIES

		/// <summary>
		/// Gets or sets the type of the record.
		/// </summary>
		/// <value>The type of the record.</value>
		public RecordType RecordType
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the date.
		/// </summary>
		/// <value>The date.</value>
		public DateTime Date
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the price.
		/// </summary>
		/// <value>The price.</value>
		public double Price
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the amount.
		/// </summary>
		/// <value>The amount.</value>
		public int Amount
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the note.
		/// </summary>
		/// <value>The note.</value>
		public string Note
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the current stock count.
		/// </summary>
		/// <value>The current stock count.</value>
		public int CurrentStockCount
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the current stock price.
		/// </summary>
		/// <value>The current stock price.</value>
        public double CurrentStockPrice
        {
            get;
            set;
        }

		/// <summary>
		/// Gets the taking amount.
		/// </summary>
		/// <value>The taking amount.</value>
        public string TakingAmount
        {
            get
            {
                if (this.RecordType == Model.RecordType.Taking)
                {
                    return this.Amount.ToString();
                }
                else
                {
                    return "-";
                }
            }
        }

		/// <summary>
		/// Gets the issue amount.
		/// </summary>
		/// <value>The issue amount.</value>
        public string IssueAmount
        {
            get
            {
                if (this.RecordType == Model.RecordType.Issue)
                {
                    return this.Amount.ToString();
                }
                else
                {
                    return "-";
                }
            }
        }

		/// <summary>
		/// Gets the taking price.
		/// </summary>
		/// <value>The taking price.</value>
        public string TakingPrice
        {
            get
            {
                if (this.RecordType == Model.RecordType.Taking)
                {
                    return (this.Amount * this.Price).ToString();
                }
                else
                {
                    return "-";
                }
            }
        }

		/// <summary>
		/// Gets the issue price.
		/// </summary>
		/// <value>The issue price.</value>
        public string IssuePrice
        {
            get
            {
                if (this.RecordType == Model.RecordType.Issue)
                {
                    return (this.Amount * this.Price).ToString();
                }
                else
                {
                    return "-";
                }
            }
        }

		/// <summary>
		/// Gets the amount change.
		/// </summary>
		/// <value>The amount change.</value>
		public int AmountChange
		{
			get
			{
				return (this.RecordType == Model.RecordType.Taking) ? this.Amount : -this.Amount;
			}
		}

		/// <summary>
		/// Gets the price change.
		/// </summary>
		/// <value>The price change.</value>
        public double PriceChange
        {
            get
            {
                return this.AmountChange * this.Price;
            }
        }

		#endregion PROPERTIES

		#region ISaveable Members

		/// <summary>
		/// Saves instance into the specified document.
		/// </summary>
		/// <param name="document">The document.</param>
		/// <returns>XMLElement of object to be saved</returns>
		public XmlElement Save(XmlDocument document)
		{
			XmlElement recordElem = document.CreateElement("record");
			
			XmlElement kindElem = document.CreateElement("kind");
			kindElem.AppendChild(document.CreateTextNode(this.RecordType.ToString()));
			recordElem.AppendChild(kindElem);

			XmlElement dateElem = document.CreateElement("date");
			dateElem.AppendChild(document.CreateTextNode(String.Format("{0}.{1}.{2}", this.Date.Day, this.Date.Month, this.Date.Year)));
			recordElem.AppendChild(dateElem);

			XmlElement priceElem = document.CreateElement("price");
			priceElem.AppendChild(document.CreateTextNode(this.Price.ToString()));
			recordElem.AppendChild(priceElem);

			XmlElement amountElem = document.CreateElement("amount");
			amountElem.AppendChild(document.CreateTextNode(this.Amount.ToString()));
			recordElem.AppendChild(amountElem);

			XmlElement noteElem = document.CreateElement("note");
			noteElem.AppendChild(document.CreateTextNode(this.Note.ToString()));
			recordElem.AppendChild(noteElem);

			return recordElem;
		}

		/// <summary>
		/// Loads instance from the specified element.
		/// </summary>
		/// <param name="data">The data element.</param>
		public void Load(XmlElement data)
		{
			this.RecordType = (data["kind"].InnerText == "Taking") ? RecordType.Taking : RecordType.Issue;
			this.Date = System.Convert.ToDateTime(data["date"].InnerText);
			this.Price = Double.Parse(data["price"].InnerText);
			this.Amount = Int32.Parse(data["amount"].InnerText);
			this.Note = data["note"].InnerText;
			this.CurrentStockCount = 0;
		}

		#endregion
	}
}
