using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMono<GameManager>
{
    //Data cua nhan vat hien tai
    public DataPlayer currentdataplayer;

//DataEnemy de chuyen sang gambit
    public EnemyData dataEnemy;
    // TODO so nhan vat da so huu
    
    //Level hien tai cua lan choi nay
    public int levelMap = 0;
    public int oldLevelMap;
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        List<Card> c = new List<Card>();
        c.Add(new Card(CardType.CardSword,0,0));
        c.Add(new Card(CardType.CardChange,0,0));
        currentdataplayer=new DataPlayer(0,1,0,0,c,0);
        oldLevelMap = levelMap;
    }

    public void SetDataEnemy(EnemyData dataEnemy)
    {
        this.dataEnemy = dataEnemy;
    }
    public EnemyData LoadDataEnemy(string name)
    {
        return Resources.Load<EnemyData>("DB/" + name);
    }
}