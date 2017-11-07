using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomEnemy : MonoBehaviour
{
	public float strength;
	public float speed;
	public Rigidbody2D rig;

	public Vector2 targetDistBounds;

	public Vector2 oldVelocity;

	void Start()
	{
		BoomEnemyManager.Instance.RegisterEnemy(this);
		Vector2 force = BoomPlayer.Instance.transform.position - transform.position;
		float sign = 1.0f;
		if (Random.Range(0, 100.0f) < 50.0f)
		{
			sign = -1.0f;
		}

		force = force + (new Vector2(-force.normalized.y, force.normalized.x)).normalized * BoomPlayer.Instance.transform.localScale.x * Random.Range(targetDistBounds.x, targetDistBounds.y) * sign;
		force = force.normalized * speed * BoomPlayer.Instance.transform.localScale.x * Random.Range(0.5f, 1.5f);
		rig.AddForce(force, ForceMode2D.Impulse);
	}

	void Update()
	{
		if (BoomEnemyManager.Instance.IsRunning() == false && oldVelocity != Vector2.zero)
		{
			rig.velocity = Vector2.zero;
		}
		else if (BoomEnemyManager.Instance.IsRunning() && rig.velocity == Vector2.zero)
		{
			rig.velocity = oldVelocity;
		}
		else if (BoomEnemyManager.Instance.IsRunning())
        {
			oldVelocity = rig.velocity;
		}


		if (transform.position.magnitude > BoomPlayer.Instance.transform.localScale.x * 20.0f)
		{
			Destroy(this);
		}
	}

	void OnDestroy()
	{
		BoomEnemyManager.Instance.UnregisterEnemy(this);
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawLine(transform.position, transform.position + new Vector3(oldVelocity.x, oldVelocity.y, 0.0f));
	}
}
