using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA_TP3
{
    public class Environement
    {
        private Forest Forest;

        private Agent agent;
        private int mesurePerf = 0;

        public Environement()
        {
            //Initialisation
            Forest = new Forest(3); // Au départ la Forest est une grille 3x3
        }

        public void setAgent(Agent agent)
        {
            this.agent = agent;
        }

        public void majMesurePerf(Action action)
        {
            switch (action)
            {
                case  Action.MOURIR:
                    mesurePerf += -10 * Forest.GetSize();
                    break;
                case Action.LANCER_PIERRE:
                    mesurePerf -= 10;
                    break;
                case Action.PRENDRE_PORTAIL:
                    mesurePerf += 10 * Forest.GetSize();
                    break;
                case Action.SE_DEPLACER:
                    mesurePerf -= 1;
                    break;
            }
        }

        public Forest GetForest()
        {
            return Forest;
        }

        public int getsize()
        {
            return Forest.GetSize();
        }

        public void informe(Action action, Direction direction)
        {

            Tuple<int, int> pos = agent.getPosition();

            majMesurePerf(action);        // met a jour la mesure de perf

            Tuple<int, int> CellVisée = null;

            if (direction != Direction.NULL)
            {
                switch (direction)
                {
                    case Direction.HAUT:
                        CellVisée = new Tuple<int,int>(pos.Item1 - 1, pos.Item2);
                        break;
                    case Direction.BAS:
                        CellVisée = new Tuple<int,int>(pos.Item1 + 1, pos.Item2);
                        break;
                    case Direction.GAUCHE:
                        CellVisée = new Tuple<int,int>(pos.Item1, pos.Item2 - 1);
                        break;
                    case Direction.DROITE:
                        CellVisée = new Tuple<int,int>(pos.Item1, pos.Item2 + 1);
                        break;
                }
            }

            if (action.Equals(Action.PRENDRE_PORTAIL) && Forest.GetCell(agent.getPosition()).Equals(ElementCell.PORTAIL))
            {
                Forest = new Forest(Forest.GetSize() + 1);
                agent = new Agent(this);
            }

            if (action.Equals(Action.LANCER_PIERRE))
            {
                if (Forest.GetCell(CellVisée).Equals(ElementCell.MONSTRE))
                {
                    Forest.SetCell(CellVisée, ElementCell.VIDE);
                }
            }


            if (action.Equals(Action.SE_DEPLACER))
            {
                // Si on se deplace sur une Cell crevasse ou monstre -> scouic
                if (Forest.GetCell(CellVisée).Equals(ElementCell.MONSTRE) || Forest.GetCell(CellVisée).Equals(ElementCell.CREVASSE))
                {
                    agent.meurt(CellVisée, Forest.GetCell(CellVisée));
                    majMesurePerf(Action.MOURIR);
                    Console.WriteLine("je suis mouru");
                    // revient en 0,0
                    agent.setPosition(new Tuple<int,int>(0, 0));
                }
            }
        }

        public Agent getAgent()
        {
            return agent;
        }

        public int getMesurePerf()
        {
            return mesurePerf;
        }
    }
}
