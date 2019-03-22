using System;

namespace IA_TP3
{
	class Forest
	{
		readonly int size;
		static Cell[,] cells;

		readonly double monsterSpawnPercent = 0.1;
		readonly double trapSpawnPercent = 0.1;


		public Forest(int size_)
		{
			size = size_;
			cells = new Cell[size_, size_];

			DeclareForest();

			Random r = new Random();
			PlacePortal(r);
			PlaceMonsters(r);
			PlaceTraps(r);

			UpdateCells();
		}

		private void DeclareForest()
		{
			for (int i = 0; i < size; i++) {
				for (int j = 0; j < size; j++) {
					cells[i, j] = new Cell(i, j);
				}
			}
			cells[0, 0].SetElement(Element.Start);
		}

		private void PlacePortal(Random r)
		{
			int portalI;
			int portalJ;

			do {
				portalI = r.Next(size);
				portalJ = r.Next(size);
			} while (!cells[portalI,portalJ].IsElement(Element.Nothing));

			cells[portalI, portalJ].SetElement(Element.Portal);
		}

		private void PlaceMonsters(Random r)
		{
			for (int i = 0; i < size; i++) {
				for (int j = 0; j < size; j++) {
					Cell current = cells[i, j];
					if (r.NextDouble() < monsterSpawnPercent && current.IsElement(Element.Nothing)) {
						current.SetElement(Element.Monster);
					}
				}
			}
		}

		private void PlaceTraps(Random r)
		{
			for (int i = 0; i < size; i++) {
				for (int j = 0; j < size; j++) {
					Cell current = cells[i, j];
					if (r.NextDouble() < trapSpawnPercent && current.IsElement(Element.Nothing)) {
						current.SetElement(Element.Trap);
					}
				}
			}
		}

		private void UpdateCells()
		{
            ClearImpacts();
			for (int i = 0; i < size; i++) {
				for (int j = 0; j < size; j++) {
					switch (cells[i, j].GetElement()) {
						case Element.Monster:
							AddImpactToNeighbors(i, j, Impact.Smell);
							break;
						case Element.Trap:
							AddImpactToNeighbors(i, j, Impact.Wind);
							break;
                        case Element.Portal:
                            cells[i,j].AddImpact(Impact.Light);
                            break;
                    }
				}
			}
		}

        private void ClearImpacts()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    getCell(i, j).GetImpacts().Clear();
                }
            }
        }

        private void AddImpactToNeighbors(int posI, int posJ, Impact impact)
		{
			if (posI > 0) {
				cells[posI - 1, posJ].AddImpact(impact);
			}
			if (posI < size - 1) {
				cells[posI + 1, posJ].AddImpact(impact);
			}
			if (posJ > 0) {
				cells[posI, posJ - 1].AddImpact(impact);
			}
			if (posJ < size - 1) {
				cells[posI, posJ + 1].AddImpact(impact);
			}
		}

		public void Log()
		{
			for (int i = 0; i < size; i++) {
				for (int j = 0; j < size; j++) {
					Console.Write(cells[i, j].ToString() + "   ");
				}
				Console.WriteLine();
			}
		}

        public static Cell getCell(int i, int j)
        {
           
            return cells[i, j];
        }

        public ProcessAction()
        {

        }

	}
}
