using IA_TP3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA_TP3
{
    class Capteur 
    {
        public Capteur()
        {
        }

        public HashSet<Impact> Feel(int i, int j)
        {
            return Forest.getCell(i, j).GetImpacts();
        }
    }
}
