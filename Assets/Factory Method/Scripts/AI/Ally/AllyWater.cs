/*
* Jacob Buri
* AllyWater.cs
* Assignment 6 - Factory Method
* Ally with water element type
*/
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
