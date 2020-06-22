using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    
    public void ShowShield(int number)
    {
        gameObject.SetActive(true);
        text.text="x" + number;
    }
    
    public void HideShield()
    {
        gameObject.SetActive(false);
    }
    private void Awake()
    {
//        gameObject.SetActive(false);
    }
}
