using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
	public static TowerController instance;
	private Transform enemySpawner;
	protected float cooldown;
	private Vector3 towerPos;
	protected Transform towerTransform;
	public Tower towerSettings;

	private void Awake()
    {
        //instance = this;
		towerPos = new Vector3(transform.position.x, transform.position.y - 0.6f, transform.position.z);
		towerTransform = gameObject.transform;
    }

	public void SetValue(Tower towerSettings, Transform enemySpawner)
    {
        this.towerSettings = towerSettings;
		this.enemySpawner = enemySpawner;
		this.cooldown = towerSettings.towerCooldown;
    }

	private void Update()
	{
		if (cooldown <= 0)
		{
			SearchTarget();
		}
		if (cooldown >= 0)
			cooldown -= Time.deltaTime;
	}

	private void SearchTarget()
	{
		Transform nearestEnemy = null;
		float enemyDistance = Mathf.Infinity;

		foreach(Transform enemy in enemySpawner)
		{
			float currDistance = Vector2.Distance(towerPos, enemy.position);
			if (currDistance < enemyDistance)
			{
				//rotate to enemy

				Vector2 diff = new Vector2(enemy.transform.position.x - towerPos.x, 
							enemy.transform.position.y - towerPos.y);

				if ((diff.x > 0 && diff.y < 0) || (diff.x > 0 && diff.y > 0))
					towerTransform.rotation = Quaternion.Euler(0, 0, 0);	
				if ((diff.x < 0 && diff.y < 0) || (diff.x < 0 && diff.y > 0))
					towerTransform.rotation = Quaternion.Euler(0, 180, 0);
					
				//hit enemy
				if (currDistance <= towerSettings.towerRange)
				{
					nearestEnemy = enemy;
					enemyDistance = currDistance;
				}
			}

		}
		if (nearestEnemy != null)
			StartCoroutine(Hit(nearestEnemy));
	}

	virtual public IEnumerator Hit(Transform enemy)
	{
		yield return null;
	}
}
