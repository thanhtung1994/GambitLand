using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public Health health;
    public int id;
    private bool isPlayer;
    private int numberTrap;
    private int numberShock;
    private int numberBlind;
    private int numberCurse;
    private int numberLock;
    private int numberFreeze;
    private int numberWeaken;
    private int numberPoison;
    private int numberDodge;
    public FuryController _furyController;
    public EnemyAI _enemyAI;
    private string textInfo;

    private void Awake()
    {
        _furyController = GetComponent<FuryController>();
    }

    public void SetPlayer(bool isPlayer)
    {
        this.isPlayer = isPlayer;
        ResetEffect();
        health.Init();
        textInfo = "";
    }

    public void ResetEffect()
    {
        numberTrap = 0;
        numberShock = 0;
        numberBlind = 0;
        numberCurse = 0;
        numberLock = 0;
        numberFreeze = 0;
        numberWeaken = 0;
        numberPoison = 0;
        numberDodge = 0;
    }

    public void TakeDamage(int damage, bool activeHitEffect = true, bool isSheild = true)
    {
        if (numberDodge > 0)
        {
            numberDodge--;
            return;
        }

        health.TakeDamage(damage, activeHitEffect, isSheild);
        if (isPlayer)
            _furyController.AddFury(damage);
    }

    public void ReduceHealthHalf()
    {
        health.ReduceHealthHalf();
    }

    public void AddShield(int number)
    {
        health.AddShield(number);
    }

    public void AddHealth(int number)
    {
        health.AddHealth(number);
        GameObject go = CardGamePlayManager.Instance.CreateEffect("MagicEnchantGreen", transform.position);
        go.transform.SetParent(transform.parent);
    }

    public void AddGold(int number)
    {
        GameManager.Instance.currentdataplayer.AddGold(number);
    }

    public void AddExp()
    {
        GameManager.Instance.currentdataplayer.AddExp();
        if (GameManager.Instance.currentdataplayer.currentexp >= GameManager.Instance.GetMaxLevelUp())
        {
            GameManager.Instance.LevelUp();
            CardGamePlayManager.Instance.LevelUp();
        }
    }

    public void TrapDice(int number)
    {
        numberTrap += number;
        UpdateTextInfo();
        health.SetTextInfo(textInfo);
    }

    public void ShockCard(int number)
    {
        numberShock = number;
        UpdateTextInfo();
        health.SetTextInfo(textInfo);
    }

    public void WeakenCard(int number)
    {
        numberWeaken = number;
        UpdateTextInfo();
        health.SetTextInfo(textInfo);
    }

    public void PoisonCard(int number)
    {
        numberPoison += number;
        UpdateTextInfo();
        health.SetTextInfo(textInfo);
    }

    public void BlindDice(int number)
    {
        numberBlind += number;
        UpdateTextInfo();
        health.SetTextInfo(textInfo);
    }

    public void CurseCard(int number)
    {
        numberCurse += number;
        UpdateTextInfo();
        health.SetTextInfo(textInfo);
    }

    public void LockDice(int number)
    {
        numberLock += number;
        UpdateTextInfo();
        health.SetTextInfo(textInfo);
    }

    public void FreezeDice(int number)
    {
        numberFreeze += number;
        UpdateTextInfo();
        health.SetTextInfo(textInfo);
    }

    public void DodgeCard(int number)
    {
        numberDodge += number;
        UpdateTextInfo();
        health.SetTextInfo(textInfo);
    }

    public void GetNewDice(int number, int value = 0)
    {
        if (isPlayer)
            CardGamePlayManager.Instance.CreateDicePlayerEffect(number, value);
        else
            CardGamePlayManager.Instance.CreateDiceEnemyEffect(number, value);
    }

    public void Roll2Dice(int value)
    {
        if (isPlayer)
            CardGamePlayManager.Instance.CreateDicePlayerEffect(2, value);
        else
            CardGamePlayManager.Instance.CreateDiceEnemyEffect(2, value);
    }

    public void CheckEffectDiceAfter()
    {
        Transform tf;
        if (isPlayer)
        {
            tf = CardGamePlayManager.Instance.dicePlayer;
        }
        else
        {
            tf = CardGamePlayManager.Instance.diceEnemy;
        }

        if (numberTrap > 0)
        {
            int countTrap = 0;

            foreach (Transform t in tf)
            {
                DragDrop drag = t.GetComponentInChildren<DragDrop>();
                if (drag.effect == -1)
                {
                    drag.OnTrap();
                    countTrap++;
                }

                if (countTrap >= numberTrap)
                {
                    numberTrap = 0;
                    break;
                }
            }
            
        }

        if (numberFreeze > 0)
        {
            int countFreeze = 0;
            
            foreach (Transform t in tf)
            {
                DragDrop drag = t.GetComponentInChildren<DragDrop>();
                if (drag.effect == -1)
                {
                    drag.OnFreeze();
                    countFreeze++;
                    Debug.Log("Co vao day khong tung");
                }

                if (countFreeze >= numberFreeze)
                {
                    numberFreeze = 0;
                    break;
                }
            }

          
        }
        UpdateTextInfo();
    }

    public void CheckEffectDiceFirst()
    {
        Transform tf;
        if (isPlayer)
        {
            tf = CardGamePlayManager.Instance.dicePlayer;
        }
        else
        {
            tf = CardGamePlayManager.Instance.diceEnemy;
        }

        if (numberLock > 0)
        {
            int countLock = 0;
            foreach (Transform t in tf)
            {
                DragDrop drag = t.GetComponentInChildren<DragDrop>();
                if (drag.effect == -1)
                {
                    drag.OnLock();
                    countLock++;
                }

                if (countLock >= numberLock)
                {
                    numberLock = 0;
                    break;
                }
            }
           
        }

        if (numberBlind > 0)
        {
            int countBlind = 0;
            foreach (Transform t in tf)
            {
                DragDrop drag = t.GetComponentInChildren<DragDrop>();
                if (drag.effect == -1)
                {
                    drag.OnBlind();
                    countBlind++;
                }

                if (countBlind >= numberBlind)
                {
                    numberBlind = 0;
                    break;
                }
            }
        }
        UpdateTextInfo();
    }

    public void CheckEffectCard()
    {
        Transform tf;
        tf = CardGamePlayManager.Instance.cardGird.transform;
        if (numberShock > 0)
        {
            int countShock = 0;
            foreach (Transform t in tf)
            {
                CardSlot cardSlot = t.GetComponentInChildren<CardSlot>();
                if (cardSlot.effect == -1)
                {
                    cardSlot.OnShock();
                    countShock++;
                }

                if (countShock >= numberShock) break;
            }
        }

        if (numberCurse > 0)
        {
            int countCurse = 0;
            foreach (Transform t in tf)
            {
                CardSlot cardSlot = t.GetComponentInChildren<CardSlot>();
                if (cardSlot.effect == -1)
                {
                    cardSlot.OnCurse();
                    countCurse++;
                }

                if (countCurse >= numberCurse) break;
            }
        }

        if (numberWeaken > 0)
        {
            int countWeakon = 0;
            foreach (Transform t in tf)
            {
                CardSlot cardSlot = t.GetComponentInChildren<CardSlot>();
                if (cardSlot.effect == -1)
                {
                    cardSlot.OnWeaken();
                    countWeakon++;
                }

                if (countWeakon >= numberWeaken) break;
            }
        }
        UpdateTextInfo();
    }

    public void CheckTextInfoEndTurn()
    {
//        UpdateTextInfo();
//        health.SetTextInfo(textInfo);
//        textInfo = "";
    }

    public void CheckEffectHurt()
    {
        if (numberPoison > 0)
        {
            numberPoison--;
            health.TakeDamage(1, false,false);
            GameObject go = CardGamePlayManager.Instance.CreateEffect("PoisonImpactPurple", transform.position);
            go.transform.SetParent(transform.parent);
        }
    }

    public DiceCard GetHigherDice(Transform tf)
    {
        int number = 0;
        DragDrop dr = null;
        CardSlot cd = null;
        foreach (Transform dice in tf)
        {
            DragDrop dragDrop = dice.GetComponentInChildren<DragDrop>();
            CardSlot cardSlot = GetCard(dragDrop.value);
            if (dragDrop.value > number && dragDrop.effect != 2 && cardSlot != null)
            {
                number = dragDrop.value;
                dr = dragDrop;
                cd = cardSlot;
            }
        }

        if (dr != null)
        {
            if (dr.effect == 0)
            {
                if (health.currentHealth > 3)
                {
                    dr.TakeDamageTrap();
                }
                else
                {
                    dr = null;
                }
            }
        }

        if (dr != null && cd != null)
        {
            DiceCard diceCard = new DiceCard();
            diceCard.dragDrop = dr;
            diceCard.cardSlot = cd;
            return diceCard;
        }

        return null;
    }

    public CardSlot GetCard(int number)
    {
        for (int i = 0; i < CardGamePlayManager.Instance.cardGird.transform.childCount; i++)
        {
            CardSlot cardSlot = CardGamePlayManager.Instance.cardGird.transform.GetChild(i)
                .GetComponentInChildren<CardSlot>();
            if (cardSlot.CheckCondition(number))
            {
                return cardSlot;
            }
        }

        return null;
    }

    public void UpdateTextInfo()
    {
        textInfo = "";
        if (numberTrap > 0)
        {
            textInfo += numberTrap + SpriteText("trap");
        }

        if (numberShock > 0)
        {
            textInfo += numberShock + SpriteText("shock");
        }

        if (numberBlind > 0)
        {
            textInfo += numberShock + SpriteText("blind");
        }

        if (numberCurse > 0)
        {
            textInfo += numberCurse + SpriteText("curse");
        }

        if (numberLock > 0)
        {
            textInfo += numberLock + SpriteText("lock");
        }

        if (numberFreeze > 0)
        {
            textInfo += numberFreeze + SpriteText("freeze");
        }

        if (numberWeaken > 0)
        {
            textInfo += numberWeaken + SpriteText("weaken");
        }

        if (numberPoison > 0)

        {
            textInfo += numberPoison + SpriteText("poison");
        }

        if (numberDodge > 0)
        {
            textInfo += numberPoison + SpriteText("dodge");
        }
        health.SetTextInfo(textInfo);
    }

    private string SpriteText(string text)
    {
        string a = string.Format("  <sprite name={0}>", text);
        return a;
    }


    public bool CheckCard()
    {
        return CardGamePlayManager.Instance.cardGird.transform.childCount > 0;
    }

    public bool CheckDice(Transform tf)
    {
        return tf.childCount > 0;
    }
}