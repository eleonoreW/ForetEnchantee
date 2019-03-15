using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA_TP3
{
	enum Element { Nothing, Start, Monster, Trap, Portal }
	enum Impact { Smell, Wind }

	class Cell
	{
		private int posI, posJ;
		private Element element;
		private HashSet<Impact> impacts;

		public Cell(int i, int j)
		{
			posI = i;
			posJ = j;
			element = Element.Nothing;
			impacts = new HashSet<Impact>();
		}

		public void SetElement(Element e)
		{
			element = e;
		}

		public Element GetElement()
		{
			return element;
		}

		public bool IsElement(Element e)
		{
			return element == e;
		}

		public void AddImpact(Impact i)
		{
			impacts.Add(i);
		}

		override public string ToString()
		{
			string str = "";

			switch (element) {
				case Element.Start:
					str += "D";
					break;
				case Element.Monster:
					str += "M";
					break;
				case Element.Trap:
					str += "T";
					break;
				case Element.Portal:
					str += "P";
					break;
				default:
					str += "_";
					break;
			}

			foreach(Impact impact in impacts) {
				switch (impact) {
					case Impact.Smell:
						str += "S";
						break;
					case Impact.Wind:
						str += "W";
						break;
				}
			}

			return str;
		}
	}
}
