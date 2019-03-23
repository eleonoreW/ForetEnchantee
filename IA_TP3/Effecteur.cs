using System;

namespace IA_TP3
{
    class Effecteur
    {
        Environement env;
        Agent agent;

        public Effecteur(Environement env, Agent agent)
        {
            this.env = env;
            this.agent = agent;
        }

        public void Fait(Tuple<int, int> pos, Action action, Direction direction)
        {
            env.informe(action, direction); // il connait pos en demandant directement à l'agent

            if (agent.IsLive())
            {
                if (action.Equals(Action.LANCER_PIERRE))
                {
                    switch (direction)
                    {
                        case Direction.HAUT:
                            agent.tirMonstre(new Tuple<int,int>(pos.Item1 - 1, pos.Item2));
                            break;
                        case Direction.BAS:
                            agent.tirMonstre(new Tuple<int,int>(pos.Item1 + 1, pos.Item2));
                            break;
                        case Direction.GAUCHE:
                            agent.tirMonstre(new Tuple<int,int>(pos.Item1, pos.Item2 - 1));
                            break;
                        case Direction.DROITE:
                            agent.tirMonstre(new Tuple<int,int>(pos.Item1, pos.Item2 + 1));
                            break;
                    }
                }

                if (action.Equals(Action.SE_DEPLACER))
                {
                    switch (direction)
                    {
                        case Direction.HAUT:
                            agent.setPosition(new Tuple<int,int>(pos.Item1 - 1, pos.Item2));
                            break;
                    case Direction.BAS:
                            agent.setPosition(new Tuple<int,int>(pos.Item1 + 1, pos.Item2));
                            break;
                    case Direction.GAUCHE:
                            agent.setPosition(new Tuple<int,int>(pos.Item1, pos.Item2 - 1));
                            break;
                    case Direction.DROITE:
                            agent.setPosition(new Tuple<int,int>(pos.Item1, pos.Item2 + 1));
                            break;
                    }
                }
            }

        }
    }
}
