using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.UI;

public class JackbotPanelController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Button btnDamage;
    [SerializeField] private Button btnHealth;
    [SerializeField] private Button btnDice;

    private void Awake()
    {
        btnDamage.onClick.AddListener(() => DoDamage(5));
        btnHealth.onClick.AddListener(() => Health(3));
        btnDice.onClick.AddListener(() => Dice(1));
    }

    public void Show()
    {
        gameObject.SetActive(true);
        InteractableButton(true);
    }

    public void Hide()
    {
        anim.SetTrigger("Hide");
    }

    public void Off()
    {
        gameObject.SetActive(false);
    }

    public void DoDamage(int damage)
    {
        CardGamePlayManager.Instance.DoDamage(damage);
        InteractableButton(false);
        Hide();
    }

    public void Health(int health)
    {
        CardGamePlayManager.Instance.DoHealth(health);
        InteractableButton(false);
        Hide();
    }

    public void Dice(int dice)
    {
        CardGamePlayManager.Instance.DoDice(dice);
        InteractableButton(false);
        Hide();
    }

    public void InteractableButton(bool isActive)
    {
        btnDamage.interactable = isActive;
        btnHealth.interactable = isActive;
        btnDice.interactable = isActive;
    }
}