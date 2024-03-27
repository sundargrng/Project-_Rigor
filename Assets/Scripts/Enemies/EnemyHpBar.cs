using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    private EnemyHealthManager eHealthMan;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider eraseHealthSlider;

    private float lerpSpeed = 0.05f;
    private float currentLerpValue;

    // Start is called before the first frame update
    void Start()
    {
        eHealthMan = GetComponentInParent<EnemyHealthManager>();
        healthSlider.maxValue = eHealthMan.maxhealth;
        eraseHealthSlider.maxValue = eHealthMan.maxhealth;
    }


    // Update is called once per frame
    void Update()
    {
        if (healthSlider.value != eHealthMan.currentHealth)
        {
            healthSlider.value = eHealthMan.currentHealth;
        }

        if (healthSlider.maxValue != eHealthMan.maxhealth)
        {
            healthSlider.maxValue = eHealthMan.maxhealth;
            eraseHealthSlider.maxValue = eHealthMan.maxhealth;
        }

        if (healthSlider.value != eraseHealthSlider.value)
        {
            currentLerpValue = Mathf.Lerp(eraseHealthSlider.value, eHealthMan.currentHealth, lerpSpeed);
            eraseHealthSlider.value = currentLerpValue;
        }
    }
}
