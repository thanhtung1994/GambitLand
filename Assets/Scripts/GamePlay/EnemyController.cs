using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public bool iscantAttack;

    private void Start()
    {
        iscantAttack = false;
    }

    private void OnEnable()
    {
        iscantAttack = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (iscantAttack)
            return;
        if (other.gameObject.tag.Equals("Player"))
        {
            GameManager.Instance.SetDataEnemy(GameManager.Instance.LoadDataEnemy(gameObject.name));
            GridManager.Instance.SetEnemy(this);
            ManagerLand.Instance.LoadGamePlay();
            Debug.Log(other.gameObject.name);
        }
    }

    public void Die()
    {
        iscantAttack = true;
        StartCoroutine(DelayDespawn());
    }

    private IEnumerator DelayDespawn()
    {
        yield return new WaitForSeconds(2);
        LevelManagerGamePLay.Instance.CreateEffect("Smoke", transform.position);
        SmartPool.Instance.Despawn(gameObject);
    }
        
}
