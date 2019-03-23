using System;

namespace IA_TP3
{
    public class Forest
    {

        readonly int size;               // size de départ initialisée à 3
        static ElementCell[,] forest;

        public Forest(int size_)
        {
            size = size_;
            forest = new ElementCell[size_, size_];
            RemplieForest();

        }

        private void RemplieForest()
        {
            // Place le portail et le obstacles aléatoirement
            Random rand = new Random();
            int randomValeur;
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    randomValeur = rand.Next(10);
                    if (randomValeur < 2 && (i != 0 || j != 0)) // si 0 ou 1
                        forest[i, j] = ElementCell.MONSTRE;
                    else if (randomValeur >= 2 && randomValeur < 4 && (i != 0 || j != 0)) // si 2 ou 3
                        forest[i, j] = ElementCell.CREVASSE;
                    else
                        forest[i, j] = ElementCell.VIDE;
                }

            // placer portail
            int randomLigne = rand.Next(size);
            int randomCol = rand.Next(size);
            forest[randomLigne, randomCol] = ElementCell.PORTAIL;

            // place les vents autour des crevasses
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    if (forest[i, j].Equals(ElementCell.CREVASSE))
                    {
                        // HAUT
                        if (i > 0)
                        {
                            if (!forest[i - 1, j].Equals(ElementCell.MONSTRE) && !forest[i - 1, j].Equals(ElementCell.PORTAIL))
                                forest[i - 1, j] = ElementCell.VENT;
                        }
                        // BAS
                        if (i < size - 1)
                        {
                            if (!forest[i + 1, j].Equals(ElementCell.MONSTRE) && !forest[i + 1, j].Equals(ElementCell.PORTAIL))
                                forest[i + 1, j] = ElementCell.VENT;
                        }
                        // GAUCHE
                        if (j > 0)
                        {
                            if (!forest[i, j - 1].Equals(ElementCell.MONSTRE) && !forest[i, j - 1].Equals(ElementCell.PORTAIL))
                                forest[i, j - 1] = ElementCell.VENT;
                        }
                        // DROITE
                        if (j < size - 1)
                        {
                            if (!forest[i, j + 1].Equals(ElementCell.MONSTRE) && !forest[i, j + 1].Equals(ElementCell.PORTAIL))
                                forest[i, j + 1] = ElementCell.VENT;
                        }
                    }
                }

            // place les cacas autour des crevasses
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    if (forest[i, j].Equals(ElementCell.MONSTRE))
                    {
                        // HAUT
                        if (i > 0)
                        {
                            if (!forest[i - 1, j].Equals(ElementCell.CREVASSE) && !forest[i - 1, j].Equals(ElementCell.PORTAIL))
                                if (forest[i - 1, j].Equals(ElementCell.VENT))
                                    forest[i - 1, j] = ElementCell.CACA_VENT;
                                else
                                    forest[i - 1, j] = ElementCell.CACA;
                        }
                        // BAS
                        if (i < size - 1)
                        {
                            if (!forest[i + 1, j].Equals(ElementCell.CREVASSE) && !forest[i + 1, j].Equals(ElementCell.PORTAIL))
                                if (forest[i + 1, j].Equals(ElementCell.VENT))
                                    forest[i + 1, j] = ElementCell.CACA_VENT;
                                else
                                    forest[i + 1, j] = ElementCell.CACA;
                        }
                        // GAUCHE
                        if (j > 0)
                        {
                            if (!forest[i, j - 1].Equals(ElementCell.CREVASSE) && !forest[i, j - 1].Equals(ElementCell.PORTAIL))
                                if (forest[i, j - 1].Equals(ElementCell.VENT))
                                    forest[i, j - 1] = ElementCell.CACA_VENT;
                                else
                                    forest[i, j - 1] = ElementCell.CACA;
                        }
                        // DROITE
                        if (j < size - 1)
                        {
                            if (!forest[i, j + 1].Equals(ElementCell.CREVASSE) && !forest[i, j + 1].Equals(ElementCell.PORTAIL))
                                if (forest[i, j + 1].Equals(ElementCell.VENT))
                                    forest[i, j + 1] = ElementCell.CACA_VENT;
                                else
                                    forest[i, j + 1] = ElementCell.CACA;
                        }
                    }
                }
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

    }
}
