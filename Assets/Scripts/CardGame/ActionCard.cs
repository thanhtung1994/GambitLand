using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ActionCard
{
    void DoSomething(int value, PlayerController player, PlayerController enemy, int valueattack = 0,
        bool isconditionEffect = true);
}