using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;

	public Slider slider;
	public Image fill;

	GameManager gm;

	public void SetMaxTime(int health)
	{
		slider.maxValue = health;
		slider.value = health;
		timerText.text = health.ToString();
	}

	public void SetTime(int health)
	{
		slider.value = health;
		timerText.text = health.ToString();
	}


}
