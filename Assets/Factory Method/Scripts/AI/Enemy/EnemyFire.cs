using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : AI
{
    public EnemyFire()
    {
        this.AIType = "Fire";
        this.Allegiance = AllegianceType.ENEMY;
        this.element = "Fire";
        this.theEnemy = "Player";
    }
}
