/*
* Jacob Buri
* AllyFire.cs
* Assignment 6 - Factory Method
* Ally with fire element type
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyFire : AI
{
    public AllyFire()
    {
        this.AIType = "Fire";
        this.Allegiance = AllegianceType.ALLY;
        this.element = "Fire";
        this.theEnemy = "EnemyUnit";
    }

    
}
