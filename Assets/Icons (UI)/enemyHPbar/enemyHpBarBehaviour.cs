using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyHpBarBehaviour : MonoBehaviour
{
    public Slider slider;
    public Color low;
    public Color high;
    public Vector3 Offset;


    public void HealthStatus(float currentHealth, float maxHealth)
    {
        slider.gameObject.SetActive(currentHealth < maxHealth);
        slider.value = currentHealth;
        slider.maxValue = maxHealth;

        slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, slider.normalizedValue);
    }

    void Update()
    {
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + Offset);
    }
}
