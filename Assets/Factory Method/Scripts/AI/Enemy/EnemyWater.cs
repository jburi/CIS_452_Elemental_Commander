using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWater : AI
{
    public EnemyWater()
    {
        this.AIType = "Water";
        this.Allegiance = AllegianceType.ENEMY;
        this.element = "Water";
        this.theEnemy = "Player";
    }
}
