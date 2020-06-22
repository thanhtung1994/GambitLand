using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelManagerGamePLay : SingletonMono<LevelManagerGamePLay>
{
    [SerializeField] private GameObject panelBlock;
    [SerializeField] private GameObject joystick;
    public Transform posSpawn;
    private Collider _collider;
    public Collider BoundsCollider { get; protected set; }
    [SerializeField] private MMProgressBar _healthBar;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        BoundsCollider = _collider;
        CreateGrid();
        MMCameraEvent.Trigger(MMCameraEventTypes.SetConfiner, null, BoundsCollider);
//            CreateHero();
//            CreateEnemy();
        GameManager.Instance._updateHealthBarGamePlay += UpdateHealthBar;
    }

    private void OnEnable()
    {
        UpdateHealthBar();
    }

    private void CreateGrid()
    {
//            int level = GameManager.Instance.levelMap == 0 ? 0 : Random.Range(0, 10);
        //TODO de tam nhu nay vi sau lam nhieu map se tao random map
        int level;
        level = GameManager.Instance.levelMap;
        GameObject go =
            SmartPool.Instance.Spawn(Resources.Load<GameObject>("Grid/GamePlay/" + level), Vector3.zero,
                Quaternion.identity);
    }

    private void CreateHero()
    {
        GameObject go = SmartPool.Instance.Spawn(Resources.Load<GameObject>("GamePlay/Hero/Boy"),
            GridManager.Instance.spawnHero.position,
            Quaternion.identity);
        Character character = go.GetComponent<Character>();
        TopDownEngineEvent.Trigger(TopDownEngineEventTypes.LevelStart, null);
        MMGameEvent.Trigger("Load");
        MMCameraEvent.Trigger(MMCameraEventTypes.SetTargetCharacter, character);
        MMCameraEvent.Trigger(MMCameraEventTypes.StartFollowing);
//        MMGameEvent.Trigger("CameraBound");
    }

    private void CreateEnemy()
    {
//            GameObject go = SmartPool.Instance.Spawn(Resources.Load<GameObject>("GamePlay/Enemy/Ninja"),
//                GridManager.Instance.spawnEnemy.position,
//                Quaternion.identity);
    }

    public void PanelBlock(bool isBlock)
    {
        panelBlock.SetActive(isBlock);
    }

    public void ActiveJoystick(bool isTrue)
    {
        joystick.SetActive(isTrue);
    }

    public void CutScene()
    {
        PanelBlock(true);
        ActiveJoystick(false);
    }

    public void EndCutScene()
    {
        PanelBlock(false);
        ActiveJoystick(true);
    }

    public GameObject CreateEffect(string name, Vector3 pos)
    {
        GameObject go = SmartPool.Instance.Spawn(Resources.Load<GameObject>("GamePlay/Effect/" + name), pos,
            Quaternion.identity);
        return go;
    }

    public void PlayAgain()
    {
        SaveManager.Del();
        SceneManager.LoadScene("Preloader");
    }

    public void UpdateHealthBar()
    {
        Debug.Log(GameManager.Instance.currentdataplayer.CurrentHealth+"CurrentHealth"+GameManager.Instance.GetMaxHealth());
        _healthBar.UpdateBar(GameManager.Instance.currentdataplayer.CurrentHealth,0,GameManager.Instance.GetMaxHealth());
    }
}