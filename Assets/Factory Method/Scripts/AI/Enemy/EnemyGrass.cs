using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrass : AI
{
    public EnemyGrass()
    {
        this.AIType = "Grass";
        this.Allegiance = AllegianceType.ENEMY;
        this.element = "Grass";
        this.theEnemy = "Player";
    }
}
