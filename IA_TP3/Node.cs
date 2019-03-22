namespace IA_TP3
{
	abstract class Node
	{
		public Node parent;
		public int[,] map;
		public int agentPosI;
		public int agentPosJ;
		public Action action;
		public int cost;
		public int depth;

		public Node(Node parent_, int[,] map_, int agentPosI_, int agentPosJ_, Action action_)
		{
			parent = parent_;
			map = map_;
			agentPosI = agentPosI_;
			agentPosJ = agentPosJ_;
			action = action_;

			// Si c'est un noeud racine
			if (parent == null)
			{
				cost = 0;
				depth = 0;
			}
			// Sinon
			else
			{
				depth = parent.depth + 1;
				cost = parent.cost + 1;
				if (action == Action.ASPIRER)
				{
					cost -= Rules.GainAspirer(map[agentPosI, agentPosJ]);

					// On enlève la poussière et le bijou de la carte
					map[agentPosI, agentPosJ] = 0;
				}
				else if (action == Action.RAMASSER)
				{
					cost -= Rules.GainRamasser(map[agentPosI, agentPosJ]);

					// On enlève la poussière et le bijou de la carte
					map[agentPosI, agentPosJ] = 0;
				}
			}
		}

		public bool NodeWithPoussiere(int i, int j)
		{
			return (map[i, j] == 1 || map[i, j] == 3);
		}

		public bool NodeWithBijou(int i, int j)
		{
			return (map[i, j] == 2 || map[i, j] == 3);
		}

		public abstract int Eval(); // Valeur de comparaison entre deux noeuds
	}
}
