using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    private PostProcessVolume _volume;
    protected Vignette _vignette;
    private float _initialIntensity;
    private float time;
    private bool isClose;
    public bool isOpen;
    private float offset;

    private void Start()
    {
        _volume = GetComponent<PostProcessVolume>();
        _volume.profile.TryGetSettings(out _vignette);
        _initialIntensity = _vignette.intensity;
        offset = 0.5f;
    }

    private void OnEnable()
    {
       Open();
    }

    private void FadeScene()
    {
    }

    private void Update()
    {
        
        if (isClose)
        {
            _initialIntensity += 0.25f;
            _vignette.intensity.Override(_initialIntensity);
           
            if (_initialIntensity >= 20)
            {
                isClose = false;
            }
        }
        else if (isOpen)
        {
            if (offset >= 0.1f)
                offset -= Time.deltaTime * 0.25f;
            _initialIntensity -= 0.25f;
            _vignette.intensity.Override(_initialIntensity);
            if (_initialIntensity <= 0)
            {
                isOpen = false;
            }
        }
    }

    public void Close()
    {
        isClose = true;
    }

    public void Open()
    {
        isOpen = true;
    }

}
