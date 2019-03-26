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
        public bool Sonder(Tuple<int, int> pos)
        {
            return forest.GetCell(pos).Equals(ElementCell.PORTAL);
        }

        public Dictionary<Direction, bool> CellsAlentour(Tuple<int, int> pos)
        {
            Dictionary<Direction, bool> voisinsPossibles = new Dictionary<Direction,bool>();

            // UP
            voisinsPossibles.Add(Direction.UP, forest.GetCell(new Tuple<int,int>(pos.Item1 - 1, pos.Item2)) != ElementCell.NULL);
            // DOWN
            voisinsPossibles.Add(Direction.DOWN, forest.GetCell(new Tuple<int,int>(pos.Item1 + 1, pos.Item2)) != ElementCell.NULL);
            // LEFT
            voisinsPossibles.Add(Direction.LEFT, forest.GetCell(new Tuple<int,int>(pos.Item1, pos.Item2 - 1)) != ElementCell.NULL);
            // RIGHT
            voisinsPossibles.Add(Direction.RIGHT, forest.GetCell(new Tuple<int,int>(pos.Item1, pos.Item2 + 1)) != ElementCell.NULL);

            return voisinsPossibles;
        }
    }
}
