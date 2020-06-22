using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PlayerDB")]
public class PlayerData : ScriptableObject
{
    public string name;
    public int[] maxHP;
    public int[] dice;
    public int[] limitBreak;
    public int[] expToLevelUp;
}
