using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA_TP3
{
    class CapteurVent : Capteur
    {
        public CapteurVent(Forest forest) : base(forest)
        {
       
        }

        
        public bool sonder(Tuple<int, int> pos)
        {
            return forest.GetCell(pos).Equals(ElementCell.VENT) || forest.GetCell(pos).Equals(ElementCell.CACA_VENT);
        }
    }
}
