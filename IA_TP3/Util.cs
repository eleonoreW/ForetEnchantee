using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA_TP3
{
    public enum Element { Nothing, Start, Monster, Trap, Portal }
    public enum Impact { Smell, Wind, Light }
    public enum Action
    {
        SE_DEPLACER,
        LANCER_PIERRE,
        PRENDRE_PORTAIL,
        MOURIR
    }
    public enum Direction {
    HAUT,
    BAS,
    GAUCHE,
    DROITE,
        NULL
    }
    public enum ElementCell {
    VIDE,
    PORTAIL,
    MONSTRE,
    CREVASSE,
    CACA,
    VENT,
    CACA_VENT,
    NULL
    }

    public enum ConnaissanceCell { SANS_RISQUE, MONSTRE, CREVASSE,PORTAIL }
    public enum Faits {
    Cell_BUT_ATTEINTE,
    FRONTIERE_CONTIENT_INCONNU_SUR,
    FRONTIERE_CONTIENT_MENACE_MONSTRE,
    FRONTIERE_CONTIENT_MENACE_INCONNUE,
    FRONTIERE_CONTIENT_MENACE_CREVASSE,
    PORTAIL_PRESENT,
    NOUVEAUX_VOISINS_OBSERVE,
    RISQUE_DE_MONSTRE
    }

}
