using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomEnemyManager : MonoBehaviour
{
	private static BoomEnemyManager instance;
	public static BoomEnemyManager Instance
	{
		get
		{
			return instance;
		}
	}

	public GameObject enemyModel;

	public float spawnDist;
	public List<BoomEnemy> enemies = new List<BoomEnemy>();
	public List<float> scales;
	public float spawnInterval;
	public float spawnDistanceVariation;

	public bool gameOver;

	public bool pause;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		StartCoroutine(SpawnAtInterval());
	}

	public void RegisterEnemy(BoomEnemy enemy)
	{
		enemies.Add(enemy);
	}

	public void UnregisterEnemy(BoomEnemy enemy)
	{
		enemies.Remove(enemy);
	}

	public bool IsRunning()
	{
		return !pause && !gameOver;
	}

	IEnumerator SpawnAtInterval()
	{
		while(true)
		{
			yield return new WaitForSeconds(spawnInterval);
			if (IsRunning())
			{
				Spawn();
			}
		}
	}

	public void Spawn()
	{
		GameObject newEnemy = Instantiate(enemyModel,
			Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.forward) * new Vector3(1.0f, 1.0f, 0.0f) * spawnDist * Random.Range(1.0f - spawnDistanceVariation, 1.0f + spawnDistanceVariation) * BoomPlayer.Instance.transform.localScale.x, 
			Quaternion.identity);
		BoomEnemy enemy = newEnemy.GetComponent<BoomEnemy>();
		enemy.strength = BoomPlayer.Instance.strength * scales[Random.Range(0, scales.Count)];
		newEnemy.transform.localScale = Vector3.one * (BoomPlayer.Instance.transform.localScale.x * enemy.strength / BoomPlayer.Instance.strength);
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(Vector3.zero, spawnDist * (1.0f - spawnDistanceVariation));
		Gizmos.DrawWireSphere(Vector3.zero, spawnDist * (1.0f + spawnDistanceVariation));
	}
}
