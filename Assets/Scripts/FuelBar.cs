using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelBar : MonoBehaviour
{
    public Image bar;
    public float fill;
    RocketEngine fuel;

    private void Start()
    {
        fill = 1f;
        fuel = GetComponent<RocketEngine>();
    }

    private void Update()
    {
        bar.fillAmount = fill;
        fill = fuel.fuel/100;
    }
}
