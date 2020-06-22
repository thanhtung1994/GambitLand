using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardSlot : MonoBehaviour, IDropHandler
{
    public CardType cardType;
    protected Animator anim;
    public int numberLoopMax;
    public int currentLoop;
    protected bool isPlayer;
    // 0:Shock
    // 1:Curse
    public int effect;
    private Image img;
    private CanvasGroup _canvasGroup;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        img = GetComponent<Image>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Init(bool isPlayer)
    {
        this.isPlayer = isPlayer;
        effect = -1;
        Reset();
    }

    private void Reset()
    {
        currentLoop = 1;
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        DragDrop dragDrop = eventData.pointerDrag.GetComponent<DragDrop>();
        OnDrop(dragDrop);

    }

    public virtual void OnDrop(DragDrop dragDrop)
    {
        dragDrop.SetIsRightPlace(true);
        if (effect == 0)
        {
            OffShock();
            return;
        }else if (effect == 1)
        {
            GameObject go = CardGamePlayManager.Instance.CreateEffect("SoulExplosionPurple",transform.position);
            go.transform.SetParent(transform.parent);
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
//            DespawnCard();
            return;
        }
        if (isPlayer)
            anim.SetTrigger("Up");
        else
            anim.SetTrigger("Down");
        CardGamePlayManager.Instance.ActionCard(dragDrop.value, cardType);
    }
    public void OnLeft()
    {
        anim.SetTrigger("Left");
    }

    public void OnEnemyLeft()
    {
        anim.SetTrigger("EnemyLeft");
    }

    public void OnEnemyRight()
    {
        anim.SetTrigger("EnemyRight");
    }

    public void DespawnCard()
    {
        Reset();
        transform.parent.SetParent(null);
        SmartPool.Instance.Despawn(transform.parent.gameObject);
       
    }

    public void DesspawnCardUp()
    {
        if (numberLoopMax > 0 && currentLoop < numberLoopMax)
        {
            currentLoop++;
            OnEnemyLeft();
        }
        else
        {
           DespawnCard();
        }
    }
    
    public void OnShock()
    {
        img.color = Color.black;
        effect = 0;
    }

    public void OffShock()
    {
        img.color = Color.white;
        effect = -1;
    }

    public void OnCurse()
    {
        effect = 1;
    }
}