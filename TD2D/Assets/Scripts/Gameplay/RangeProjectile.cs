using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeProjectile : ProjectileController
{
    private void Start()
    {
        speed = 5;
    }

    override public void EnemyInteraction(Transform enemy)
    {
        enemy.GetComponent<EnemyController>().TakeDamage(damage);
    }
}
