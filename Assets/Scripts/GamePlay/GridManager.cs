using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine.UIElements;

public enum TypeSpawnEnemy
{
    OnePoint,
    Random,
    Range,
}

public enum TypeGrid
{
}

public enum NameEnemy
{
    Monster1,
    Monster2,
    Monster3,
    Monster4,
    Monster5,
    Monster6,
    Pirate1,
    Pirate2,
    Pirate3,
    Pirate4,
    Pirate5,
    Pirate6,
    Pirate7,
    Pirate8,
    Pirate9,
}

public class GridManager : SingletonMono<GridManager>
{
    public Transform spawnHero;
    public Transform spawnEnemy;
    public Vector3[] listPos;
    public Transform[] pointEnemy;
    public TypeSpawnEnemy typeSpawn;
    public GameObject gate;
    public List<EnemyController> listEnemys;
    private EnemyController _enemyController;

    private void Start()
    {
        CreateHero();
//        Reshuffle(listPos);
//        for (int i = 0; i < GetNumberEnemy(); i++)
//        {
//            CreateEnemy(i);
//        }

//        gate.SetActive(false);
    }

    private void CreateEnemy(int i)
    {
        listEnemys = new List<EnemyController>();
        Vector2 pos = new Vector2(listPos[i].x + Random.Range(-listPos[i].z, listPos[i].z),
            listPos[i].y + Random.Range(-listPos[i].z, listPos[i].z));
        GameObject go = SmartPool.Instance.Spawn(Resources.Load<GameObject>("GamePlay/Enemy/" + GetNameEnemy()),
            pos,
            Quaternion.identity);
        listEnemys.Add(go.GetComponent<EnemyController>());
    }

    private int GetNumberEnemy()
    {
        if (GameManager.Instance.levelMap == -1)
        {
            return 1;
        }
        else
        {
            return Random.Range(2, 4);
        }
    }

    public void SetEnemy(EnemyController enemyController)
    {
        this._enemyController = enemyController;
    }

    private string GetNameEnemy()
    {
        string name = "";
        if (GameManager.Instance.levelMap == -1)
        {
            name = "Wizzard";
        }
        else
        {
            var values = Enum.GetValues(typeof(NameEnemy));
            name = values.GetValue(Random.Range(0, values.Length)).ToString();
        }

        name = "Pirate5";
        return name;
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

    public void Win()
    {
        bool openGate = true;
        Debug.Log(GameManager.Instance.levelMap+"Levelmap");
        if (GameManager.Instance.levelMap != -1)
        {
            this._enemyController.Die();
            Debug.Log("Co vao day khong");
            listEnemys.Remove(this._enemyController);
            for (int i = 0; i < listEnemys.Count; i++)
            {
                if (!listEnemys[i].iscantAttack)
                    openGate = false;
            }
        }

        if (openGate)
        {
            gate.SetActive(true);
            listEnemys.Clear();
        }
    }

    private void Reshuffle(Vector3[] texts)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < texts.Length; t++)
        {
            Vector3 tmp = texts[t];
            int r = Random.Range(t, texts.Length);
            texts[t] = texts[r];
            texts[r] = tmp;
        }
    }
}