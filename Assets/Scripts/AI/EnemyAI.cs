using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EnemyAI : MonoBehaviour
{
    public PlayerController _playercontroller;
    private Vector3 pos;
    private bool isAction;
    private Transform dice;
    private CardSlot cardSlot;
    private DragDrop _dragDrop;
    private float speed = 4.5F;

    public void Action()
    {
//        MoveDice(_playercontroller.GetHigherDice(CardGamePlayManager.Instance.diceEnemy).transform,
//            _playercontroller.GetPosCard(0));
        StartCoroutine(FindDiceCard());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        if (isAction)
        {
            dice.position = Vector2.MoveTowards(dice.position, pos, speed);
            if (dice.position == pos)
            {
                cardSlot.OnDrop(_dragDrop);
                _dragDrop.DespawnDice();
                isAction = false;
                if (!CardGamePlayManager.Instance.isover)
                    StartCoroutine(FindDiceCard());
            }
        }
    }


    private IEnumerator FindDiceCard()
    {
        yield return new WaitForSeconds(1.5f);
        if (_playercontroller.CheckCard() && _playercontroller.CheckDice(CardGamePlayManager.Instance.diceEnemy))
        {
            isAction = true;
            DiceCard diceCard = _playercontroller.GetHigherDice(CardGamePlayManager.Instance.diceEnemy);
            if (diceCard == null)
            {
                CardGamePlayManager.Instance.EndTurn();
                isAction = false;
                Debug.Log("Dice Card null");
            }
            else
            {
                _dragDrop = diceCard.dragDrop;
                dice = _dragDrop.transform;
                cardSlot = diceCard.cardSlot;
                pos = cardSlot.transform.GetChild(0).transform.position;
                _dragDrop.IsEnableAnim(false);
                Debug.Log("Dice Card khong null");
            }
        }
        else
        {
            CardGamePlayManager.Instance.EndTurn();
        }

        yield return null;
    }
}

public class DiceCard
{
    public DragDrop dragDrop;
    public CardSlot cardSlot;
}