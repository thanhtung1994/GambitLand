using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeElement
{
    None,
    Fire,
    Poison,
    Earth,
    Shock,
    Ice,
}
[CreateAssetMenu(menuName = "EnemyDB")]
public class EnemyData : ScriptableObject
{
    public string name;
    public int level;
    public int maxhealth;
    public int dice;
    public List<Card> cards;
    public bool isBoos;
    public bool rare;
    public TypeElement typeElement;
}
