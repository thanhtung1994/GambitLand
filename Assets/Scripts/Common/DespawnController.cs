using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnController : MonoBehaviour
{
    [SerializeField] private float time;

    private void OnEnable()
    {
        StartCoroutine(Despawn());
    }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(time); 
        SmartPool.Instance.Despawn(gameObject);
    }
}
