using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private TextMeshProUGUI textDice;
    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private TextMeshProUGUI textDes;

    [SerializeField] private Image imgDice;
    [SerializeField] private Sprite[] spriteDice;
    public CardType cardType;
    protected Animator anim;
    private string name;
    private string des;
    public int numberLoopMax;
    public int currentLoop;

    public int max;
    public int min;
    public int currentScore;
    public int score;
    protected bool isPlayer;

    private bool isActive;
    public int evenodd;

    public bool isDiceValueAttack;
    public bool isDiceValue;
    public int value;
    public bool useDiceValueEffect;
    public int valueAttack;
    public int scaleDamageAttack;
    public int dicenumber;
    public int conditionEffect;
    public int onePerBattle;
    public bool isDealDamage = true;
    public bool isUse1TimeBattle;
    private string oldName;


    // 0:Shock
    // 1:Curse
    //2:Weaken
    public int effect;
    private Image img;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        img = GetComponent<Image>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetData(CardDB cardDb)
    {
        this.name = cardDb.name;
        this.des = cardDb.des;
        this.numberLoopMax = cardDb.loopMax;
        this.max = cardDb.max;
        this.min = cardDb.min;
        this.score = cardDb.score;
        this.value = cardDb.value;
        this.valueAttack = cardDb.valueAttack;
        this.scaleDamageAttack = cardDb.scaleDamageAttack;
        this.evenodd = cardDb.evenodd;
        this.dicenumber = cardDb.dicenumber;
        this.conditionEffect = cardDb.conditionEffect;
        this.useDiceValueEffect = cardDb.useDiceValueEffect;
        this.isDealDamage = cardDb.isDealDamage;
        this.isUse1TimeBattle = cardDb.isUseTimeBattle;
        if (valueAttack != 0)
            isDiceValueAttack = false;
        else
            isDiceValueAttack = true;

        if (value != 0)
        {
            isDiceValue = false;
        }
        else
        {
            isDiceValue = true;
        }

        currentLoop = 1;
    }


    public void Init(bool isPlayer)
    {
//        img.material.DisableKeyword("SHAKEUV_ON");
        this.isPlayer = isPlayer;
        if (score > 0)
        {
            if (CardGamePlayManager.Instance.scoreCard.ContainsKey(name + isPlayer.ToString()))
            {
                currentScore = CardGamePlayManager.Instance.scoreCard[name + isPlayer.ToString()];
//                Debug.Log("Co vao day khong"+currentScore);
            }
            else
                currentScore = score;
        }

        effect = -1;
//        Reset();

        if (max > 0)
        {
            textDice.gameObject.SetActive(true);
            textDice.text = "Max " + max;
        }
        else if (score > 0)
        {
            textDice.gameObject.SetActive(true);
            textDice.text = currentScore.ToString();
//            currentScore = score;
        }
        else if (evenodd > 0)
        {
            textDice.gameObject.SetActive(true);
            if (evenodd == 1)
            {
                textDice.text = "Even";
            }
            else
            {
                textDice.text = "Odd";
            }
        }
        else if (dicenumber > 0)
        {
            imgDice.sprite = spriteDice[dicenumber - 1];
        }
        else
        {
            textDice.gameObject.SetActive(false);
        }

        textName.text = name;
        textDes.text = des;
    }

    private void Reset()
    {
        currentLoop = 1;
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        currentScore = 0;
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        DragDrop dragDrop = eventData.pointerDrag.GetComponent<DragDrop>();
        OnDrop(dragDrop);
    }

    public virtual void OnDrop(DragDrop dragDrop)
    {
        if (!GameManager.Instance.tutorial)
        {
            if (TutorialController.Instance.count == 0||TutorialController.Instance.count==3||TutorialController.Instance.count==11)
            {
                TutorialController.Instance.DoneTutorial();
            }
        }
        if (dragDrop.value > max && max > 0)
        {
            OnVibrate();
            return;
        }

        if (dragDrop.value < min && min > 0)
        {
            OnVibrate();
            return;
        }

        if (evenodd == 1)
        {
            if (dragDrop.value % 2 != 0)
            {
                OnVibrate();
                return;
            }
        }
        else if (evenodd == 2)
        {
            if (dragDrop.value % 2 == 0)
            {
                OnVibrate();
                return;
            }
        }

        if (dicenumber > 0)
        {
            if (dragDrop.value != dicenumber)
            {
                OnVibrate();
                return;
            }
        }

        if (score > 0)
        {
            currentScore -= dragDrop.value;
            if (currentScore > 0)
            {
                dragDrop.SetIsRightPlace(true);
                StartCoroutine(CountdownNumber());
                return;
            }
        }

        dragDrop.SetIsRightPlace(true);
        if (effect == 0)
        {
            OffShock();
            return;
        }
        else if (effect == 1)
        {
            StartCoroutine(CurseEffect());
            return;
        }

        if (isPlayer)
            anim.SetTrigger("Up");
        else
            anim.SetTrigger("Down");
        bool isconditionEffect = true;
        if (conditionEffect > 0)
        {
            if (conditionEffect != dragDrop.value)
                isconditionEffect = false;
        }

        if (isUse1TimeBattle)
            CardGamePlayManager.Instance.RemoveCard(isPlayer, name);
        CardGamePlayManager.Instance.ActionCard(useDiceValueEffect ? dragDrop.value : value, cardType,
            isDealDamage ? (isDiceValueAttack ? dragDrop.value * scaleDamageAttack : valueAttack) : 0,
            isconditionEffect);
    }

    public bool CheckCondition(int number)
    {
        if (number > max && max > 0)
        {
            return false;
        }

        if (number < min && min > 0)
        {
            return false;
        }

        if (evenodd == 1)
        {
            if (number % 2 != 0)
            {
                return false;
            }
        }
        else if (evenodd == 2)
        {
            if (number % 2 == 0)
            {
                return false;
            }
        }

        if (dicenumber > 0)
        {
            if (number != dicenumber)
            {
                return false;
            }
        }

        return true;
    }

    private IEnumerator CurseEffect()
    {
        GameObject go = CardGamePlayManager.Instance.CreateEffect("CloudExplosionGreenSkull", transform.position);
        go.transform.SetParent(transform.parent);
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        yield return new WaitForSeconds(0.5f);
        DespawnCard();
        yield return null;
    }

    public void OnError()
    {
        StartCoroutine(CurseEffect());
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

    public void OnVibrate()
    {
        StartCoroutine(Vibrate());
    }

    private IEnumerator Vibrate()
    {
        img.material.EnableKeyword("SHAKEUV_ON");
        yield return new WaitForSeconds(0.25f);
        img.material.DisableKeyword("SHAKEUV_ON");
        yield return null;
    }

    public void DespawnCard()
    {
        if (currentScore > 0)
        {
            if (CardGamePlayManager.Instance.scoreCard.ContainsKey(name + isPlayer.ToString()))
                CardGamePlayManager.Instance.scoreCard[name + isPlayer.ToString()] = currentScore;
            else
                CardGamePlayManager.Instance.scoreCard.Add(name + isPlayer.ToString(), currentScore);
        }
        else
        {
            if (CardGamePlayManager.Instance.scoreCard.ContainsKey(name + isPlayer.ToString()))
                CardGamePlayManager.Instance.scoreCard.Remove(name + isPlayer.ToString());
        }

        Reset();
        var parent = transform.parent;
        parent.SetParent(null);
        SmartPool.Instance.Despawn(parent.gameObject);
    }

    public void DesspawnCardUp()
    {
        Debug.Log("currentLoop"+currentLoop+"loopmax"+numberLoopMax);
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
        GameObject go = CardGamePlayManager.Instance.CreateEffect("FlashBomb", transform.position);
        go.transform.SetParent(transform.parent);
    }

    public void OnWeaken()
    {
        _canvasGroup.alpha = 0.7f;
        effect = 2;
    }

    public void OffWeaken()
    {
        _canvasGroup.alpha = 1;
        effect = -1;
    }

    public void OnCurse()
    {
        effect = 1;
    }

    public IEnumerator CountdownNumber()
    {
        int oldscore = int.Parse(textDice.text);
        while (oldscore > currentScore)
        {
            yield return new WaitForSeconds(0.25f);
            oldscore--;
            textDice.text = oldscore.ToString();
        }

        yield return null;
    }
}