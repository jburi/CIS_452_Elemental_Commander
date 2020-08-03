/*
* Jacob Buri
* AllyGrass.cs
* Assignment 6 - Factory Method
* Ally with grass element type
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyGrass : AI
{
    public AllyGrass()
    {
        this.AIType = "Grass";
        this.Allegiance = AllegianceType.ALLY;
        this.element = "Grass";
        this.theEnemy = "EnemyUnit";
    }
}
