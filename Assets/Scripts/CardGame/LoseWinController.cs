using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoseWinController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void SetText(string content)
    {
        text.text = content;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
