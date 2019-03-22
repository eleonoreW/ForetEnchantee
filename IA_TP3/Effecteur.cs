namespace IA_TP3
{
    class Effecteur
    {
        public void faire(Action a, int i, int j)
        {
			// Notifier l'environnement de l'action effectuée - action a la position i,j
		    Forest.ProcessAction(a, i, j);
            // Modifier l'état interne du robot (son score et ses croyances)
            switch (a)
            {
                case Action.HAUT:
                    Agent.perf--;
                    if (j > 0)
                        Agent.posJ--;
                    break;
                case Action.BAS:
                    Agent.perf--;
                    if (j < Rules.height - 1)
                        Agent.posJ++;
                    break;
                case Action.GAUCHE:
                    Agent.perf--;
                    if (i > 0)
                        Agent.posI--;
                    break;
                case Action.DROITE:
                    Agent.perf--;
                    if (i < Rules.width - 1)
                        Agent.posI++;
                    break;
                case Action.ASPIRER:
                    Agent.perf--;
                    Agent.perf += Rules.GainAspirer(Agent.croyance[i, j]);
                    Agent.croyance[i, j] = 0;
                    break;
                case Action.RAMASSER:
                    Agent.perf--;
                    Agent.perf += Rules.GainRamasser(Agent.croyance[i, j]);
                    Agent.croyance[i, j] = 0;
                    break;
                default:
                    break;
            }
        }
    }
}
