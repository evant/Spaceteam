using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HoldingHealth : MonoBehaviour
{
    public float health;
    float maxHealth;
    public RawImage bar;
    float barWidth;

    void Start ()
    {
        maxHealth = health;
        barWidth = bar.rectTransform.sizeDelta.x;
    }
	
	void Update ()
    {
        health = maxHealth;
        foreach (var problem in FindObjectsOfType<Problem>())
        {
            health -= problem.damage;
        }
        if (health <= 0)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("GameOver");
        }
        else if (health <= 30)
        {
            FindObjectOfType<ShakeBehavior>().TriggerShake();
        }
        HealthBar();
    }

    public void HealthBar()
    {
        float percentHealth = health / maxHealth;   
        float healthLeft = barWidth * percentHealth;
        RectTransform innerBar = bar.GetComponent<RectTransform>();
        innerBar.sizeDelta = new Vector2(healthLeft, bar.rectTransform.sizeDelta.y);
        innerBar.anchoredPosition = new Vector2(healthLeft/2 +6 , innerBar.anchoredPosition.y);

    }
}
