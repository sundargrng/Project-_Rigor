using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider hpBar;


    public void UpdateHpBar(float currentValue, float maxValue)
    {
        hpBar.value = currentValue / maxValue;
    }

}
