using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomWave : MonoBehaviour
{
	public float targetSize;
	public float speed;
	public float damage;

	public List<BoomEnemy> enemiesInRange;

	void Update()
	{
		transform.localScale = Vector3.one * (targetSize);
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.tag == "Enemy")
		{
			BoomEnemy enemy = coll.GetComponent<BoomEnemy>();
			enemiesInRange.Add(enemy);
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.tag == "Enemy")
		{
			BoomEnemy enemy = coll.GetComponent<BoomEnemy>();
			enemiesInRange.Remove(enemy);
		}
	}


}
