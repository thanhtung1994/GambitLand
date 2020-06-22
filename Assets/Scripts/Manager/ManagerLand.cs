using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class ManagerLand : SingletonMono<ManagerLand>
{
    [SerializeField] private MMTouchDynamicJoystick _joystick;
    [SerializeField] private CameraController gambitCam;
    [SerializeField] private CameraController gameplayCam;
    [SerializeField] private GameObject panelLose;
    public GameObject gamePlay;

    public GameObject gambit;
    private bool isWin;
    private void Start()
    {
        isWin = false;
    }
    

    public void LoadGamePlay()
    {
        StartCoroutine(AysnGamePlay());
//        gamePlay.SetActive(true);
//        gambit.SetActive(false);
        
    }

    private IEnumerator AysnGamePlay()
    {
        gambitCam.Close();
        yield return new WaitForSeconds(1.5f);
        gamePlay.SetActive(true);
        gambit.SetActive(false);
        
    }
    public void LoadGambitLand()
    {
        StartCoroutine(AysnGambitLand());
        
    }

    private IEnumerator AysnGambitLand()
    {
        gameplayCam.Close();
        yield return  new WaitForSeconds(1.5f);
        gambit.SetActive(true);
        gamePlay.SetActive(false);
        if (isWin)
        {
            GridManager.Instance.Win();
        }
    }
    public void StopJoystick()
    {
        _joystick.OnEndDrag(null);
    }

    public void SetWinorLose(bool isWin)
    {
        this.isWin = isWin;
        if(!isWin)
            panelLose.SetActive(true);
    }
    
}