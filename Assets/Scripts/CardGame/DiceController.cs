using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiceController : MonoBehaviour
{
    public bool isPlayer;
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        if (isPlayer)
            if (GameManager.Instance.currentdataplayer.id != 1)
                SetText(GameManager.Instance.currentdataplayer.numberDice);
            else
            {
                gameObject.SetActive(false);
            }
        else
        {
            SetText(GameManager.Instance.dataEnemy.dice);
        }
    }

    public void SetText(int number)
    {
        text.text = "x" + number.ToString();
    }
}