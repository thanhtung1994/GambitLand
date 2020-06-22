using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuryUIController : MonoBehaviour
{
    public GameObject furyUIInfoText;
    public GameObject furyText;
    public void OnClick()
    {
        if (CardGamePlayManager.Instance.isFury)
        {
            if (!GameManager.Instance.tutorial)
            {
                TutorialController.Instance.DoneTutorial();
                CardGamePlayManager.Instance.ChangeTurnPlayerTutorial();
            }

            CardGamePlayManager.Instance.UsedFury();
            furyText.SetActive(true);
        }
        else
        {
            furyUIInfoText.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        furyText.SetActive(false);
    }
}
    