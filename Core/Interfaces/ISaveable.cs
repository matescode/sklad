using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SkladCore.Interfaces
{
	/// <summary>
	/// interface for saveable items in project
	/// </summary>
	public interface ISaveable
	{
		/// <summary>
		/// Saves instance into the specified document.
		/// </summary>
		/// <param name="document">The document.</param>
		/// <returns>XMLElement of object to be saved</returns>
		XmlElement Save(XmlDocument document);

		/// <summary>
		/// Loads instance from the specified element.
		/// </summary>
		/// <param name="data">The data element.</param>
		void Load(XmlElement data);
	}
}
