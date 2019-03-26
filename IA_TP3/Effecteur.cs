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
            env.informe(action, direction); // On connait pos en demandant directement à l'agent

            if (agent.IsLive())
            {
                if (action.Equals(Action.LAUNCH_ROCK))
                {
                    switch (direction)
                    {
                        case Direction.UP:
                            agent.tirMONSTER(new Tuple<int,int>(pos.Item1 - 1, pos.Item2));
                            break;
                        case Direction.DOWN:
                            agent.tirMONSTER(new Tuple<int,int>(pos.Item1 + 1, pos.Item2));
                            break;
                        case Direction.LEFT:
                            agent.tirMONSTER(new Tuple<int,int>(pos.Item1, pos.Item2 - 1));
                            break;
                        case Direction.RIGHT:
                            agent.tirMONSTER(new Tuple<int,int>(pos.Item1, pos.Item2 + 1));
                            break;
                    }
                }

                if (action.Equals(Action.MOVE))
                {
                    switch (direction)
                    {
                        case Direction.UP:
                            agent.setPosition(new Tuple<int,int>(pos.Item1 - 1, pos.Item2));
                            break;
                    case Direction.DOWN:
                            agent.setPosition(new Tuple<int,int>(pos.Item1 + 1, pos.Item2));
                            break;
                    case Direction.LEFT:
                            agent.setPosition(new Tuple<int,int>(pos.Item1, pos.Item2 - 1));
                            break;
                    case Direction.RIGHT:
                            agent.setPosition(new Tuple<int,int>(pos.Item1, pos.Item2 + 1));
                            break;
                    }
                }
            }

        }
    }
}
