using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardChange : CardSlot
{
    public override void OnDrop(DragDrop dragDrop)
   {
       if (isPlayer)
           anim.SetTrigger("Up");
       else
           anim.SetTrigger("Down");
       CardGamePlayManager.Instance.ActionCard(dragDrop.value, cardType);
       dragDrop.CombineDice();
   }
}
