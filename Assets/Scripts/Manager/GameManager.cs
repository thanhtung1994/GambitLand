using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : SingletonMono<GameManager>
{
    //Data cua nhan vat hien tai
    public DataPlayer currentdataplayer;

//DataEnemy de chuyen sang gambit
    public EnemyData dataEnemy;

    // TODO so nhan vat da so huu
    public PlayerData _playerData;

    //Level hien tai cua lan choi nay
    public bool tutorial;
    public UnityAction _updateHealthBarGamePlay;
    public int levelMap
    {
        get { return currentdataplayer.currentLevelMap; }
    }

    private void Awake()
    {
//        DontDestroyOnLoad(this.gameObject);
        List<Card> c = new List<Card>();
//        SaveManager.Del();
        int intTurotial = PlayerPrefs.GetInt("Tutorial", 0);
        if (intTurotial == 0)
            tutorial = false;
        else
            tutorial = true;
        c.Add(new Card("Broadsword", 0, 0));
        if (tutorial)
            c.Add(new Card("player0", 0, 0));
        if (SaveManager.CheckData())
        {
            currentdataplayer = SaveManager.Load();
            _playerData = LoadDataHero(currentdataplayer.name);
        }
        else
        {
            _playerData = LoadDataHero("Boy");
            currentdataplayer = new DataPlayer(0, 0, _playerData.maxHP[0], tutorial ? 0 : -1, c, 0);
        }
    }
    public void SetDataEnemy(EnemyData dataEnemy)
    {
        this.dataEnemy = dataEnemy;
    }

    public EnemyData LoadDataEnemy(string name)
    {
        return Resources.Load<EnemyData>("DBEnemy/" + name);
    }

    public void NextMap()
    {
        currentdataplayer.currentLevelMap++;
        Save();
    }
    
    public int GetMaxLevelUp()
    {
        return _playerData.expToLevelUp[currentdataplayer.level];
    }

    public int GetMaxHealth()
    {
        return _playerData.maxHP[currentdataplayer.level];
    }

    public void LevelUp()
    {
        currentdataplayer.level++;
        currentdataplayer.SetHealth(_playerData.maxHP[currentdataplayer.level]);
    }

    public void Save()
    {
        SaveManager.Save(currentdataplayer);
    }
    private void OnApplicationQuit()
    {
//        SaveManager.Save(currentdataplayer);
    }

    public PlayerData LoadDataHero(string name)
    {
        return Resources.Load<PlayerData>("DBHero/" + name);
    }
}