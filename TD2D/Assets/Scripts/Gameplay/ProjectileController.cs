using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
	protected Transform target;
	protected int damage;
    protected float speed;
	private Transform projTransform;

    void Update()
    {
		Move ();
    }
	public void SetValue(Transform enemy, int dmg)
	{
		damage = dmg;
		target = enemy;
		projTransform = gameObject.transform;
	}

	private void Move()
	{
		if (target != null) 
		{	
			if (Vector2.Distance (transform.position, target.position) < .1f) 
			{
                EnemyInteraction(target);
				Destroy(gameObject);
			}
			else 
			{
				projTransform.position = Vector3.MoveTowards(projTransform.position, target.transform.position, speed * Time.deltaTime);
			}
		} 
		else
			Destroy(gameObject);
	}

    virtual public void EnemyInteraction(Transform enemy)
    {
    }
}
