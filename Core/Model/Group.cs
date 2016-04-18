using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using SkladCore.Interfaces;
using SkladCore.Utilities;

namespace SkladCore.Model
{
	/// <summary>
	/// Class of "Group"
	/// </summary>
	public class Group : ISaveable
	{
		#region ATTRIBUTES

		private List<Card> cards;
		private CardComparer comparer;

		#endregion ATTRIBUTES

		/// <summary>
		/// Initializes a new instance of the <see cref="Group"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="color">The color.</param>
		public Group(string name, Color color)
		{
			this.Init();
			this.Name = name;
			this.Color = color;
            this.InitGroupBrush();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Group"/> class.
		/// </summary>
		/// <param name="data">The data.</param>
		public Group(XmlElement data)
		{
			this.Init();
			this.Load(data);
		}

		/// <summary>
		/// Inits this instance.
		/// </summary>
		private void Init()
		{
			this.cards = new List<Card>();
			this.comparer = new CardComparer();
		}

		/// <summary>
		/// Adds the card.
		/// </summary>
		/// <param name="card">The card.</param>
        public void AddCard(Card card)
        {
            if (!this.cards.Contains(card))
            {
                this.cards.Add(card);
            }
        }

		/// <summary>
		/// Deletes the card.
		/// </summary>
		/// <param name="card">The card.</param>
        public void DeleteCard(Card card)
        {
            if (this.cards.Contains(card))
            {
                this.cards.Remove(card);
            }
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
		/// Gets or sets the color.
		/// </summary>
		/// <value>The color.</value>
		public Color Color
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the group brush.
		/// </summary>
		/// <value>The group brush.</value>
		public Brush GroupBrush
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the cards.
		/// </summary>
		/// <value>The cards.</value>
		public List<Card> Cards
		{
			get
			{
				List<Card> result = this.cards.ToList();
				result.Sort(this.comparer);
				return result;
			}
		}

		#endregion PROPERTIES

		/// <summary>
		/// Inits the group brush.
		/// </summary>
        public void InitGroupBrush()
        {
            this.GroupBrush = new SolidColorBrush(this.Color);
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
			foreach (var card in this.Cards)
			{
				result += card.GetPriceStock(stockType, year);
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
			foreach (var card in this.Cards)
			{
				result += card.GetCountStock(stockType, year);
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
			XmlElement groupElem = document.CreateElement("group");
			
			XmlElement nameElem = document.CreateElement("name");
			nameElem.AppendChild(document.CreateTextNode(this.Name));
			groupElem.AppendChild(nameElem);

			XmlElement colorElem = document.CreateElement("colorSchema");
			
			XmlAttribute r = document.CreateAttribute("R");
			r.Value = System.Convert.ToDouble(this.Color.R).ToString();
			colorElem.Attributes.Append(r);

			XmlAttribute g = document.CreateAttribute("G");
			g.Value = System.Convert.ToDouble(this.Color.G).ToString();
			colorElem.Attributes.Append(g);

			XmlAttribute b = document.CreateAttribute("B");
			b.Value = System.Convert.ToDouble(this.Color.B).ToString();
			colorElem.Attributes.Append(b);

			groupElem.AppendChild(colorElem);
			XmlElement cardsElem = document.CreateElement("cards");
			foreach (var card in this.cards)
			{
				cardsElem.AppendChild(card.Save(document));
			}
			groupElem.AppendChild(cardsElem);

			return groupElem;
		}

		/// <summary>
		/// Loads instance from the specified element.
		/// </summary>
		/// <param name="data">The data element.</param>
		public void Load(XmlElement data)
		{
			XmlNode nameNode = data["name"];
			this.Name = nameNode.InnerText;

			XmlNode colorNode = data["colorSchema"];
			byte r = System.Convert.ToByte(colorNode.Attributes["R"].Value);
			byte g = System.Convert.ToByte(colorNode.Attributes["G"].Value);
			byte b = System.Convert.ToByte(colorNode.Attributes["B"].Value);

			this.Color = Color.FromRgb(r, g, b);
            this.InitGroupBrush();

			XmlNodeList cardList = data["cards"].GetElementsByTagName("card");
			for (int i = 0; i < cardList.Count; ++i)
			{
				Card card = new Card(cardList[i] as XmlElement);
				this.cards.Add(card);
			}
		}

		#endregion
	}
}
