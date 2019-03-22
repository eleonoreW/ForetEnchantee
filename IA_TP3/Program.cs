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
            Agent agent = new Agent();
            int forestSize = 3;
			while(true) {
				Generate(forestSize);
                agent.ProchaineAction();
                if(Forest.getCell(Agent.posI,Agent.posJ).GetElement() == Element.Portal)
                {
                    forestSize++;
                    Generate(forestSize);
                    agent = new Agent();

                } else if (Forest.getCell(Agent.posI, Agent.posJ).IsElement(Element.Monster) || Forest.getCell(Agent.posI, Agent.posJ).IsElement(Element.Trap))
                {
                    Generate(forestSize);
                    agent = new Agent();
                }
                Console.ReadKey();
				Console.Clear();
			}
		}

		static void Generate(int size)
		{
			Forest f = new Forest(size);
			f.Log();
		}
	}
}
