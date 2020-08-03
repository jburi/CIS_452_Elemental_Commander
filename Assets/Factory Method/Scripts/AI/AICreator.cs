/*
* Jacob Buri
* AICreator.cs
* Assignment 6 - Factory Method
* Abstract class for Ally and Enemy Factories
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AICreator
{
    public abstract GameObject CreateAIPrefab(string type);

}
