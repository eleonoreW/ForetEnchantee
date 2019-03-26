using System;

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
                case  Action.DIE:
                    mesurePerf += -10 * Forest.GetSize();
                    break;
                case Action.LAUNCH_ROCK:
                    mesurePerf -= 10;
                    break;
                case Action.TAKE_PORTAL:
                    mesurePerf += 10 * Forest.GetSize();
                    break;
                case Action.MOVE:
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

            majMesurePerf(action);        // met a jour la mesure de performance

            Tuple<int, int> CellVisée = null;

            if (direction != Direction.NULL)
            {
                switch (direction)
                {
                    case Direction.UP:
                        CellVisée = new Tuple<int,int>(pos.Item1 - 1, pos.Item2);
                        break;
                    case Direction.DOWN:
                        CellVisée = new Tuple<int,int>(pos.Item1 + 1, pos.Item2);
                        break;
                    case Direction.LEFT:
                        CellVisée = new Tuple<int,int>(pos.Item1, pos.Item2 - 1);
                        break;
                    case Direction.RIGHT:
                        CellVisée = new Tuple<int,int>(pos.Item1, pos.Item2 + 1);
                        break;
                }
            }

            if (action.Equals(Action.TAKE_PORTAL) && Forest.GetCell(agent.getPosition()).Equals(ElementCell.PORTAL))
            {
                Forest = new Forest(Forest.GetSize() + 1);
                agent = new Agent(this);
            }

            if (action.Equals(Action.LAUNCH_ROCK))
            {
                if (Forest.GetCell(CellVisée).Equals(ElementCell.MONSTER))
                {
                    Forest.SetCell(CellVisée, ElementCell.EMPTY);
                    Forest.UpdateCells();
                }
                
            }


            if (action.Equals(Action.MOVE))
            {
                // Si on se deplace sur une Cell TRAP ou MONSTER -> scouic
                if (Forest.GetCell(CellVisée).Equals(ElementCell.MONSTER) || Forest.GetCell(CellVisée).Equals(ElementCell.TRAP))
                {
                    agent.meurt(CellVisée, Forest.GetCell(CellVisée));
                    majMesurePerf(Action.DIE);
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
