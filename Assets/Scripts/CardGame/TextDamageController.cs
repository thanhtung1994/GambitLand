using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextDamageController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void SetText(int damage)
    {
        gameObject.SetActive(true);
        text.text = "-" + damage;
    }
}
