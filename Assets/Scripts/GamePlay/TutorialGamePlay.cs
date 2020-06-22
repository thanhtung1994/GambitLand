using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialGamePlay : MonoBehaviour
{
    [SerializeField] private EnemyData _dataEnemy;
    [SerializeField] private string[] key;
    [SerializeField] private GameObject gate;
    private bool isTutorial;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isTutorial)
            return;
        if (other.tag.Equals("Player"))
        {
            ManagerLand.Instance.StopJoystick();
            LevelManagerGamePLay.Instance.CutScene();
            StartCoroutine(RunSpeech());
            isTutorial = true;
        }
    }

    public IEnumerator RunSpeech()
    {
//        for (int i = 0; i < key.Length; i++)
//        {
//            SpeechController.Instance.SetString(I2.Loc.LocalizationManager.GetTranslation(key[i]));
//            float time = I2.Loc.LocalizationManager.GetTranslation(key[i]).ToCharArray().Length;
//            yield return new WaitForSeconds(time * 0.05f+2);
//        }
        yield return new WaitForSeconds(1);
        SpeechController.Instance.Hide();
        GameManager.Instance.SetDataEnemy(GameManager.Instance.LoadDataEnemy("Wizzard"));
        //        SceneManager.LoadScene("GamePlay");
        LevelManagerGamePLay.Instance.EndCutScene();
        ManagerLand.Instance.LoadGamePlay();
    }
}