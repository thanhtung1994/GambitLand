using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using MoreMountains.FeedbacksForThirdParty;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CardGamePlayManager : SingletonMono<CardGamePlayManager>
{
    public Transform dicePlayer;
    public Transform diceEnemy;
    public Canvas canvasScale;
    public GameObject turotialObj;
    [SerializeField] private MMChromaticAberrationShaker hitCam;
    [SerializeField] private GameObject panelBlock;
    [SerializeField] private GameObject dicePlayerElement;
    [SerializeField] private GameObject diceEnemyElement;
    [SerializeField] private GameObject cardElement;
    [SerializeField] private GameObject endturnPanel;
    [SerializeField] private Transform playerPos;
    [SerializeField] private Transform enemyPos;
    [SerializeField] private GridLayoutGroup playerGrid;
    [SerializeField] private GridLayoutGroup enemyGrid;
    [SerializeField] private GridLayoutGroup card1;
    [SerializeField] private GridLayoutGroup card2;
    [SerializeField] private GridLayoutGroup card3;
    [SerializeField] private GridLayoutGroup card4;
    [SerializeField] private Animator animHealthPlayer;
    [SerializeField] private Animator animHealthEnemy;
    [SerializeField] private ShieldController playerShieldController, enemyShieldController;
    public GridLayoutGroup cardGird;
    [SerializeField] private LoseWinController _loseWinController;
    [SerializeField] private LevelUpManager _levelUpManager;
    [SerializeField] private RobotController _robotController;
    private bool isTurnPlayer;
    public PlayerController _playerController;
    public PlayerController _enemyController;
    private CardController _cardController;
    public bool isFury;
    public bool isUseFury;
    private bool isCanAction;

    private List<Card> playerCard;
    private List<Card> enemyCard;
    private int numberDicePlayer;
    private int numberDiceEnemy;
    public bool isover;
    public Dictionary<string, int> scoreCard = new Dictionary<string, int>();
    public int temCurrentHealth;
    public int temCurrentFury;

    public bool isAutoSpawnDicePlayer
    {
        get { return _playerController.id != 1; }
    }

    private void Awake()
    {
        if (!GameManager.Instance.tutorial)
        {
            GameObject go = SmartPool.Instance.Spawn(turotialObj, Vector2.zero, Quaternion.identity);
            go.transform.SetParent(canvasScale.transform);
            go.transform.localScale = Vector3.one;
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.offsetMax = rect.offsetMin = Vector2.zero;
        }
    }

    private void OnEnable()
    {
        if (GameManager.Instance.tutorial)
            StartCoroutine(IStart());
        else
            StartCoroutine(ITutorial());
    }

    private IEnumerator IStart()
    {
        isover = false;
        isTurnPlayer = true;
        _loseWinController.Hide();
        _levelUpManager.gameObject.SetActive(false);
        playerCard = new List<Card>(GameManager.Instance.currentdataplayer.card);
        enemyCard = new List<Card>(GameManager.Instance.dataEnemy.cards);
        numberDicePlayer = GameManager.Instance.currentdataplayer.numberDice;
        numberDiceEnemy = GameManager.Instance.dataEnemy.dice;
        playerShieldController.gameObject.SetActive(true);
        enemyShieldController.gameObject.SetActive(true);
        CreateHero();
        CreateEnemy();
        StartCoroutine(ICreatePlayerDice());
//        StartCoroutine(ICreateEnemyDice());


        yield return new WaitForSeconds(1f);
        StartCoroutine(ICreatePlayerCard());
        endturnPanel.SetActive(true);
    }

    private IEnumerator ITutorial()
    {
        isover = false;
        isTurnPlayer = true;
        _loseWinController.Hide();
        _levelUpManager.gameObject.SetActive(false);
        playerCard = new List<Card>(GameManager.Instance.currentdataplayer.card);
        enemyCard = new List<Card>(GameManager.Instance.dataEnemy.cards);
        numberDicePlayer = GameManager.Instance.currentdataplayer.numberDice;
        numberDiceEnemy = GameManager.Instance.dataEnemy.dice;
        playerShieldController.gameObject.SetActive(true);
        enemyShieldController.gameObject.SetActive(true);
        CreateHero();
        CreateEnemy();
        StartCoroutine(ICreatePlayerDiceRequest(5));
        yield return new WaitForSeconds(1f);
        StartCoroutine(ICreatePlayerCard());
        endturnPanel.SetActive(false);
        yield return new WaitForSeconds(1f);
        TutorialController.Instance.Show();
    }

    public void EndTurn()
    {
        if (isover)
            return;
        if (!GameManager.Instance.tutorial)
        {
            if (TutorialController.Instance.count == 3)
                TutorialController.Instance.Done();
            else if (TutorialController.Instance.count == 13)
                TutorialController.Instance.DoneTutorial();
        }

        isCanAction = false;
        if (isTurnPlayer)
        {
            foreach (Transform dice in dicePlayer)
            {
                DragDrop dragDrop = dice.GetComponentInChildren<DragDrop>();
                if (dragDrop != null)
                    dragDrop.OnDown();
            }
        }
        else
        {
            foreach (Transform dice in diceEnemy)
            {
                DragDrop dragDrop = dice.GetComponentInChildren<DragDrop>();
                if (dragDrop != null)
                    dragDrop.OnDown();
            }
        }

        if (isTurnPlayer)
        {
            foreach (Transform card in cardGird.transform)
            {
                card.GetComponentInChildren<CardSlot>().OnLeft();
            }

            if (_playerController.id == 1)
                _robotController.Init(false);
        }
        else
        {
            foreach (Transform card in cardGird.transform)
            {
                card.GetComponentInChildren<CardSlot>().OnEnemyRight();
            }
        }

        if (isTurnPlayer)
        {
            _playerController.CheckTextInfoEndTurn();
        }

        StartCoroutine(ChangeTurn());
        endturnPanel.SetActive(false);
    }

    private IEnumerator ChangeTurn()
    {
        yield return new WaitForSeconds(1);

        isTurnPlayer = !isTurnPlayer;
        if (isTurnPlayer)
        {
            StopCoroutine(ICreateEnemyDice());
            ChangeTurnPlayer();
            ClosePanelBlock();
        }
        else
        {
            StopCoroutine(ICreatePlayerDice());
            OpenPanelBlock();
            ChangeTurnEnemy();
        }
    }

    private void ChangeTurnPlayer()
    {
        if (!GameManager.Instance.tutorial)
        {
            if (TutorialController.Instance.count >= 14)
                return;
//            if (TutorialController.Instance.count == 15 || TutorialController.Instance.count == 14 ||
//                TutorialController.Instance.count == 16)
//                StartCoroutine(ICreatePlayerDiceRequest(6));
//            else
            StartCoroutine(ICreatePlayerDiceRequest(1));
            TutorialController.Instance.DoneTutorial();
            if (TutorialController.Instance.count == 15)
            {
                StartCoroutine(ICreatePlayerCardRequest("Broadsword"));
            }
            else
                StartCoroutine(ICreatePlayerCardRequest("player0", 5));

//            endturnPanel.SetActive(f);
        }
        else
        {
            StartCoroutine(ICreatePlayerDice());
            StartCoroutine(ICreatePlayerCard());
            endturnPanel.SetActive(true);
        }
    }

    public void ChangeTurnPlayerTutorial()
    {
        StartCoroutine(ICreatePlayerDiceRequest(6));
        StartCoroutine(ICreatePlayerCardRequest("Broadsword"));
    }

    private void ChangeTurnEnemy()
    {
        if (!GameManager.Instance.tutorial)
        {
            StartCoroutine(ICreateEnemyDiceRequest());
            StartCoroutine(ICreateEnemyCard());
        }
        else
        {
            StartCoroutine(ICreateEnemyDice());
            StartCoroutine(ICreateEnemyCard());
        }
    }

    public void ActionCard(int value, CardType cardtype, int valueAttack = 0, bool isConditionEffect = true)
    {
        switch (cardtype)
        {
            case CardType.CardSword:
                _cardController = new SwordCard();
                break;
            case CardType.CardTrap:
                _cardController = new TrapCard();
                break;
            case CardType.CardShield:
                _cardController = new ShieldCard();
                break;
            case CardType.CardChange:
                _cardController = new ChangeCard();
                break;
            case CardType.CardShock:
                _cardController = new ShockCard();
                break;
            case CardType.CardBlind:
                _cardController = new BlindCard();
                break;
            case CardType.CardCurse:
                _cardController = new CurseCard();
                break;
            case CardType.CardLock:
                _cardController = new LockCard();
                break;
            case CardType.CardFreeze:
                _cardController = new FreezeCard();
                break;
            case CardType.CardWeakon:
                _cardController = new WeakenCard();
                break;
            case CardType.CardPoison:
                _cardController = new PoisonCard();
                break;
            case CardType.CardHealth:
                _cardController = new HealthCard();
                break;
            case CardType.CardSteal:
                _cardController = new StealCard();
                break;
            case CardType.CardDodge:
                _cardController = new DodgeCard();
                break;
            case CardType.CardNewDice:
                _cardController = new NewDiceCard();
                break;
            case CardType.CardDrain:
                _cardController = new DrainCard();
                break;
            case CardType.CardReducehealthbyhalf:
                _cardController = new ReducehealthbyhalfCard();
                break;
            case CardType.CardRoll2Dice:
                _cardController = new Roll2DiceCard();
                break;
            case CardType.CardRat:
                _cardController = new RatCard();
                break;
            case CardType.CardPoisonBlind:
                _cardController = new PoisonBlindCard();
                break;
        }

        if (isTurnPlayer)
        {
            _cardController.DoSomething(value, _playerController, _enemyController, valueAttack, isConditionEffect);
            if (_playerController.id == 0 && isUseFury)
            {
                StartCoroutine(FuryPlayer0(value, valueAttack, isConditionEffect));
                isUseFury = false;
            }
        }
        else
        {
            _cardController.DoSomething(value, _enemyController, _playerController, valueAttack, isConditionEffect);
        }
    }

    private IEnumerator FuryPlayer0(int value, int valueAttack, bool isConditionEffect)
    {
        yield return new WaitForSeconds(1);
        _cardController.DoSomething(value, _playerController, _enemyController, valueAttack, isConditionEffect);
    }

    private IEnumerator ICreatePlayerDice()
    {
//        if (dicePlayer.childCount > 0)
//        {
//            foreach (Transform dice in dicePlayer)
//            {
//                dice.SetParent(null);
//            }
//            yield return new WaitForSeconds(0.5f);
//            foreach (Transform dice in dicePlayer)
//            {
//                SmartPool.Instance.Despawn(dice.gameObject);
//            }
//        }
//        yield return new WaitForSeconds(0.5f);
        if (isAutoSpawnDicePlayer)
        {
            playerGrid.enabled = true;
            for (int i = 0; i < numberDicePlayer; i++)
            {
                CreateDicePlayer();
                yield return new WaitForSeconds(0.15f);
            }
            _playerController.CheckEffectDiceFirst();
            yield return new WaitForSeconds(0.75f);
            playerGrid.enabled = false;
            _playerController.CheckEffectDiceAfter();
            _playerController.CheckEffectHurt();
        }
    }

    private IEnumerator ICreatePlayerDiceRequest(int number)
    {
        playerGrid.enabled = true;
        for (int i = 0; i < 1; i++)
        {
            CreateDicePlayer(number);
            yield return new WaitForSeconds(0.15f);
        }

        _playerController.CheckEffectDiceFirst();
        yield return new WaitForSeconds(0.5f);
        playerGrid.enabled = false;
        _playerController.CheckEffectDiceAfter();
        _playerController.CheckEffectHurt();
    }

    public void CreatePlayerDiceRequest(int number)
    {
        StartCoroutine(ICreatePlayerDiceRequest(number));
    }

    private IEnumerator ICreateEnemyDice()
    {
        enemyGrid.enabled = true;
        for (int i = 0; i < numberDiceEnemy; i++)
        {
            CreateDiceEnemy();
            yield return new WaitForSeconds(0.15f);
        }

        _enemyController.CheckEffectDiceFirst();
        yield return new WaitForSeconds(0.5f);
        enemyGrid.enabled = false;
        _enemyController.CheckEffectDiceAfter();
        _enemyController.CheckEffectHurt();
    }

    private IEnumerator ICreateEnemyDiceRequest()
    {
        enemyGrid.enabled = true;
        for (int i = 0; i < 3; i++)
        {
            CreateDiceEnemy(i + 3);
            yield return new WaitForSeconds(0.15f);
        }

        _enemyController.CheckEffectDiceFirst();
        yield return new WaitForSeconds(0.5f);
        enemyGrid.enabled = false;
        _enemyController.CheckEffectDiceAfter();
        _enemyController.CheckEffectHurt();
    }

    public void CreateDicePlayerEffect(int number, int value = 0)
    {
        StartCoroutine(IECreateDicePlayerEffect(number, value));
    }

    private IEnumerator IECreateDicePlayerEffect(int number, int value = 0)
    {
        playerGrid.enabled = true;
        for (int i = 0; i < number; i++)
        {
            CreateDicePlayer(value);
            yield return new WaitForSeconds(0.15f);
        }

        yield return new WaitForSeconds(0.5f);
        playerGrid.enabled = false;
    }

    public void CreateDiceEnemyEffect(int number, int value = 0)
    {
        StartCoroutine(IECreateDiceEnemyEffect(number, value));
    }

    private IEnumerator IECreateDiceEnemyEffect(int number, int value = 0)
    {
        enemyGrid.enabled = true;
        for (int i = 0; i < number; i++)
        {
            CreateDiceEnemy(value);
            yield return new WaitForSeconds(0.15f);
        }

        yield return new WaitForSeconds(0.5f);
        enemyGrid.enabled = false;
    }

    private IEnumerator ICreatePlayerCard()
    {
//        if (card.childCount > 0)
//        {
//            foreach (Transform cardSlot in card)
//            {
//                SmartPool.Instance.Despawn(cardSlot.gameObject);
//            }
//
//            foreach (Transform cardSlot in card)
//            {
//                cardSlot.SetParent(null);
//            }
//        }
        cardGird.enabled = true;
        SetTranformCard(playerCard.Count);
        if (_playerController.id == 1)
            _robotController.Init(true);

        for (int i = 0; i < playerCard.Count; i++)
        {
            CreatePlayerCard(playerCard[i].nameCard);
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(0.25f);
        cardGird.enabled = false;
        _playerController.CheckEffectCard();
        isCanAction = true;
    }

    private IEnumerator ICreatePlayerCardRequest(string name, int time = 0)
    {
        yield return new WaitForSeconds(time);
        cardGird.enabled = true;
        SetTranformCard(1);
        CreatePlayerCard(name);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(0.25f);
        cardGird.enabled = false;
//        _playerController.CheckEffectCard();
        isCanAction = true;
    }

    private IEnumerator ICreateEnemyCard()
    {
//        if (card.childCount > 0)
//        {
//            foreach (Transform cardSlot in card)
//            {
//                SmartPool.Instance.Despawn(cardSlot.gameObject);
//            }
//
//            foreach (Transform cardSlot in card)
//            {
//                cardSlot.SetParent(null);
//            }
//        }
        cardGird.enabled = true;
        SetTranformCard(enemyCard.Count);
        for (int i = 0; i < enemyCard.Count; i++)
        {
            CreateEnemyCard(enemyCard[i].nameCard);
            yield return new WaitForSeconds(0.5f);
        }

        if (_playerController.id == 1)
        {
        }

        yield return new WaitForSeconds(0.25f);
        cardGird.enabled = false;
        _enemyController.CheckEffectCard();
        isCanAction = true;
        AIEnemyAction();
    }

    private void AIEnemyAction()
    {
        _enemyController._enemyAI.Action();
    }


    private void CreateDicePlayer(int value = 0)
    {
        GameObject go = SmartPool.Instance.Spawn(dicePlayerElement,
            Vector3.zero, Quaternion.identity) as GameObject;
        go.transform.SetParent(dicePlayer);
        go.transform.localScale = Vector3.one;
        DragDrop dragDrop = go.GetComponentInChildren<DragDrop>();
        if (value == 0)
            dragDrop.Init();
        else
        {
            dragDrop.SetValueDice(value);
        }
    }

    private void CreateDiceEnemy(int value = 0)
    {
        GameObject go = SmartPool.Instance.Spawn(diceEnemyElement,
            Vector3.zero, Quaternion.identity) as GameObject;
        go.transform.SetParent(diceEnemy);
        go.transform.localScale = Vector3.one;
        DragDrop dragDrop = go.GetComponentInChildren<DragDrop>();
        if (value == 0)
            dragDrop.Init();
        else
        {
            dragDrop.SetValueDice(value);
        }
    }

    private void CreatePlayerCard(string name)
    {
        CardDB cardDb = Resources.Load<CardDB>("DBCard/" + name);
        GameObject go =
            SmartPool.Instance.Spawn(CreateCard(cardDb.cardType.ToString()), Vector3.zero, Quaternion.identity) as
                GameObject;
        go.transform.SetParent(cardGird.transform);
        go.transform.localScale = Vector3.one;
        CardSlot cardSlot = go.GetComponentInChildren<CardSlot>();
        cardSlot.SetData(cardDb);
        cardSlot.Init(true);
    }

    private void CreateEnemyCard(string name)
    {
        CardDB cardDb = Resources.Load<CardDB>("DBCard/" + name);
        GameObject go =
            SmartPool.Instance.Spawn(CreateCard(cardDb.cardType.ToString()), Vector3.zero, Quaternion.identity) as
                GameObject;
        go.transform.SetParent(cardGird.transform);
        go.transform.localScale = Vector3.one;
        CardSlot cardSlot = go.GetComponentInChildren<CardSlot>();
        cardSlot.SetData(cardDb);
        cardSlot.Init(false);
    }

    private void CreateHero()
    {
        GameObject go =
            SmartPool.Instance.Spawn(Resources.Load<GameObject>("CardGame/Hero/Sword"), playerPos.position,
                Quaternion.identity) as GameObject;
        go.transform.SetParent(playerPos);
        go.transform.localScale = Vector2.one;
        _playerController = go.GetComponent<PlayerController>();
        _playerController.SetPlayer(true);
    }

    private void CreateEnemy()
    {
        GameObject go =
            SmartPool.Instance.Spawn(
                Resources.Load<GameObject>("CardGame/Enemy/" + GameManager.Instance.dataEnemy.name), enemyPos.position,
                Quaternion.identity) as GameObject;
        go.transform.SetParent(enemyPos);
        go.transform.localScale = Vector2.one;
        _enemyController = go.GetComponent<PlayerController>();
        _enemyController.SetPlayer(false);
    }

    public GameObject CreateEffect(string name, Vector3 pos)
    {
        GameObject go = SmartPool.Instance.Spawn(Resources.Load<GameObject>("CardGame/Effect/" + name), pos,
            Quaternion.identity);
        return go;
    }

    public GameObject CreateCard(string name)
    {
        GameObject go = Resources.Load<GameObject>("CardGame/Card/" + name);
        return go;
    }

    public void Lose()
    {
        isover = true;
        SmartPool.Instance.Despawn(_playerController.gameObject);
        SmartPool.Instance.Despawn(_enemyController.gameObject);
        _loseWinController.SetText("you lose!");
        ManagerLand.Instance.SetWinorLose(false);
        StartCoroutine(StartSceneGamePlay());
        scoreCard.Clear();
    }

    public void Win()
    {
        SmartPool.Instance.Despawn(_playerController.gameObject);
        SmartPool.Instance.Despawn(_enemyController.gameObject);
        isover = true;
        _loseWinController.SetText("you win!");
        if (temCurrentHealth > 0)
            GameManager.Instance.currentdataplayer.CurrentHealth = temCurrentHealth;
        if (temCurrentFury > 0)
            GameManager.Instance.currentdataplayer.currentFury = temCurrentFury;
        _playerController.AddExp();
        ManagerLand.Instance.SetWinorLose(true);
        StartCoroutine(StartSceneGamePlay());
        scoreCard.Clear();
    }

    private IEnumerator StartSceneGamePlay()
    {
        if (isTurnPlayer)
        {
            foreach (Transform dice in dicePlayer)
            {
                DragDrop dragDrop = dice.GetComponentInChildren<DragDrop>();
                if (dragDrop != null)
                    dragDrop.OnDown();
            }
        }
        else
        {
            foreach (Transform dice in diceEnemy)
            {
                DragDrop dragDrop = dice.GetComponentInChildren<DragDrop>();
                if (dragDrop != null)
                    dragDrop.OnDown();
            }
        }

        if (isTurnPlayer)
        {
            foreach (Transform card in cardGird.transform)
            {
                card.GetComponentInChildren<CardSlot>().OnLeft();
            }
        }
        else
        {
            foreach (Transform card in cardGird.transform)
            {
                card.GetComponentInChildren<CardSlot>().OnEnemyRight();
            }
        }

        animHealthEnemy.SetTrigger("Hide");
        animHealthPlayer.SetTrigger("Hide");
        endturnPanel.SetActive(false);
        yield return new WaitForSeconds(2);

        ManagerLand.Instance.LoadGambitLand();
        SmartPool.Instance.Despawn(_playerController.gameObject);
        SmartPool.Instance.Despawn(_enemyController.gameObject);
    }

    public void SetTranformCard(int number)
    {
        switch (number)
        {
            case 1:
                cardGird = card1;
                break;
            case 2:
                cardGird = card2;
                break;
            case 3:
                cardGird = card3;
                break;
            case 4:
                cardGird = card4;
                break;
        }
    }

    public void UsedFury()
    {
        _playerController._furyController.UsedFury();
        isUseFury = true;
        isFury = false;
        if (_playerController.id == 1)
        {
            isUseFury = false;
            _robotController.OnJackbot();
        }
    }

    public void OpenPanelBlock()
    {
        panelBlock.SetActive(true);
    }

    public void ClosePanelBlock()
    {
        panelBlock.SetActive(false);
    }

    public void WhenBeHit()
    {
        hitCam.StartShaking();
    }

    public void LevelUp()
    {
        _levelUpManager.gameObject.SetActive(true);
    }

    public void RemoveCard(bool isPlayer, string name)
    {
        if (isPlayer)
        {
            for (int i = 0; i < playerCard.Count; i++)
            {
                Debug.Log(playerCard[i].nameCard + "Card" + name);
                if (playerCard[i].nameCard == name)
                {
                    playerCard.RemoveAt(i);
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < enemyCard.Count; i++)
            {
                if (enemyCard[i].nameCard == name)
                {
                    enemyCard.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public void ActiveEndTurn()
    {
        endturnPanel.SetActive(true);
    }

    public void TutorialCount9()
    {
        StartCoroutine(ITutorialCount9());
    }

    private IEnumerator ITutorialCount9()
    {
        foreach (Transform card in cardGird.transform)
        {
            card.GetComponentInChildren<CardSlot>().OnLeft();
        }

        yield return new WaitForSeconds(1);
        StartCoroutine(ICreatePlayerCardRequest("Broadsword", 0));
        TutorialController.Instance.DoneTutorial();
        yield return null;
    }

    public void ErrorCard()
    {
        foreach (Transform card in cardGird.transform)
        {
            CardSlot cd = card.GetComponentInChildren<CardSlot>();
            cd.OnError();
        }
    }

    public void DoDamage(int damage)
    {
        _enemyController.TakeDamage(5);
    }
    public void DoHealth(int health){
        _playerController.AddHealth(health);
    }

    public void DoDice(int number)
    {
        _playerController.GetNewDice(number);
    }
}