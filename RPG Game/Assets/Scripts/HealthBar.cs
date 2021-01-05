using System;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private HealthSystem healthSystem;

    public void Setup(HealthSystem healthSystem) 
    {
        this.healthSystem = healthSystem;

        healthSystem.OnHealthChange += OnHealthchange;
    }

    /*Evento*/
    private void OnHealthchange(object sender, EventArgs e)
    {
        gameObject.transform.Find("Bar").localScale = new Vector3(healthSystem.GetHealthPercent(), 1);
    }
}
