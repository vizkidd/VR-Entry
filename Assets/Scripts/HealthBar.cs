using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    public GameObject asteroidGO;
    Asteroid asteroid;
    float fullHealth;
    Image healthBar;
	// Use this for initialization
	void Awake () {
        asteroid = asteroidGO.GetComponent<Asteroid>();
        healthBar = GetComponent<Image>();
        asteroid.HealthSet.AddListener(SetFullHealth);
	}
	void SetFullHealth()
    {
        fullHealth = asteroid.health;
    }
	// Update is called once per frame
	void Update () {
        
        healthBar.fillAmount = (asteroid.health / fullHealth);
        
        if(healthBar.fillAmount>0.6f)
        {
            healthBar.color = new Color32(178, 255, 0, 255);
        }
        else if (healthBar.fillAmount > 0.35f)
        {
            healthBar.color = new Color32(255, 185, 0, 255);
        }
        else
        {
            healthBar.color = new Color32(255, 60, 0, 255);
        }
	}
}
