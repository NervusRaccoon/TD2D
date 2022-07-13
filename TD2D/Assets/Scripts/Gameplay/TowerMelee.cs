using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMelee : TowerController
{
	override public IEnumerator Hit(Transform enemy)
	{
		cooldown = towerSettings.towerCooldown;
		gameObject.GetComponent<Animator>().SetBool("hit", true); 
		yield return new WaitForSeconds(0.8f);
		gameObject.GetComponent<Animator>().SetBool("hit", false);
		if (enemy != null)
			enemy.GetComponent<EnemyController>().TakeDamage(towerSettings.towerDamage);
	}
}
