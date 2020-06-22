using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TMPro;
using UnityEngine;

public class TeleType : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
       
    }

    public void SetText(string content)
    {
        gameObject.SetActive(true);
        StartCoroutine(ShowText(content));
    }
        
    private IEnumerator ShowText(string content)
    {
        var originalString = content;
//        text.text=originalString;

        var numCharsRevealed = 0;
        while (numCharsRevealed < originalString.Length)
        {
            ++numCharsRevealed;
            text.text = originalString.Substring(0, numCharsRevealed);

            yield return new WaitForSeconds(0.05f);
        }
    }
}