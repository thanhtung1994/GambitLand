using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactiveController : MonoBehaviour
{
    [SerializeField] private float time;

    private void OnEnable()
    {
        StartCoroutine(Despawn());
    }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}