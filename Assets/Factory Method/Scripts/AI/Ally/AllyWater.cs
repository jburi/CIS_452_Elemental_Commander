using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyWater : AI
{
    public AllyWater()
    {
        this.AIType = "Water";
        this.Allegiance = AllegianceType.ALLY;
        this.element = "Water";
        this.theEnemy = "EnemyUnit";
    }
}
