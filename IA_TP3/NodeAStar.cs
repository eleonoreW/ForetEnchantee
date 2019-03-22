using System;
using System.Collections.Generic;


namespace IA_TP3
{
	class NodeAStar : Node
	{
		public int costH; // Cout associe a l'heuristique

		public NodeAStar(Node p, int[,] m, int i, int j, Action a) : base(p, m, i, j, a)
		{
			costH = Heuristic();
		}

		public static List<Node> AStarSearch(Node n)
		{
			// Récupérer tous les neouds réalisables a partir de l'etat actuel
			List<Node> neighbors = new List<Node>();

			// Haut
			if (n.agentPosJ > 0)
			{
				Node newNode = new NodeAStar(n, n.map, n.agentPosI, n.agentPosJ - 1, Action.HAUT);
				neighbors.Add(newNode);
			}

			// Bas
			if (n.agentPosJ < Rules.height - 1)
			{
				Node newNode = new NodeAStar(n, n.map, n.agentPosI, n.agentPosJ + 1, Action.BAS);
				neighbors.Add(newNode);
			}

			// Gauche
			if (n.agentPosI > 0)
			{
				Node newNode = new NodeAStar(n, n.map, n.agentPosI - 1, n.agentPosJ, Action.GAUCHE);
				neighbors.Add(newNode);
			}

			// Droite
			if (n.agentPosI < Rules.width - 1)
			{
				Node newNode = new NodeAStar(n, n.map, n.agentPosI + 1, n.agentPosJ, Action.DROITE);
				neighbors.Add(newNode);
			}

			// Aspirer
			if (n.NodeWithPoussiere(n.agentPosI, n.agentPosJ))
			{
				Node newNode = new NodeAStar(n, n.map, n.agentPosI, n.agentPosJ, Action.ASPIRER);
				neighbors.Add(newNode);
			}

			// Ramasser
			if (n.NodeWithBijou(n.agentPosI, n.agentPosJ))
			{
				Node newNode = new NodeAStar(n, n.map, n.agentPosI, n.agentPosJ, Action.RAMASSER);
				neighbors.Add(newNode);
			}

			return neighbors;
		}

		private int Heuristic()
		{
			// Si on est déjà sur une case solution
			if (NodeWithPoussiere(agentPosI, agentPosJ) || NodeWithBijou(agentPosI, agentPosJ))
				return 0;

			// Chercher la case avec poussiere ou bijou dans des cases de plus en plus éloignées
			for (int k = 1; k <= 10; k++)
				for (int j = -1; j <= 1; j++)
					for (int i = -1; i <= 1; i++)
					{
						int newI = agentPosI + i;
						int newJ = agentPosJ + j;

						// Si on est dans les limites du tableau
						if (newI >= 0 && newI < Rules.width && newJ >= 0 && newJ < Rules.height)
							// Si on en trouve une, on retourne la distance de Manhattan entre la position de l'agent et cette case
							if (NodeWithPoussiere(newI, newJ) || NodeWithBijou(newI, newJ))
								return Dist(agentPosI, agentPosJ, newI, newJ);

					}

			// On en a pas trouve, on retourne l'infini
			return 100000;
		}

		private int Dist(int x1, int y1, int x2, int y2)
		{
			// Distance de Manhattan entre deux points
			return (Math.Abs(x1 - x2) + Math.Abs(y1 - y2));
		}

		override public int Eval()
		{
			return cost + costH;
		}
	}
}
