using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA_TP3
{
    class CapteurOdeur : Capteur
    {
        public CapteurOdeur(Forest forest) : base(forest)
        {

        }

        public bool Sonder(Tuple<int, int> pos)
        {
            return forest.GetCell(pos).Equals(ElementCell.SMELL) || forest.GetCell(pos).Equals(ElementCell.SMELL_WIND);
        }
    }
}
