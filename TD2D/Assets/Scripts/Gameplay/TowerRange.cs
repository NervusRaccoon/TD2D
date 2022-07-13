using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRange : TowerController
{
    public GameObject projPref;

	override public IEnumerator Hit(Transform enemy)
	{
		cooldown = towerSettings.towerCooldown;
		gameObject.GetComponent<Animator>().SetBool("hit", true); 
		yield return new WaitForSeconds(0.8f);
		gameObject.GetComponent<Animator>().SetBool("hit", false);
		if (enemy != null)
			CreateProjectile(enemy);
	}

    private void CreateProjectile(Transform enemy)
    {
        GameObject proj = Instantiate(projPref, towerTransform.position, towerTransform.rotation, towerTransform);
        proj.GetComponent<ProjectileController>().SetValue(enemy, towerSettings.towerDamage);
    }
}
