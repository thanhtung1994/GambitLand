using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextInfoController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextDamageController textDamage;

    [SerializeField] private TextMeshProUGUI textDamageHealthBar;

    private void OnEnable()
    {
        text.transform.parent.gameObject.SetActive(false);
    }

    public void SetText(string textInfo)
    {
        if (textInfo != "")
        {
            text.transform.parent.gameObject.SetActive(true);
            text.text = textInfo;
        }
        else
        {
            text.transform.parent.gameObject.SetActive(false);
        }
    }

    public void SetTextDamage(int damage)
    {
        textDamage.SetText(damage);
    }

    public void SetTextDamageHealthBar(int currentHealth,int maxHealth)
    {
        textDamageHealthBar.text = currentHealth + "/" + maxHealth;
    }
}