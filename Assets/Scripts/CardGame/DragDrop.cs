using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [MMReadOnly] public int value;
    [SerializeField] private CanvasGroup canvasGroup;
    private RectTransform _rectTransform;
    private Animator anim;
    private Vector2 posOrigin;
    private bool isRightPlace;
    private Material _material;

    private Image img;

    //0:Burn
    //1:Blind
    //2:lock
    //3:Frezze
    public int effect;
    private bool isPlayer;
    [SerializeField] private Sprite[] diceSprite;
    [SerializeField] private Sprite trapSprite;

    [SerializeField] private Sprite blindSprite;
    [SerializeField] private Sprite lockSprite;
    private int tmp;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        anim = GetComponent<Animator>();
        posOrigin = _rectTransform.anchoredPosition;
        isRightPlace = false;
        img = GetComponent<Image>();
//        _material = img.material;
        if (gameObject.tag.Equals("DicePlayer"))
        {
            isPlayer = true;
        }
        else if (gameObject.tag.Equals("DiceEnemy"))
        {
            isPlayer = false;
        }
    }


    private void OnEnable()
    {
        transform.localPosition = Vector3.zero;
        effect = -1;
        OnUp();
    }

    public void SetIsRightPlace(bool isRight)
    {
        isRightPlace = isRight;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
//        Debug.Log("PointDow");
        if (effect == 2)
            return;
        anim.enabled = false;
        if (effect == 0)
        {
            TakeDamageTrap();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
//        Debug.Log("BeginDrag");
        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");
        if (!isRightPlace)
        {
            canvasGroup.blocksRaycasts = true;
            _rectTransform.anchoredPosition = posOrigin;
        }
        else
        {
            DespawnDice();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
//        Debug.Log("OnDrag" + eventData.delta);
        _rectTransform.anchoredPosition += eventData.delta/CardGamePlayManager.Instance.canvasScale.scaleFactor;
    }

    public void OnDown()
    {
        anim.enabled = true;
        anim.SetTrigger("Down");
    }

    public void OnUp()
    {
        anim.enabled = true;
        anim.SetTrigger("Up");
    }

    public void DespawnDice()
    {
        transform.parent.SetParent(null);
        canvasGroup.blocksRaycasts = true;
        isRightPlace = false;
        SmartPool.Instance.Despawn(transform.parent.gameObject);
    }

    public void Init()
    {
        tmp = Random.Range(0, 6);
        this.value = tmp + 1;
        img.sprite = diceSprite[tmp];
        effect = -1;
    }

    public void SetValueDice(int number)
    {
        this.value = number;
        img.sprite = diceSprite[number - 1];
        effect = -1;
    }
    public void OnTrap()
    {
        img.sprite = trapSprite;
        effect = 0;
    }

    public void OffTrap()
    {
        img.sprite = diceSprite[tmp];
        effect = -1;
    }

    public void OnBlind()
    {
        img.sprite = blindSprite;
        effect = 1;
        Debug.Log("co vao day khong");
    }

    public void OnLock()
    {
        img.sprite = lockSprite;
        effect = 2;
    }

    public void OnFreeze()
    {
      GameObject go =CardGamePlayManager.Instance.CreateEffect("Freeze",transform.position);
      go.transform.SetParent(transform);
      value = 1;
      img.sprite = diceSprite[0];
      effect = 3;
    }

    
    public void TakeDamageTrap()
    {
        PlayerController play = isPlayer
            ? CardGamePlayManager.Instance._playerController
            : CardGamePlayManager.Instance._enemyController;
        play.TakeDamage(2,true,true);
        OffTrap();
    }

    public void CombineDice()
    {
        Init();
        OnUp();
    }

    public void IsEnableAnim(bool isEnable)
    {
        anim.enabled = isEnable;
    }
}