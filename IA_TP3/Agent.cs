using System;
using System.Collections.Generic;

namespace IA_TP3
{
    public class Agent
    {
        private CapteurLumiere oeil;
        private CapteurOdeur nez;
        private CapteurVent peau;
        private bool isLive;

        private Tuple<int, int> position;
        private ElementCell elementPosition;
        private Dictionary<Direction, bool> voisinsPossible;

        private Dictionary<Tuple<int, int>, ConnaissanceCell> croyance;

        private List<Faits> faits;

        // FRONTIERE
        private List<Tuple<int, int>> CellsMonstreSuspect;
        private List<Tuple<int, int>> CellsCrevasseSuspecte;
        private List<Tuple<int, int>> CellsInconnuesSuspecte;
        private List<Tuple<int, int>> CellsInconnuesInnofensives;

        private Tuple<Action, Direction> actionChoisie;
        private Tuple<int, int> CellObjectif;
        private Effecteur Effecteur;

        public Agent(Environement env)
        {
            croyance = new Dictionary<Tuple<int, int>, ConnaissanceCell>(); // la croyance est une carte de la Forest vide
            oeil = new CapteurLumiere(env.GetForest());
            nez = new CapteurOdeur(env.GetForest());
            peau = new CapteurVent(env.GetForest());
            isLive = true;

            Effecteur = new Effecteur(env, this);

            position = new Tuple<int, int>(0, 0); // l'agent se positionne toujours en haut à gauche au départ

            CellsMonstreSuspect = new List<Tuple<int, int>>();
            CellsCrevasseSuspecte = new List<Tuple<int, int>>();
            CellsInconnuesInnofensives = new List<Tuple<int, int>>();
            CellsInconnuesSuspecte = new List<Tuple<int, int>>();

        }

        /***
         * Observe l'Environement, mets à jours ces croyance, en déduis des faits puis une action
         */
        public void etablirFait()
        {
            if (!isLive)
            {
                ressuscite();
            }
            sonderEnvironement();
            actionChoisie = majEtat();
            //actionChoisie = determineAction();
            if (actionChoisie != null)
            {
                Console.WriteLine(actionChoisie.Item1.ToString() + (actionChoisie.Item2 == Direction.NULL ? "" : " : " + actionChoisie.Item2));
                Console.WriteLine("---------------------------------------");
            }
        }

        /**
         * effectue l'action résultant de la fonction établirFait
         */
        public void agir()
        {
            if (actionChoisie != null)
            {
                effectuerActions(actionChoisie.Item1, actionChoisie.Item2);
            }
        }

        /**
         * Appelle tout les capteurs pour avoir des informations sur la position actuelle
         */
        private void sonderEnvironement()
        {
            // Cell vide par defaut
            croyance[new Tuple<int, int>(position.Item1, position.Item2)] =  ConnaissanceCell.SANS_RISQUE;
            elementPosition = ElementCell.VIDE;
            bool menaceMonstre = false;

            if (oeil.sonder(position))
            {
                croyance[new Tuple<int, int>(position.Item1, position.Item2)] = ConnaissanceCell.PORTAIL;
                elementPosition = ElementCell.PORTAIL;
            }
            if (nez.sonder(position))
            {
                elementPosition = ElementCell.CACA;
                menaceMonstre = true;
            }
            if (peau.sonder(position))
            {
                if (menaceMonstre)
                    elementPosition = ElementCell.CACA_VENT;
                else
                    elementPosition = ElementCell.VENT;
            }

            voisinsPossible = oeil.CellsAlentour(position);
        }

        /**
         * Mets à jours les croyances et les faits et determine une action à partir des faits
         * @return Action a réaliser et la direction si necessaire
         */
        private Tuple<Action, Direction> majEtat()
        {

            // actualise les informations sur la position
            if (CellsInconnuesInnofensives.Contains(position))
                CellsInconnuesInnofensives.Remove(position);
            if (CellsMonstreSuspect.Contains(position))
                CellsMonstreSuspect.Remove(position);
            if (CellsCrevasseSuspecte.Contains(position))
                CellsCrevasseSuspecte.Remove(position);
            if (CellsInconnuesSuspecte.Contains(position))
                CellsInconnuesSuspecte.Remove(position);
            // METS A JOUR LES FAIT
            faits = new List<Faits>();
            Dictionary<Direction, Tuple<int, int>> voisins = this.getVoisinsPossibles(position);

            // SI CellObjectif est a null => FAIT Cell objectif atteinte
            if (CellObjectif == null)
                faits.Add(Faits.Cell_BUT_ATTEINTE);

            // SI l'agent est sur la Cell objectif => FAIT Cell objectif atteinte
            if (CellObjectif != null && position.Equals(CellObjectif))
                faits.Add(Faits.Cell_BUT_ATTEINTE);

            // SI l'agent est sur portail => FAIT portail present
            if (elementPosition.Equals(ElementCell.PORTAIL))
                faits.Add(Faits.PORTAIL_PRESENT);

            // METS A JOUR LA FRONTIERE
            // SI Cell atteinte but atteinte && pas de portail ici => mets a jours la frontiere && FAIT nouveau voisins observés
            if (faits.Contains(Faits.Cell_BUT_ATTEINTE) && !faits.Contains(Faits.PORTAIL_PRESENT))
            {
                // Mets a jours les voisins
                foreach (KeyValuePair<Direction, Tuple<int, int>> voisin in voisins)
                {
                    // SI c'est un voisin qui n'a pas encore été visité
                    if (!croyance.ContainsKey(voisin.Value))
                    {
                        // SI on est sur une Cell vide
                        if (elementPosition.Equals(ElementCell.VIDE))
                        {
                            // si pas deja present dans la liste
                            if (!CellsInconnuesInnofensives.Contains(voisin.Value))
                                CellsInconnuesInnofensives.Add(voisin.Value);
                            CellsMonstreSuspect.Remove(voisin.Value);
                            CellsCrevasseSuspecte.Remove(voisin.Value);
                            CellsInconnuesSuspecte.Remove(voisin.Value);
                        }
                        // SI on est sur une Cell caca
                        if (elementPosition.Equals(ElementCell.CACA) && !CellsInconnuesInnofensives.Contains(voisin.Value))
                        {
                            if (!CellsMonstreSuspect.Contains(voisin.Value))
                                CellsMonstreSuspect.Add(voisin.Value);
                            CellsCrevasseSuspecte.Remove(voisin.Value);
                            CellsInconnuesSuspecte.Remove(voisin.Value);
                        }
                        // SI on est sur une Cell inconnue (on ne sait pas si c'est un monstre une crevasse ou rien)
                        if (elementPosition.Equals(ElementCell.VENT) && !CellsInconnuesInnofensives.Contains(voisin.Value))
                        {
                            // si pas deja present dans la liste
                            if (!CellsCrevasseSuspecte.Contains(voisin.Value))
                                CellsCrevasseSuspecte.Add(voisin.Value);
                            CellsMonstreSuspect.Remove(voisin.Value);
                            CellsInconnuesSuspecte.Remove(voisin.Value);
                        }
                        // SI on est sur une Cell vent
                        if (elementPosition.Equals(ElementCell.CACA_VENT) && !CellsInconnuesInnofensives.Contains(voisin.Value))
                        {
                            if (!CellsCrevasseSuspecte.Contains(voisin.Value) && !CellsMonstreSuspect.Contains(voisin.Value) && !CellsInconnuesSuspecte.Contains(voisin.Value))
                                CellsInconnuesSuspecte.Add(voisin.Value);
                        }
                    }
                }
                faits.Add(Faits.NOUVEAUX_VOISINS_OBSERVE);
            }

            // DEDUIS FAITS

            // SI la friontière contient au moins une Cell inconnue et sans risque => FAIT frontiere contient inconnu sans risque
            if (!(CellsInconnuesInnofensives.Count == 0) && faits.Contains(Faits.NOUVEAUX_VOISINS_OBSERVE) &&
                    faits.Contains(Faits.NOUVEAUX_VOISINS_OBSERVE))
                faits.Add(Faits.FRONTIERE_CONTIENT_INCONNU_SUR);

            // SI la frontière ne contient pas d'inconnus sans risques ET contient au moins une Cell inconnue suspecte d'avoir un monstre => FAIT frontiere contient menace monstre
            if (!faits.Contains(Faits.FRONTIERE_CONTIENT_INCONNU_SUR) && !(CellsMonstreSuspect.Count == 0) &&
                    faits.Contains(Faits.NOUVEAUX_VOISINS_OBSERVE))
                faits.Add(Faits.FRONTIERE_CONTIENT_MENACE_MONSTRE);

            // SI la frontière ne contient pas d'inconnus sans risques ET pas de menace monstre ET contient au moins une Cell dont la menace est inconnue (monstre ou crevasse) => FAIT frontiere contient menace inconnue
            if (!faits.Contains(Faits.FRONTIERE_CONTIENT_INCONNU_SUR) && !faits.Contains(Faits.FRONTIERE_CONTIENT_MENACE_MONSTRE) &&
                    !(CellsInconnuesSuspecte.Count == 0)
                    && faits.Contains(Faits.NOUVEAUX_VOISINS_OBSERVE))
                faits.Add(Faits.FRONTIERE_CONTIENT_MENACE_INCONNUE);

            // SI la frontière ne contient pas d'inconnus sans risques ET pas de menace monstre ET pas de menace inconnue ET contient au moins une Cell inconnue suspecte d'avoir une crevasse => FAIT frontiere contient menace crevasse
            if (!faits.Contains(Faits.FRONTIERE_CONTIENT_INCONNU_SUR) && !faits.Contains(Faits.FRONTIERE_CONTIENT_MENACE_MONSTRE) &&
                    !faits.Contains(Faits.FRONTIERE_CONTIENT_MENACE_INCONNUE) &&
                    !(CellsCrevasseSuspecte.Count == 0) &&
                    faits.Contains(Faits.NOUVEAUX_VOISINS_OBSERVE))
                faits.Add(Faits.FRONTIERE_CONTIENT_MENACE_CREVASSE);

            // SI on a atteint la Cell visée ET on a mis a jour la frontiere ET il n'y a pas de portail ici && la frontiere a une Cell sans risque => vise la Cell inconnue sans risque la plus proche de la position de l'agent
            if (faits.Contains(Faits.Cell_BUT_ATTEINTE) && faits.Contains(Faits.NOUVEAUX_VOISINS_OBSERVE) && !faits.Contains(Faits.PORTAIL_PRESENT) &&
                    faits.Contains(Faits.FRONTIERE_CONTIENT_INCONNU_SUR))
            {
                // parmis liste inconnue inoffensif prendre le + proche ( distance de manhantan)
                CellObjectif = chercheCellProche(position, CellsInconnuesInnofensives);
            }

            // SI on a atteint la Cell visée ET on a mis a jour la frontiere ET il n'y a pas de portail ici && la frontiere a une Cell menace monstre => vise la Cell menace monstre la plus proche de la position de l'agent
            if (faits.Contains(Faits.Cell_BUT_ATTEINTE) && faits.Contains(Faits.NOUVEAUX_VOISINS_OBSERVE) && !faits.Contains(Faits.PORTAIL_PRESENT) &&
                    faits.Contains(Faits.FRONTIERE_CONTIENT_MENACE_MONSTRE))
            {
                CellObjectif = chercheCellProche(position, CellsMonstreSuspect);
            }

            // SI on a atteint la Cell visée ET on a mis a jour la frontiere ET il n'y a pas de portail ici && la frontiere a une Cell menace inconnue => vise la Cell menace inconnue la plus proche de la position de l'agent
            if (faits.Contains(Faits.Cell_BUT_ATTEINTE) && faits.Contains(Faits.NOUVEAUX_VOISINS_OBSERVE) &&
                    !faits.Contains(Faits.PORTAIL_PRESENT) && faits.Contains(Faits.FRONTIERE_CONTIENT_MENACE_INCONNUE))
            {
                // parmis liste inconnue inoffensif prendre le + proche ( distance de manhantan)
                CellObjectif = chercheCellProche(position, CellsInconnuesSuspecte);
            }

            // SI on a atteint la Cell visée ET on a mis a jour la frontiere ET il n'y a pas de portail ici && la frontiere a une Cell menace crevasse => vise la Cell menace crevasse la plus proche de la position de l'agent
            if (faits.Contains(Faits.Cell_BUT_ATTEINTE) && faits.Contains(Faits.NOUVEAUX_VOISINS_OBSERVE) &&
                    !faits.Contains(Faits.PORTAIL_PRESENT) && faits.Contains(Faits.FRONTIERE_CONTIENT_MENACE_CREVASSE))
            {
                // parmis liste inconnue inoffensif prendre le + proche ( distance de manhantan)
                CellObjectif = chercheCellProche(position, CellsCrevasseSuspecte);
            }

            // SI on est voisin avec la Cell objectif et que la menace est soit inconnue soit une menace de monstre => FAIT risque de monstre
            if (voisins.ContainsValue(CellObjectif) && (CellsMonstreSuspect.Contains(CellObjectif) || CellsInconnuesSuspecte.Contains(CellObjectif)))
                faits.Add(Faits.RISQUE_DE_MONSTRE);

            // SI risque de monstre => LANCE PIERRE vers la Cell visée
            if (faits.Contains(Faits.RISQUE_DE_MONSTRE))
            {
                Direction dirObjectif = Direction.NULL;
                foreach (KeyValuePair<Direction, Tuple<int, int>> voisin in voisins)
                    if (voisin.Value.Equals(CellObjectif))
                    {
                        dirObjectif = voisin.Key;
                    }
                return new Tuple<Action, Direction>(Action.LANCER_PIERRE, dirObjectif);
            }


            // SI portail present => prends le portail
            if (faits.Contains(Faits.PORTAIL_PRESENT))
                return new Tuple<Action, Direction>(Action.PRENDRE_PORTAIL, Direction.NULL);

            // SI pas de Portail ET pas de risque de monstre => DEPLACEMENT vers la Cell nous rapprochant le plus de la Cell objectif
            if (!faits.Contains(Faits.PORTAIL_PRESENT) && !faits.Contains(Faits.RISQUE_DE_MONSTRE))
            {
                // action = deplacement vers la Cell connue qui rapproche le plus
                List<Direction> directionsSelectionne = new List<Direction>();
                int distVoisinSelectionne = 1000;
                foreach (KeyValuePair<Direction, Tuple<int, int>> voisin in voisins)
                {
                    // si c'est un voisin deja connu sans risque ou la Cell objectif
                    if ((croyance.ContainsKey(voisin.Value) && croyance[voisin.Value].Equals(ConnaissanceCell.SANS_RISQUE)) ||
                       voisin.Value.Equals(CellObjectif))
                        // prends celui qui se rapproche le plus de l'objectif
                        if (distVoisinSelectionne > DistManhattan(voisin.Value, CellObjectif))
                        {
                            directionsSelectionne = new List<Direction>();
                            directionsSelectionne.Add(voisin.Key);
                            distVoisinSelectionne = DistManhattan(voisin.Value, CellObjectif);
                        }
                        else if (distVoisinSelectionne == DistManhattan(voisin.Value, CellObjectif))
                            directionsSelectionne.Add(voisin.Key);
                }

                // si plusieurs voisins sans a distance égale, en sélectionne un aléatoirement pour éviter les boucles
                Direction directionSelectionne = Direction.NULL;
                if (!(directionsSelectionne.Count == 0))
                {
                    Random rand = new Random();
                    int randomIndice = rand.Next(directionsSelectionne.Count);
                    directionSelectionne = directionsSelectionne[randomIndice];
                }
                else
                    Console.WriteLine("IL N'Y A AUCUN ECHAPATOIRE !!!!");

                return new Tuple<Action, Direction>(Action.SE_DEPLACER, directionSelectionne);
            }

            return null;
        }

        private int DistManhattan(Tuple<int, int> CellDepart, Tuple<int, int> CellObjectif)
        {
            return (Math.Abs(CellDepart.Item1 - CellObjectif.Item1) + Math.Abs(CellDepart.Item2 - CellObjectif.Item2));
        }

        private Tuple<int, int> chercheCellProche(Tuple<int, int> position, List<Tuple<int, int>> listeCellsFrontiere)
        {
            Tuple<int, int> CellForestSelectionne = null;
            int distCellForestSelectionne = 1000;
            foreach (Tuple<int, int> CellForest in listeCellsFrontiere)
            {
                // prends celui qui se rapproche le plus de l'objectif
                if (CellForestSelectionne == null || distCellForestSelectionne > DistManhattan(position, CellForest))
                {
                    CellForestSelectionne = CellForest;
                    distCellForestSelectionne = DistManhattan(position, CellForest);
                }
            }

            return CellForestSelectionne;
        }



        /**
         * Transmet l'action a l'Effecteur (et la direction si necessaire)
         * @param action a effectuer
         * @param dir direction dans laquelle effectuer l'action
         */
        private void effectuerActions(Action action, Direction dir)
        {
            Effecteur.Fait(position, action, dir);
        }

        /**
         * retourne la liste des voisins autour de l'agent
         * @param position : position de l'agent
         * @return Dictionary contenant la direction vers laquelle se trouve la voisin et les coordonnées de celui ci.
         */
        private Dictionary<Direction, Tuple<int, int>> getVoisinsPossibles(Tuple<int, int> position)
        {
            Dictionary<Direction, Tuple<int, int>> voisins = new Dictionary<Direction, Tuple<int, int>>();

            if (voisinsPossible[Direction.HAUT])    // si possede un voisin en haut
                voisins.Add(Direction.HAUT, new Tuple<int, int>(position.Item1 - 1, position.Item2));
            if (voisinsPossible[Direction.BAS])    // si possede un voisin en bas
                voisins.Add(Direction.BAS, new Tuple<int, int>(position.Item1 + 1, position.Item2));
            if (voisinsPossible[Direction.GAUCHE])    // si possede un voisin en haut
                voisins.Add(Direction.GAUCHE, new Tuple<int, int>(position.Item1, position.Item2 - 1));
            if (voisinsPossible[Direction.DROITE])    // si possede un voisin en haut
                voisins.Add(Direction.DROITE, new Tuple<int, int>(position.Item1, position.Item2 + 1));

            return voisins;
        }

        /**
         * mets a jours les croyances en cas de tir vers une Cell que l'agent suppose contenir un monstre
         * @param CellVisee : Cell vers laquelle un tire
         */
        public void tirMonstre(Tuple<int, int> CellVisee)
        {

            CellsMonstreSuspect.Remove(CellVisee);
            if (CellsInconnuesSuspecte.Contains(CellVisee))
            {
                // on sait qu'elle ne peut plus avoir de montre, donc la seule menace restante est celle de la crevasse
                CellsInconnuesSuspecte.Remove(CellVisee);
                CellsCrevasseSuspecte.Add(CellVisee);
                CellVisee = null;
            }
            else
                // indique que la zone a été pacifiée et quelle est sans risque
                CellsInconnuesInnofensives.Add(CellVisee);
            CellsMonstreSuspect.Remove(CellVisee);

        }

        /**
         * mets à jour les croyances en cas de mort de l'agent
         * @param pos : position à laquelle il est mort
         * @param causeMort : cause de la mort (crevasse ou monstre)
         */
        public void meurt(Tuple<int, int> pos, ElementCell causeMort)
        {
            // mets a jour les connaissances
            croyance.Add(pos, (causeMort.Equals(ElementCell.MONSTRE) ? ConnaissanceCell.MONSTRE : ConnaissanceCell.CREVASSE));
            CellsInconnuesInnofensives.Remove(pos);
            CellsCrevasseSuspecte.Remove(pos);
            CellsMonstreSuspect.Remove(pos);
            CellsInconnuesSuspecte.Remove(pos);
            // reinitialise la Cell objectif
            CellObjectif = null;
            isLive = false;
        }

        /**
         * mets a jour la position de l'agent
         * @param position : position a affecter à l'agent
         */
        public void setPosition(Tuple<int, int> position)
        {
            this.position = position;
        }

        /**
         * retourne la position de l'agent
         * @return position de l'agent
         */
        public Tuple<int, int> getPosition()
        {
            return position;
        }


        /**
         * indique si l'agent est vivant
         * @return vrai si vivant, faux sinon
         */
        public bool IsLive()
        {
            return isLive;
        }

        /**
         * indique que l'agent revit
         */
        public void ressuscite()
        {
            isLive = true;
        }

        /**
         * retourne la liste des faits établis par l'agent
         * @return liste des faits
         */
        public List<Faits> getFaits()
        {
            return faits;
        }

        /**
         * retourne l'action choisie et sa direction si necessaire
         * @return
         */
        public Tuple<Action, Direction> getActionChoisie()
        {
            return actionChoisie;
        }
    }

}

