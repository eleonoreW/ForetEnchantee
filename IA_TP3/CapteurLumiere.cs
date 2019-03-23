using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA_TP3
{
    class CapteurLumiere : Capteur
    {
        public CapteurLumiere(Forest forest) : base(forest)
        {

        }
        public bool sonder(Tuple<int, int> pos)
        {
            return forest.GetCell(pos).Equals(ElementCell.PORTAIL);
        }

        public Dictionary<Direction, bool> CellsAlentour(Tuple<int, int> pos)
        {
            Dictionary<Direction, bool> voisinsPossibles = new Dictionary<Direction,bool>();

            // HAUT
            voisinsPossibles.Add(Direction.HAUT, forest.GetCell(new Tuple<int,int>(pos.Item1 - 1, pos.Item2)) != ElementCell.NULL);
            // BAS
            voisinsPossibles.Add(Direction.BAS, forest.GetCell(new Tuple<int,int>(pos.Item1 + 1, pos.Item2)) != ElementCell.NULL);
            // GAUCHE
            voisinsPossibles.Add(Direction.GAUCHE, forest.GetCell(new Tuple<int,int>(pos.Item1, pos.Item2 - 1)) != ElementCell.NULL);
            // DROITE
            voisinsPossibles.Add(Direction.DROITE, forest.GetCell(new Tuple<int,int>(pos.Item1, pos.Item2 + 1)) != ElementCell.NULL);

            return voisinsPossibles;
        }
    }
}
