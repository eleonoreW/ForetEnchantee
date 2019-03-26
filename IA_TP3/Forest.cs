using System;

namespace IA_TP3
{
    public class Forest
    {

        readonly int size;   // size de départ initialisée à 3
        static ElementCell[,] forest;

        public Forest(int size_)
        {
            size = size_;
            forest = new ElementCell[size_, size_];
            RemplieForest();

        }

        private void RemplieForest()
        {
            // Place le PORTAL et le obstacles aléatoirement
            Random rand = new Random();
            int randomValeur;
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    randomValeur = rand.Next(10);
                    if (randomValeur < 2 && (i != 0 || j != 0)) // si 0 ou 1
                        forest[i, j] = ElementCell.MONSTER;
                    else if (randomValeur >= 2 && randomValeur < 4 && (i != 0 || j != 0)) // si 2 ou 3
                        forest[i, j] = ElementCell.TRAP;
                    else
                        forest[i, j] = ElementCell.EMPTY;
                }

            // placer PORTAL
            int randomLigne = rand.Next(size);
            int randomCol = rand.Next(size);
            forest[randomLigne, randomCol] = ElementCell.PORTAL;

            UpdateCells();
        }

        public ElementCell[,] getCarte()
        {
            return forest;
        }

        public ElementCell GetCell(Tuple<int, int> coordonnees)
        {
            if (coordonnees.Item1 < 0 || coordonnees.Item1 >= size || coordonnees.Item2 < 0 || coordonnees.Item2 >= size)
                return ElementCell.NULL;
            return forest[coordonnees.Item1, coordonnees.Item2];
        }

        public int GetSize()
        {
            return size;
        }

        public void SetCell(Tuple<int, int> pos, ElementCell element)
        {
            forest[pos.Item1, pos.Item2] = element;
        }

        public void UpdateCells()
        {

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    if(forest[i,j].Equals(ElementCell.SMELL) || forest[i, j].Equals(ElementCell.SMELL_WIND) || forest[i, j].Equals(ElementCell.WIND))
                    {
                        forest[i, j] = ElementCell.EMPTY;
                    }
                }
            // place les WINDs autour des crevasses
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    if (forest[i, j].Equals(ElementCell.TRAP))
                    {
                        // UP
                        if (i > 0)
                        {
                            if (!forest[i - 1, j].Equals(ElementCell.MONSTER) && !forest[i - 1, j].Equals(ElementCell.PORTAL))
                                forest[i - 1, j] = ElementCell.WIND;
                        }
                        // DOWN
                        if (i < size - 1)
                        {
                            if (!forest[i + 1, j].Equals(ElementCell.MONSTER) && !forest[i + 1, j].Equals(ElementCell.PORTAL))
                                forest[i + 1, j] = ElementCell.WIND;
                        }
                        // LEFT
                        if (j > 0)
                        {
                            if (!forest[i, j - 1].Equals(ElementCell.MONSTER) && !forest[i, j - 1].Equals(ElementCell.PORTAL))
                                forest[i, j - 1] = ElementCell.WIND;
                        }
                        // RIGHT
                        if (j < size - 1)
                        {
                            if (!forest[i, j + 1].Equals(ElementCell.MONSTER) && !forest[i, j + 1].Equals(ElementCell.PORTAL))
                                forest[i, j + 1] = ElementCell.WIND;
                        }
                    }
                }

            // place les SMELLs autour des crevasses
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    if (forest[i, j].Equals(ElementCell.MONSTER))
                    {
                        // UP
                        if (i > 0)
                        {
                            if (!forest[i - 1, j].Equals(ElementCell.TRAP) && !forest[i - 1, j].Equals(ElementCell.PORTAL))
                                if (forest[i - 1, j].Equals(ElementCell.WIND))
                                    forest[i - 1, j] = ElementCell.SMELL_WIND;
                                else
                                    forest[i - 1, j] = ElementCell.SMELL;
                        }
                        // DOWN
                        if (i < size - 1)
                        {
                            if (!forest[i + 1, j].Equals(ElementCell.TRAP) && !forest[i + 1, j].Equals(ElementCell.PORTAL))
                                if (forest[i + 1, j].Equals(ElementCell.WIND))
                                    forest[i + 1, j] = ElementCell.SMELL_WIND;
                                else
                                    forest[i + 1, j] = ElementCell.SMELL;
                        }
                        // LEFT
                        if (j > 0)
                        {
                            if (!forest[i, j - 1].Equals(ElementCell.TRAP) && !forest[i, j - 1].Equals(ElementCell.PORTAL))
                                if (forest[i, j - 1].Equals(ElementCell.WIND))
                                    forest[i, j - 1] = ElementCell.SMELL_WIND;
                                else
                                    forest[i, j - 1] = ElementCell.SMELL;
                        }
                        // RIGHT
                        if (j < size - 1)
                        {
                            if (!forest[i, j + 1].Equals(ElementCell.TRAP) && !forest[i, j + 1].Equals(ElementCell.PORTAL))
                                if (forest[i, j + 1].Equals(ElementCell.WIND))
                                    forest[i, j + 1] = ElementCell.SMELL_WIND;
                                else
                                    forest[i, j + 1] = ElementCell.SMELL;
                        }
                    }
                }
        }

    }
}
