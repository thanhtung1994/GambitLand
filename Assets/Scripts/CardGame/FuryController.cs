using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class FuryController : MonoBehaviour
{
    public TypeHealthBar _typefuryBar;

    [SerializeField] private Material matDefault;
    [SerializeField] private Material matFury;
    [MMReadOnly] public int currentFury;
    [MMReadOnly] public int maximumFury;
    [MMReadOnly] public int initialHealth;
    private MMProgressBar _furyBar;
    private bool isMax;
    private float time;
    private Image _spriteRenderer;

    private void Start()
    {
        currentFury = GameManager.Instance.currentdataplayer.currentFury;
        maximumFury = 10;
        initialHealth = 0;
        _furyBar = GameObject.FindGameObjectWithTag(_typefuryBar.ToString()).GetComponent<MMProgressBar>();
        UpdateFuryBar();
        _spriteRenderer = GetComponent<Image>();
        CheckMaxFury();
    }

    private void Update()
    {
        if (isMax)
        {
            if (Time.time - time >= 0.5f)
            {
                time = Time.time;
                _furyBar.Bump();
            }
        }
    }

    public void AddFury(int number)
    {
        if (currentFury >= maximumFury)
            return;
        currentFury += number;
        CheckMaxFury();
        CardGamePlayManager.Instance.temCurrentFury = currentFury;
        UpdateFuryBar();
    }

    public void CheckMaxFury()
    {
        if (currentFury >= maximumFury)
        {
            currentFury = maximumFury;
            isMax = true;
            _spriteRenderer.material = matFury;
            CardGamePlayManager.Instance.isFury = true;
            if (!GameManager.Instance.tutorial)
            {
                TutorialController.Instance.DoneTutorial();
            }
        }
    }

    private void UpdateFuryBar()
    {
        _furyBar.UpdateBar(currentFury, 0f, maximumFury);
    }

    public void UsedFury()
    {
        isMax = false;
        _spriteRenderer.material = matDefault;
        currentFury = 0;
        UpdateFuryBar();
    }
}