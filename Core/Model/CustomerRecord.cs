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
	/// Customer record class
	/// </summary>
	public class CustomerRecord : ISaveable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CustomerRecord"/> class.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <param name="text">The text.</param>
		public CustomerRecord(DateTime date, string text)
		{
			this.Date = date;
			this.Text = text;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CustomerRecord"/> class.
		/// </summary>
		/// <param name="data">The data.</param>
		public CustomerRecord(XmlElement data)
		{
			this.Load(data);
		}

		#region PROPERTIES

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
		/// Gets or sets the text.
		/// </summary>
		/// <value>The text.</value>
		public string Text
		{
			get;
			set;
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
			XmlElement elem = document.CreateElement("record");

			XmlElement nameElem = document.CreateElement("date");
			nameElem.AppendChild(document.CreateTextNode(String.Format("{0}.{1}.{2}", this.Date.Day, this.Date.Month, this.Date.Year)));
			elem.AppendChild(nameElem);

			XmlElement textElem = document.CreateElement("text");
			textElem.AppendChild(document.CreateTextNode(this.Text));
			elem.AppendChild(textElem);

			return elem;
		}

		/// <summary>
		/// Loads instance from the specified element.
		/// </summary>
		/// <param name="data">The data element.</param>
		public void Load(XmlElement data)
		{
			this.Text = data["text"].InnerText;
			this.Date = System.Convert.ToDateTime(data["date"].InnerText);
		}

		#endregion
	}
}
