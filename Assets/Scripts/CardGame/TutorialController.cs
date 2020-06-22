using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : SingletonMono<TutorialController>
{
    [SerializeField] private GameObject[] steps;
    public int count;

    public void Show()
    {
        steps[count].SetActive(true);
    }

    public void Hide()
    {
    }

    public void Done()
    {
        steps[count].SetActive(false);
    }

    public void DoneTutorial()
    {
//      count = index;
        steps[count].SetActive(false);
        count++;
        if (count < steps.Length)
            steps[count].SetActive(true);
//        StopAllCoroutines();
        if (count == 3 || count == 13)
            CardGamePlayManager.Instance.ActiveEndTurn();

        if (count == 1)
            StartCoroutine(AutoNext(2));
        else if (count == 5)
            StartCoroutine(AutoNext(3));
        else if (count == 12)
            StartCoroutine(AutoNext(1));
        else if (count == 16)
        {
            GameManager.Instance.currentdataplayer.AddCard(new Card("player0", 0, 0));
            GameManager.Instance.tutorial = true;
            GameManager.Instance.Save();
            PlayerPrefs.SetInt("Tutorial", 1);
        }
    }

    public IEnumerator AutoNext(int number)
    {
        yield return new WaitForSeconds(2);
        for (int i = 0; i < number; i++)
        {
            DoneTutorial();
            yield return new WaitForSeconds(2);
        }

//        yield return null;
    }
}