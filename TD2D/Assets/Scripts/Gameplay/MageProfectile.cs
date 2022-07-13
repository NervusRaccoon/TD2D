using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageProfectile : ProjectileController
{
    private void Start()
    {
        speed = 3;
    }

    override public void EnemyInteraction(Transform enemy)
    {
        enemy.GetComponent<EnemyController>().TakeFear(damage, 2f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            EnemyInteraction(col.transform);
        }
    }
}
