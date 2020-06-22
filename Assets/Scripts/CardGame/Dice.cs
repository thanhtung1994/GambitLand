using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private GameObject child;

    private void Start()
    {
        child.SetActive(true);
    }
}