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

		public string ToString(int rowIndex)
		{
			string str = "";

			switch (rowIndex) {
				case 0:
					str += impacts.Contains(Impact.Smell) ? "S " : "  ";
					str += element == Element.Start ? "D " : "  ";
					str += impacts.Contains(Impact.Wind) ? "W " : "  ";
					break;
				case 1:
					str += element == Element.Monster ? "M " : "  ";
					str += "  "; // Affichage pêrsonnage
					str += element == Element.Trap ? "T " : "  ";
					break;
				case 2:
					str += impacts.Contains(Impact.Smell) ? "S " : "  ";
					str += element == Element.Portal ? "P " : "  ";
					str += impacts.Contains(Impact.Wind) ? "W " : "  ";
					break;
			}

			return str;
		}
	}
}
