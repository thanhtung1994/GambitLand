using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataPlayer
{
    public string name;
    public int id;
    public int level;
    public int currentexp;

    public int CurrentHealth
    {
        get {  return currentHealth; }
        set
        {
            currentHealth = value;
            if (GameManager.Instance._updateHealthBarGamePlay != null)
                GameManager.Instance._updateHealthBarGamePlay.Invoke();
        }
    }

    public int currentHealth;
  
    public int maxFury;
    public int currentFury;

    public int currentLevelMap;
    public int numberDice;
    public List<Card> card;
    public int gold;
    public int diff;

    // TODO: Va nhieu thu nua
    public DataPlayer()
    {
    }

    public DataPlayer(int typePlayer, int level, int currentHealth, int currentLevelMap, List<Card> card, int diff)
    {
        name = "Boy";
        this.currentLevelMap = 0;
        this.id = typePlayer;
        this.level = level;
        this.currentHealth = currentHealth;
        this.currentLevelMap = currentLevelMap;
        this.card = card;
        this.diff = diff;
        this.numberDice = 2;
        maxFury = 10;
        id = 0;
    }

    public void AddGold(int number)
    {
        gold += number;
    }

    public void AddExp()
    {
        currentexp++;
    }

    public void SetHealth(int health)
    {
        CurrentHealth = health;
    }

    public void AddHealth(int health)
    {
        CurrentHealth += health;
    }

    public void AddCard(Card card)
    {
        this.card.Add(card);
    }
}

[Serializable]
public class Card
{
    public string nameCard;
    public int numberType;
    public int level;

    public Card()
    {
    }

    public Card(string nameCard, int numberType, int level)
    {
        this.nameCard = nameCard;
        this.numberType = numberType;
        this.level = level;
    }
}