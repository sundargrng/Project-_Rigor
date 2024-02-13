using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private HealthManager healthManager;
    public Slider hpAmount;
    public Text hpText;

    // Start is called before the first frame update
    void Start()
    {
        healthManager = FindObjectOfType<HealthManager>();
    }

    // Update is called once per frame
    void Update()
    {
        hpAmount.maxValue = healthManager.maxHealth;
        hpAmount.value = healthManager.currentHealth;

        hpText.text = "HP " + healthManager.currentHealth + " / " + healthManager.maxHealth;
    }
}
