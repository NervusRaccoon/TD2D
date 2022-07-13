using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	private List<Vector3> wayPoints;
	private int wayIndex = 0;
	private Enemy enemySettings;
	public int enemyHP;
	private ShopController shopController;
	private bool stop = false;
	private Transform enemyTransform; 
	private Animator enemyAnimator;
	private SpriteRenderer enemySprRenderer;

	private void Awake()
    {
        //instance = this;
		enemyTransform = transform;
		enemyAnimator = gameObject.GetComponent<Animator>();
		enemySprRenderer = gameObject.GetComponent<SpriteRenderer>();
  	}

	public void SetValue(Enemy enemySettings, List<Vector3> wayPoints, ShopController shopController)
    {
        this.enemySettings = enemySettings;
		this.wayPoints = wayPoints;
		this.enemyHP = enemySettings.enemyHP;
		this.shopController = shopController;
    }

    void Update()
    {
		if (!stop)
			Move();
		CheckIsAlive();
    }

	private void Move()
	{
		Vector2 currWayPos = new Vector2(wayPoints[wayIndex].x, wayPoints[wayIndex].y+0.4f);	
		enemyTransform.position = Vector3.MoveTowards(enemyTransform.position, currWayPos, enemySettings.enemySpeed * Time.deltaTime);
		//enemyTransform.position = Vector3.Lerp(enemyTransform.position, currWayPos, enemySettings.enemySpeed * Time.deltaTime);

		if (Vector3.Distance(transform.position, currWayPos) < 0.1f) 
		{
			if (wayIndex < wayPoints.Count - 1)
			{
				wayIndex++;
				//rotate enemy
				Vector2 diff = new Vector2(wayPoints[wayIndex-1].x - wayPoints[wayIndex].x, 
											wayPoints[wayIndex-1].y - wayPoints[wayIndex].y);

				if ((diff.x > 0 && diff.y < 0) || (diff.x > 0 && diff.y > 0))
					enemyTransform.rotation = Quaternion.Euler(0, 180, 0);	
				if ((diff.x < 0 && diff.y < 0) || (diff.x < 0 && diff.y > 0))
					enemyTransform.rotation = Quaternion.Euler(0, 0, 0);
			}
			else 
			{
				shopController.HealthOperation();
				Destroy(gameObject);
			}
		}
	}

	public void TakeDamage(int damage)
	{
		StartCoroutine(EnemyHitReaction());
		enemyHP -= damage;
	}

	public void TakeFear(int damage, float time)
	{
		StartCoroutine(EnemyFear(time));
		enemyHP -= damage;
	}

	private void CheckIsAlive()
	{
		if (enemyHP <= 0) 
		{
			shopController.MoneyOperation(enemySettings.enemyGold);
			Destroy(gameObject);
		}
	}
	IEnumerator EnemyHitReaction()
	{
		stop = true;
		enemySprRenderer.color = Color.red;
		enemyAnimator.SetBool("TakeDamage", true);
		yield return new WaitForSeconds(0.2f);
		enemySprRenderer.color = Color.white;
		enemyAnimator.SetBool("TakeDamage", false); 
		stop = false;
	}

	IEnumerator EnemyFear(float time)
	{
		stop = true;
		enemyAnimator.SetBool("TakeDamage", true); 
		yield return new WaitForSeconds(time);
		enemyAnimator.SetBool("TakeDamage", false); 
		stop = false;
	}
}
