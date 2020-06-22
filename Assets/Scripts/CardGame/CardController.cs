using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    CardSword,
    CardTrap,
    CardShield,
    CardChange,
    CardShock,
    CardBlind,
    CardCurse,
    CardLock,
    CardFreeze,
    CardWeakon,
    CardPoison,
    CardHealth,
    CardSteal,
    CardDodge,
    CardNewDice,
    CardDrain,
    CardReducehealthbyhalf,
    CardRoll2Dice,
    CardRat,
    CardPoisonBlind,
}

public class CardController : ActionCard
{
    public CardType cardType;

    public CardController()
    {
    }

    public virtual void DoSomething(int value, PlayerController player, PlayerController enemy, int valueattack = 0,
        bool isconditionEffect = true)
    {
    }
}

public class SwordCard : CardController
{
    public override void DoSomething(int value, PlayerController player, PlayerController enemy, int valueattack = 0,
        bool isconditionEffect = true)
    {
        enemy.TakeDamage(valueattack);
    }
}

public class TrapCard : CardController
{
    public override void DoSomething(int value, PlayerController player, PlayerController enemy, int valueattack = 0,
        bool isconditionEffect = true)
    {
        enemy.TakeDamage(valueattack);
        if (isconditionEffect)
            enemy.TrapDice(value);
    }
}

public class ShieldCard : CardController
{
    public override void DoSomething(int value, PlayerController player, PlayerController enemy, int valueattack = 0,
        bool isconditionEffect = true)
    {
        enemy.TakeDamage(valueattack);
        if (isconditionEffect)
            player.AddShield(value);
    }
}

public class ChangeCard : CardController
{
    public override void DoSomething(int value, PlayerController player, PlayerController enemy, int valueattack = 0,
        bool isconditionEffect = true)
    {
    }
}

public class ShockCard : CardController
{
    public override void DoSomething(int value, PlayerController player, PlayerController enemy, int valueattack = 0,
        bool isconditionEffect = true)
    {
        enemy.TakeDamage(valueattack);
        if (isconditionEffect)
            enemy.ShockCard(value);
    }
}

public class BlindCard : CardController
{
    public override void DoSomething(int value, PlayerController player, PlayerController enemy, int valueattack = 0,
        bool isconditionEffect = true)
    {
        enemy.TakeDamage(valueattack);
        if (isconditionEffect)
            enemy.BlindDice(value);
    }
}

public class CurseCard : CardController
{
    public override void DoSomething(int value, PlayerController player, PlayerController enemy, int valueattack = 0,
        bool isconditionEffect = true)
    {
        enemy.TakeDamage(valueattack);
        if (isconditionEffect)
            enemy.CurseCard(value);
    }
}

public class LockCard : CardController
{
    public override void DoSomething(int value, PlayerController player, PlayerController enemy, int valueattack = 0,
        bool isconditionEffect = true)
    {
        enemy.TakeDamage(valueattack);
        if (isconditionEffect)
            enemy.LockDice(value);
    }
}

public class FreezeCard : CardController
{
    public override void DoSomething(int value, PlayerController player, PlayerController enemy, int valueattack = 0,
        bool isconditionEffect = true)
    {
        enemy.TakeDamage(valueattack);
        if (isconditionEffect)
            enemy.FreezeDice(value);
    }
}

public class WeakenCard : CardController
{
    public override void DoSomething(int value, PlayerController player, PlayerController enemy, int valueattack = 0,
        bool isconditionEffect = true)
    {
        enemy.TakeDamage(valueattack);
        if (isconditionEffect)
            enemy.WeakenCard(value);
    }
}

public class PoisonCard : CardController
{
    public override void DoSomething(int value, PlayerController player, PlayerController enemy, int valueattack = 0,
        bool isconditionEffect = true)
    {
        enemy.TakeDamage(valueattack);
        if (isconditionEffect)
            enemy.PoisonCard(value);
    }
}

public class HealthCard : CardController
{
    public override void DoSomething(int value, PlayerController player, PlayerController enemy, int valueattack = 0,
        bool isconditionEffect = true)
    {
        enemy.TakeDamage(valueattack);
        if (isconditionEffect)
            player.AddHealth(value);
    }
}

public class StealCard : CardController
{
    public override void DoSomething(int value, PlayerController player, PlayerController enemy, int valueattack = 0,
        bool isconditionEffect = true)
    {
        enemy.TakeDamage(valueattack);
        if (isconditionEffect)
            player.AddGold(value);
    }
}

public class DodgeCard : CardController
{
    public override void DoSomething(int value, PlayerController player, PlayerController enemy, int valueattack = 0,
        bool isconditionEffect = true)
    {
        enemy.TakeDamage(valueattack);
        if (isconditionEffect)
            player.DodgeCard(value);
    }
}

public class NewDiceCard : CardController
{
    public override void DoSomething(int value, PlayerController player, PlayerController enemy, int valueattack = 0,
        bool isconditionEffect = true)
    {
        enemy.TakeDamage(valueattack);
        if (!GameManager.Instance.tutorial)
        {
            if (TutorialController.Instance.count == 8)
            {
                player.GetNewDice(value, 4);
                TutorialController.Instance.DoneTutorial();
            }
            else if (TutorialController.Instance.count == 9)
            {
                player.GetNewDice(value, 6);
                TutorialController.Instance.DoneTutorial();
                CardGamePlayManager.Instance.TutorialCount9();
            }

        }
        else
        {
            if (isconditionEffect)
                player.GetNewDice(value);
        }
    }
}

public class DrainCard : CardController
{
    public override void DoSomething(int value, PlayerController player, PlayerController enemy, int valueattack = 0,
        bool isconditionEffect = true)
    {
        enemy.TakeDamage(valueattack);
        if (isconditionEffect)
        {
            player.AddHealth(value);
            enemy.TakeDamage(value, true, false);
        }
    }
}

public class ReducehealthbyhalfCard : CardController
{
    public override void DoSomething(int value, PlayerController player, PlayerController enemy, int valueattack = 0,
        bool isconditionEffect = true)
    {
        enemy.ReduceHealthHalf();
    }
}

public class Roll2DiceCard : CardController
{
    public override void DoSomething(int value, PlayerController player, PlayerController enemy, int valueattack = 0,
        bool isconditionEffect = true)
    {
        player.Roll2Dice(value);
    }
}

public class RatCard : CardController
{
    public override void DoSomething(int value, PlayerController player, PlayerController enemy, int valueattack = 0,
        bool isconditionEffect = true)
    {
        enemy.TakeDamage(valueattack);
        if (isconditionEffect)
        {
            player.GetNewDice(1);
            enemy.PoisonCard(value);
        }
    }
}

public class PoisonBlindCard : CardController
{
    public override void DoSomething(int value, PlayerController player, PlayerController enemy, int valueattack = 0,
        bool isconditionEffect = true)
    {
        if (isconditionEffect)
        {
            enemy.PoisonCard(value);
            enemy.BlindDice(valueattack);
        }
    }
}