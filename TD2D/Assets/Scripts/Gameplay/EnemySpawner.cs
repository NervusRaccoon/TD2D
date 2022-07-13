using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public GameManager gameManager;
    private float countTime = 0;
	private List<EnemySpawn> enemySpawnList;
	private Transform spawnTransform;
	public ResultController result;
	private bool win = false;

	private void Awake()
    {
		spawnTransform = gameObject.transform;
        //instance = this;
    }

	public void SetValue(List<EnemySpawn> enemySpawnList)
    {
        this.enemySpawnList = enemySpawnList;
    }
    
	public bool CheckWin()
	{
		bool win = false;
		if (enemySpawnList != null && spawnTransform.childCount == 0)
		{
			win = true;
			foreach (EnemySpawn spawnSettings in enemySpawnList)
				if (spawnSettings.enemyCount < spawnSettings.enemyMaxCount)
					win = false;
		}

		return win;
	}

    void Update()
    {
		if (enemySpawnList != null)
			foreach (EnemySpawn spawnSettings in enemySpawnList)
			{
				if (spawnSettings.enemyCount < spawnSettings.enemyMaxCount) 
				{
					if (spawnSettings.enemySpawnWaiting <= countTime)
					{
						if (spawnSettings.enemyTimeCount <= 0)
						{
							spawnSettings.enemyCount++;
							gameManager.InstanceEnemy(spawnSettings.enemyType);
							spawnSettings.enemyTimeCount = spawnSettings.enemySpawnTime;
						}
					}
					else
						spawnSettings.enemyTimeCount = spawnSettings.enemySpawnTime;
				} 
				spawnSettings.enemyTimeCount -= Time.deltaTime;
			}

		countTime += Time.deltaTime;
		if (CheckWin() && !win)
		{
			win = true;
			result.Result();
		}
    }
}
