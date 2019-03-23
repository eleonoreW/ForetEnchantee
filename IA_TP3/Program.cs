using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA_TP3
{
	class Program
	{
		static void Main(string[] args)
		{
            Environement env = new Environement();
            Agent agent = new Agent(env);
            env.setAgent(agent);
			while(true) {
                agent.agir();
                if (!agent.IsLive())
                {
                    Console.WriteLine("Mort de l'agent");
                }
                agent.etablirFait();
                Tuple<Action, Direction> actionChoisie = agent.getActionChoisie();

                Console.WriteLine("MESURE DE PERFORMENCE : " + env.getMesurePerf());
                Console.WriteLine("Faits de l'agent : " + agent.getFaits());
                if (actionChoisie != null)
                {
                    if (actionChoisie.Item1 == Action.PRENDRE_PORTAIL)
                        Console.WriteLine("Prochaine action choisie : " + actionChoisie.Item1);
                    else
                        Console.WriteLine("Prochaine action choisie : " + actionChoisie.Item1 + " vers " +
                                (actionChoisie.Item2 == Direction.DROITE || actionChoisie.Item2 == Direction.GAUCHE ? "la " : "le ") + actionChoisie.Item2);
                }
                
                agent = env.getAgent();
                Afficher(agent, env);
                Console.ReadKey();
				Console.Clear();
			}
		}

        private static void Afficher(Agent agent, Environement env)
        {
            Console.WriteLine(agent.getPosition().Item1 +" "+ agent.getPosition().Item2);
            string str;
            int size = env.GetForest().GetSize();
            for (int i = 0; i < size; i++)
            {
                Console.WriteLine(new string('-', size * 7 + 1));
                for (int j = 0; j < size; j++)
                {
                    str = "";
                    if (j == 0)
                    {
                        str += "|";
                    }
                    str += env.GetForest().GetCell(new Tuple<int,int>(i, j));

                    if(agent.getPosition().Item1 == i && agent.getPosition().Item2 == j){
                        str += "** agent **";
                    }
                    str += "|";
                    Console.Write(str);
                }
                Console.WriteLine();
            }
            Console.WriteLine(new string('-', size * 7 + 1));
        }
    }
}
