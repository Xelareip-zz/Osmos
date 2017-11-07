using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomPlayer : MonoBehaviour
{
	private static BoomPlayer instance;
	public static BoomPlayer Instance
	{
		get
		{
			return instance;
		}
	}

	public GameObject endGame;

	public BoomWave wave;
	public float chargeSpeed;
	public float reduceSpeed;

	public bool charging;
	public float power;

	public float strength;
	public float score;

	void Awake()
	{
		charging = false;
		instance = this;
	}

	void Update()
	{
		if (BoomEnemyManager.Instance.IsRunning() == false)
		{
			return;
		}

		bool wasCharging = charging;
		if (Input.GetMouseButton(0) || Input.touchCount > 0)
		{
			charging = true;
			power += Time.deltaTime * chargeSpeed;
		}
		else
		{
			charging = false;
		}

		wave.targetSize = power;

		if (!charging && wasCharging)
		{
			Attack();
		}

		ZoomOut();

		ScoreManager.Instance.score = Mathf.FloorToInt(score);
	}

	private void ZoomOut()
	{
		if (transform.localScale.x <= 1.0f)
		{
			return;
		}

		float reduceVal = Mathf.Min(Mathf.Max(transform.localScale.x - 1.0f, 1.0f) * Time.deltaTime * reduceSpeed, transform.localScale.x - 1.0f);
		float proportionalReduction = reduceVal / transform.localScale.x;

		transform.localScale = (transform.localScale.x - reduceVal) * Vector3.one;
		wave.transform.localScale = (wave.transform.localScale.x - reduceVal) * Vector3.one;

		for (int enemyIdx = 0; enemyIdx < BoomEnemyManager.Instance.enemies.Count; ++enemyIdx)
		{
			BoomEnemy currentEnemy = BoomEnemyManager.Instance.enemies[enemyIdx];

            if (currentEnemy == null)
			{
				continue;
			}

			if (currentEnemy.transform.localScale.x - reduceVal < 0.05f)
			{
				Destroy(currentEnemy.gameObject);
			}
			else
			{
				currentEnemy.transform.localScale -= currentEnemy.transform.localScale * proportionalReduction;

				currentEnemy.transform.position -= currentEnemy.transform.position * proportionalReduction;
				if (currentEnemy.rig != null)
				{
					currentEnemy.rig.velocity -= currentEnemy.rig.velocity * proportionalReduction;
				}
            }
		}
	}

	public void EndGame()
	{
		BoomEnemyManager.Instance.gameOver = true;
		endGame.SetActive(true);
	}

	private void Attack()
	{
		wave.enemiesInRange.Sort((e0, e1) =>
		{
			return (e0.transform.position.magnitude - e0.transform.localScale.x / 2.0f > e1.transform.position.magnitude - e1.transform.localScale.x / 2.0f) ? -1 : 1;
        });

		StartCoroutine(AbsorbEnemies(wave.enemiesInRange));

	}

	private IEnumerator AbsorbEnemies(List<BoomEnemy> enemiesAbsorbed)
	{
		BoomEnemyManager.Instance.pause = true;
        while (enemiesAbsorbed.Count > 0)
		{
			BoomEnemy enemy = enemiesAbsorbed[enemiesAbsorbed.Count - 1];

			float strengthIncrease = 1.0f + (enemy.strength / strength);
			enemiesAbsorbed.Remove(enemy);
			DestroyImmediate(enemy.rig);
			DestroyImmediate(enemy.GetComponent<Collider2D>());
			enemy.enabled = false;
			Vector3 toGo = transform.position - enemy.transform.position;
			float enemySpeed = toGo.magnitude / 0.2f;
			while (toGo != Vector3.zero && toGo.magnitude > (transform.localScale.x - enemy.transform.localScale.x) / 2.0f)
			{
				yield return new WaitForEndOfFrame();
				enemy.transform.position += toGo.normalized * Mathf.Min(toGo.magnitude, enemySpeed * Time.deltaTime);
				toGo = transform.position - enemy.transform.position;
			}

			if (enemy.strength <= strength)
			{
				yield return new WaitForSeconds(0.1f);
				score += (strengthIncrease - 1.0f) * 10.0f;
				transform.localScale *= strengthIncrease;
				strength *= strengthIncrease;
				Destroy(enemy.gameObject);
			}
			else
			{
				EndGame();
				enemiesAbsorbed.Remove(enemy);
				enemy.gameObject.transform.localScale *= strengthIncrease;
				GetComponentInChildren<Renderer>().enabled = false;
				break;
			}
		}
		power = 0.0f;
		BoomEnemyManager.Instance.pause = false;
		yield return null;
	}
}
