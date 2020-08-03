/*
* Jacob Buri
* HealthBar.cs
* Assignment 6 - Factory Method
* Used to display the current health as a slider
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

	public Slider slider;
	public Image fill;

	private void Awake()
	{
		slider = gameObject.GetComponent<Slider>();
	}

	public void SetMaxHealth(int health)
	{
		slider.maxValue = health;
		slider.value = health;
	}

    public void SetHealth(int health)
	{
		slider.value = health;
	}

}
