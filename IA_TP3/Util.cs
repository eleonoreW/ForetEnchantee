namespace IA_TP3
{
    public enum Action
    {
        MOVE,
        LAUNCH_ROCK,
        TAKE_PORTAL,
        DIE
    }
    public enum Direction {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        NULL
    }
    public enum ElementCell {
        EMPTY,
        PORTAL,
        MONSTER,
        TRAP,
        SMELL,
        WIND,
        SMELL_WIND,
        NULL
    }
    public enum BeliefCell {
        NO_RISK,
        MONSTER,
        TRAP,
        PORTAL }
    public enum Faits {
        GOAL_CELL_REACHED,
        FRONTIERE_CONTIENT_INCONNU_SUR,
        FRONTIERE_CONTIENT_MENACE_MONSTER,
        FRONTIERE_CONTIENT_MENACE_INCONNUE,
        FRONTIERE_CONTIENT_MENACE_TRAP,
        PORTAL_PRESENT,
        NOUVEAUX_VOISINS_OBSERVE,
        RISQUE_DE_MONSTER
    }

}
