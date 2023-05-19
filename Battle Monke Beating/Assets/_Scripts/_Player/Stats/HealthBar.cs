using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Image fillBar;
    public Gradient gradient;
    public void SetMaxHealth(float maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;

        fillBar.color = gradient.Evaluate(1f);
    }
    public void SetCurrentHealth(float currentHealth)
    {
        slider.value = currentHealth;

        // change the healthbar color when health is updated
        fillBar.color = gradient.Evaluate(slider.normalizedValue);
    }
}
