using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "CardDB")]
public class CardDB : ScriptableObject
{
    public CardType cardType;
    public string name;
    public string des;
    public int loopMax;
    public int max;
    public int min;
    public int score;
    public int effect;
    public int value;
    public bool useDiceValueEffect;
    public int valueWeakon;
    public int valueAttack;
    //Scale damage vd x2 x3 
    public int scaleDamageAttack = 1;
    //chan le 
    public int evenodd;
    // Bat buoc phai dung so nay moi dc 
    public int dicenumber;
    public int conditionEffect;
    public bool isDealDamage = true;
    public bool isUseTimeBattle;
}
