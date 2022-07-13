using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class BoardSetting
{
    public int xSize, ySize;
    public int[] map;
    public List<Vector3> wayPoints;
}

[System.Serializable]
public class Tower
{
    public string towerType;
    public int towerCost;
    public int towerDamage;
    public int towerCooldown;
    public int towerRange;
    public GameObject towerPref;
}

[System.Serializable]
public class Enemy
{
    public string enemyType;
    public int enemyHP;
    public float enemySpeed;
    public int enemyGold;
    public GameObject enemyPref;
}

[System.Serializable]
public class EnemySpawn
{
    public string enemyType;
    public int enemySpawnWaiting;
    public int enemyMaxCount;
    public int enemyCount;
    public int enemySpawnTime;
    public float enemyTimeCount;
}

[System.Serializable]
public class PrefabList
{
    public string name;
    public GameObject pref;
}

public class GameManager : MonoBehaviour
{
    private List<Vector3> wayPoints = new List<Vector3>();
    public GameObject board;
    public Transform enemySpawner;
    public Transform towerParent;
    private ShopController shopController;
    private string pathRoot;
    private const string boardJSON = "/BoardSettingsData.json";
    private const string towerJSON = "/TowerData.json";
    private const string enemyJSON = "/EnemyData.json";
    private const string enemySpawnJSON = "/EnemySpawnData.json";

    private BoardSetting boardSetting;

    private List<Tower> towerList = new List<Tower>();

    private List<Enemy> enemyList = new List<Enemy>();

    private List<EnemySpawn> enemySpawnList = new List<EnemySpawn>();
    
    public List<PrefabList> prefList;
    public static float mark = 0f;

    IEnumerator Start()
    {
        pathRoot = Application.streamingAssetsPath;
        yield return StartCoroutine(GetWayPoints());

        board.transform.Rotate(45.0f, 0.0f, 45.0f, Space.Self);
        LoadData(towerJSON);
        LoadData(enemyJSON);
        LoadData(enemySpawnJSON);
        shopController = gameObject.GetComponent<ShopController>();
        shopController.SetValue(towerList);
        yield return StartCoroutine(StartSpawn());
    }
    
    IEnumerator GetWayPoints()
    {
        LoadData(boardJSON);
        board.GetComponent<Board>().SetValue(boardSetting.xSize, boardSetting.ySize, boardSetting.map);
        wayPoints = boardSetting.wayPoints;
        yield return null;
    }

    IEnumerator StartSpawn()
    {
        enemySpawner.gameObject.GetComponent<EnemySpawner>().SetValue(enemySpawnList);  
        yield return null;
    }

    public void InstanceTower(string towerType, Vector2 pos)
    {
        foreach (Tower tower in towerList)
            if (tower.towerType == towerType)
            {
                GameObject towerGO = Instantiate(tower.towerPref, pos, Quaternion.identity, towerParent);
                towerGO.GetComponent<TowerController>().SetValue(tower, enemySpawner);  
            }
    }

    public void InstanceEnemy(string enemyType)
    {
        if (wayPoints.Count > 1)
        {
            Vector3 boardPos = wayPoints[0];
            Vector3 enemyPos = new Vector3(boardPos.x, boardPos.y+0.4f, 0);
            foreach (Enemy enemy in enemyList)
                if (enemy.enemyType == enemyType)
                {
                    GameObject enemyGO = Instantiate(enemy.enemyPref, enemyPos, Quaternion.identity, enemySpawner);
                    enemyGO.GetComponent<EnemyController>().SetValue(enemy, wayPoints, shopController);  
                }
        }
    }

    private void LoadData(string pathName)
    {
        string path = pathRoot + pathName;
        var jsonString = File.ReadAllText(path);

        if (pathName == enemyJSON)
        {
            Enemy[] data = JsonHelper.FromJson<Enemy>(jsonString);
            foreach(Enemy child in data)
            {
                foreach(PrefabList childPref in prefList)
                    if (child.enemyType == childPref.name)
                        child.enemyPref = childPref.pref;
                enemyList.Add(child);
            }
        }
        else if (pathName == enemySpawnJSON)
        {
            EnemySpawn[] data = JsonHelper.FromJson<EnemySpawn>(jsonString);
            foreach(EnemySpawn child in data)
                enemySpawnList.Add(child);            
        }
        else if (pathName == towerJSON)
        {
            Tower[] data = JsonHelper.FromJson<Tower>(jsonString);
            foreach(Tower child in data)
            {
                foreach(PrefabList childPref in prefList)
                    if (child.towerType == childPref.name)
                        child.towerPref = childPref.pref;
                towerList.Add(child);
            }            
        }
        else if (pathName == boardJSON)
        {
            BoardSetting[] boardSettingsList = JsonHelper.FromJson<BoardSetting>(jsonString);
            boardSetting = boardSettingsList[0];
        }
    }

    public void BackToPractice()
	{
		SceneManager.LoadSceneAsync("Practice");
	}

    public void Restart()
	{
        mark += 1f;
		SceneManager.LoadSceneAsync("Gameplay");
	}

    public void Pause()
	{
		
	}

    public void Play()
	{
		
	}

    public void SaveBoardSettings(BoardSetting newData)
    {
        string pathToSave = pathRoot + boardJSON;
        /*var jsonString = File.ReadAllText(pathToSave);
        BoardSetting[] data = JsonHelper.FromJson<BoardSetting>(jsonString);
        Array.Resize(ref data, data.Length + 1);
        data[data.Length] = newData;*/
        BoardSetting[] data = new BoardSetting[1];
        data[0] = newData;
        string json = JsonHelper.ToJson(data, true);
        File.WriteAllText(pathToSave, json);
    }

    public class JsonHelper 
    {
        public static T[] FromJson<T>(string json) 
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array) 
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint) 
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [System.Serializable]
        private class Wrapper<T> 
        {
            public T[] Items;
        }
    }
}
