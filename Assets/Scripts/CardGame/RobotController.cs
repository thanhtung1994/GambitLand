using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.FeedbacksForThirdParty;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class RobotController : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite blackSprite;
    [SerializeField] private Sprite greenSprite;
    [SerializeField] private MMProgressBar _bar;
    [SerializeField] private TextMeshProUGUI textError;
    [SerializeField] private Button btnCal;
    [SerializeField] private TextMeshProUGUI textInfo;
    [SerializeField] private JackbotPanelController jackbotPanel;
    [SerializeField] private GameObject robotPanel;
    private int oldCount;
    private MMChromaticAberrationShaker _shaker;

    private void Start()
    {
        _bar.UpdateBar(0, 0, 9);
        oldCount = 0;
        _shaker = Camera.main.GetComponent<MMChromaticAberrationShaker>();
        btnCal.onClick.AddListener(OnCalculater);
        
    }

//    private void OnEnable()
//    {
//       Reset();
//    }

    private void Reset()
    {
        textInfo.gameObject.SetActive(false);
        _image.sprite = greenSprite;
        btnCal.interactable = true;
        _bar.UpdateBar(0, 0, 9);
        textError.gameObject.SetActive(false);
        oldCount = 0;
    }
    public void OnCalculater()
    {
        int a = Random.Range(1, 7);
        oldCount += a;
        
        if (oldCount > 9)
        {
            //TODO:Error
            _image.sprite = blackSprite;
            _shaker.StartShaking();
            textError.text = "Error \n" + oldCount;
            oldCount = 9;
            textError.gameObject.SetActive(true);
            btnCal.interactable= false;
            CardGamePlayManager.Instance.ErrorCard();
            textInfo.gameObject.SetActive(false);
        }
        else
        {
            textInfo.gameObject.SetActive(true);
            textInfo.text = oldCount.ToString();
            if (a < 6)
            {
                CardGamePlayManager.Instance.CreatePlayerDiceRequest(a);
            }
            else
            {
                On6();
            }

            if (oldCount == 9)
            {
                OnJackbot();
            }
        }
        _bar.UpdateBar(oldCount, 0, 9);
    }

    private void On6()
    {
        int a = Random.Range(0, 6);
        switch (a)
        {
            case 0:
                CardGamePlayManager.Instance.CreatePlayerDiceRequest(6);
                break;
            case 1:
                CardGamePlayManager.Instance.CreatePlayerDiceRequest(5);
                CardGamePlayManager.Instance.CreatePlayerDiceRequest(1);
                break;
            case 2:
                CardGamePlayManager.Instance.CreatePlayerDiceRequest(4);
                CardGamePlayManager.Instance.CreatePlayerDiceRequest(2);
                break;
            case 3:
                CardGamePlayManager.Instance.CreatePlayerDiceRequest(4);
                CardGamePlayManager.Instance.CreatePlayerDiceRequest(1);
                CardGamePlayManager.Instance.CreatePlayerDiceRequest(1);
                break;
            case 4:
                CardGamePlayManager.Instance.CreatePlayerDiceRequest(3);
                CardGamePlayManager.Instance.CreatePlayerDiceRequest(3);
                break;
            case 5:
                CardGamePlayManager.Instance.CreatePlayerDiceRequest(3);
                CardGamePlayManager.Instance.CreatePlayerDiceRequest(2);
                CardGamePlayManager.Instance.CreatePlayerDiceRequest(1);
                break;
        }
    }

    public void OnTextInfo(int a )
    { 
//        textInfo.transform.Translate();
    }

    public void Init(bool isactive)
    {
        robotPanel.SetActive(isactive);
        if(isactive)
            Reset();
        else
        {
            jackbotPanel.Hide();
        }
    }
    private  IEnumerator Jackbot()
    {
        btnCal.interactable= false;
        _bar.Bump();
        yield return  new WaitForSeconds(1);
        jackbotPanel.Show();
        robotPanel.SetActive(false);
    }

    public void OnJackbot()
    {
        StartCoroutine(Jackbot());
    }
}