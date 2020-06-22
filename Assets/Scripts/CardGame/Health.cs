using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public enum TypeHealthBar
{
    PlayerHealthBar,
    EnemyHealthBar,
    FuryBar,
}

public enum TypeShield
{
    PlayerShield,
    EnemyShield,
}

public class Health : MonoBehaviour
{
    public TypeHealthBar typeHealthBar;
    public TypeShield typeShield;
    public bool isPlayer;

    public int currentHealth;
    public int initialHealth;
    public int maximumHealth;

    [MMReadOnly] public int currentsheild;
    public UnityAction onHit;
    public UnityAction onRevive;
    public UnityAction onDeath;
    private MMProgressBar _healthBar;
    private ShieldController _shieldController;
    public TextInfoController _textInfoController;
    [SerializeField] private ParticleSystem hit;

    [SerializeField] private TextMeshProUGUI textDice;
//    protected void Start()
//    {
//        if (!isPlayer)
//        {
//            maximumHealth = GameManager.Instance.dataEnemy.maxhealth;
//            currentHealth = maximumHealth;
//        }
//        else
//        {
//            maximumHealth = GameManager.Instance.GetMaxHealth();
//            currentHealth = GameManager.Instance.currentdataplayer.currentHealth;
//        }
//
//        _healthBar = GameObject.FindGameObjectWithTag(typeHealthBar.ToString()).GetComponent<MMProgressBar>();
//        _textInfoController = GameObject.FindGameObjectWithTag(typeHealthBar.ToString())
//            .GetComponent<TextInfoController>();
//        SetTextInfo("");
//        UpdateHealthBar();
//        FindShieldController();
//    }
    public void Init()
    {
        if (!isPlayer)
        {
            maximumHealth = GameManager.Instance.dataEnemy.maxhealth;
            currentHealth = maximumHealth;
        }
        else
        {
            maximumHealth = GameManager.Instance.GetMaxHealth();
            currentHealth = GameManager.Instance.currentdataplayer.CurrentHealth;
        }

        _healthBar = GameObject.FindGameObjectWithTag(typeHealthBar.ToString()).GetComponent<MMProgressBar>();
        _textInfoController = GameObject.FindGameObjectWithTag(typeHealthBar.ToString())
            .GetComponent<TextInfoController>();
        SetTextInfo("");
        UpdateHealthBar();
        FindShieldController();
      
    }

    public void SetTextInfo(string text)
    {
        _textInfoController.SetText(text);
    }

    private void FindShieldController()
    {
        _shieldController = GameObject.FindGameObjectWithTag(typeShield.ToString()).GetComponent<ShieldController>();
        _shieldController.gameObject.SetActive(false);
    }

    public virtual void TakeDamage(int damage, bool activeHitEffect = true, bool isSheild = true)
    {
        if (currentHealth <= 0 || damage <= 0)
        {
            return;
        }

        _textInfoController.SetTextDamage(damage);
        if (currentsheild > 0 && isSheild)
        {
            if (currentsheild >= damage)
            {
                currentsheild -= damage;
                _shieldController.ShowShield(currentsheild);
            }
            else
            {
                int damageMinus = damage - currentsheild;
                currentHealth -= damageMinus;
                currentsheild = 0;
                _shieldController.HideShield();
            }
        }
        else
        {
            currentHealth -= damage;
        }

        if (onHit != null)
        {
            onHit.Invoke();
        }

        if (isPlayer)
            CardGamePlayManager.Instance.temCurrentHealth = currentHealth;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        UpdateHealthBar();
        if (activeHitEffect)
            hit.Play();
        CardGamePlayManager.Instance.WhenBeHit();
    }

    public void ReduceHealthHalf()
    {
        currentHealth = (int) currentHealth / 2;
        UpdateHealthBar();
        hit.Play();
        CardGamePlayManager.Instance.WhenBeHit();
    }

    public void AddShield(int number)
    {
        currentsheild += number;
        _shieldController.ShowShield(currentsheild);
    }

    public void AddHealth(int number)
    {
        currentHealth += number;

        UpdateHealthBar();
    }

    protected void UpdateHealthBar()
    {
        if (currentHealth > maximumHealth)
            currentHealth = maximumHealth;
        _healthBar.UpdateBar(currentHealth, 0f, maximumHealth);
        _textInfoController.SetTextDamageHealthBar(currentHealth, maximumHealth);
    }

    private void Die()
    {
        if (isPlayer)
        {
            CardGamePlayManager.Instance.Lose();
        }
        else
        {
            CardGamePlayManager.Instance.Win();
        }
    }


}