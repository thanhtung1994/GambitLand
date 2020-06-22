using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeechController : SingletonMono<SpeechController>
{
    [SerializeField]
    private TeleType textSpeech;

    public void SetString(string content)
    {
        
        textSpeech.SetText(content);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
