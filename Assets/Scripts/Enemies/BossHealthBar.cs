using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    private float health;
    private float lerpTimer;
    public float maxHealth = 500f;
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image BackHealthBar;

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        Debug.Log(health);
    }
}
