using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuryUIText : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(DelayHide());
    }

    private IEnumerator DelayHide()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}
