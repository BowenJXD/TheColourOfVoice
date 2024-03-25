using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

[Serializable]
public struct TaskConfig
{
	public int amount;
	public float duration;
}

/// <summary>
/// Generate enemies based on task configs.
/// The spawn position will be distributed in a circle around the player.
/// </summary>
public class EnemyGenerator : MonoBehaviour
{
	public Enemy enemyPrefab;
	public List<TaskConfig> taskConfigs;
	public LoopTask task;
	[NonSerialized] public List<Enemy> activeEnemies = new List<Enemy>();
	
	protected ObjectPool<ParticleSystem> deathFXPool;
	public ParticleSystem deathFX;
	protected float deathFXDuration;
	
	// 生成位置波动
	[Tooltip("The minimum distance between the player and the spawning point.")]
	public float minDistance = 10;
	[Tooltip("The maximum distance between the player and the spawning point.")]
	public float maxDistance = 20;
	[Tooltip("The period of the sine wave that controls the distribution variation.")]
	public float distributionVariationInterval = 5;
	[Tooltip("[0 - 360] The minimum change in angle between each spawn.")]
	public float minDistribution = 30;
	[Tooltip("[0 - 360] The maximum change in angle between each spawn.")]
	public float maxDistributionVariation = 60;
	
	protected float lastSpawnedAngle = 0;

	public Action<Enemy> onSpawn;
	
	EntityPool<Enemy> enemyPool;
	
	Transform playerTransform;
	
	private void Awake()
	{
		deathFXPool = new ObjectPool<ParticleSystem>(() =>
		{
			ParticleSystem particleSystem = Instantiate(deathFX);
			return particleSystem;
		}, prefab =>
		{
			prefab.gameObject.SetActive(true);
			prefab.Play();
		}
		, prefab =>
		{
			prefab.gameObject.SetActive(false);
			prefab.Stop();
			prefab.Clear();
		}
		, prefab => { Destroy(prefab); }
		, true, 100);
		enemyPool = new EntityPool<Enemy>(enemyPrefab, transform);
		deathFXDuration = deathFX.main.duration;
	}

	void Start()
	{
		NewTask();
		// find player by tag
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}

	public void NewTask()
	{
		if (taskConfigs.Count > 0)
		{
			task = new LoopTask
			{
				interval = taskConfigs[0].duration / taskConfigs[0].amount,
				loop = taskConfigs[0].amount,
				loopAction = ProcessTask,
				finishAction = NewTask,
			};
			taskConfigs.RemoveAt(0);
			task.Start();
		}
	}

	public void ProcessTask()
	{
		if (!playerTransform) return;
		
		Vector3 spawnPosition = GetSpawnPosition();
		Enemy enemy = enemyPool.Get();
		activeEnemies.Add(enemy);
		enemy.transform.position = spawnPosition;
		onSpawn?.Invoke(enemy);
		enemy.onDeinit += () =>
		{
			ParticleSystem ps = deathFXPool.Get();
			ps.transform.position = enemy.transform.position;
			ps.transform.parent = transform;
			LoopTask loopTask = new LoopTask
			{
				interval = deathFXDuration,
				loopAction = () => { deathFXPool.Release(ps); }
			};
			loopTask.Start();
			activeEnemies.Remove(enemy);
		};

		enemy.Init();
	}

	Vector3 GetSpawnPosition()
	{
		float randomDistance = Random.Range(minDistance, maxDistance);
		float randomRange = maxDistributionVariation 
		                    * Mathf.Cos(Time.timeSinceLevelLoad / distributionVariationInterval);
		int roundedAbsRange = Mathf.RoundToInt(Mathf.Abs(randomRange));
		int distribution = (int)minDistribution + new System.Random().Next(0, roundedAbsRange);
		distribution *= new System.Random().Next(1) == 0 ? 1 : -1;
		lastSpawnedAngle += distribution;
		Vector3 spawnDirection = Quaternion.Euler(0f, 0f, lastSpawnedAngle) * Vector2.right;
		Vector3 spawnPosition = playerTransform.position + spawnDirection * randomDistance;
		return spawnPosition;
	}
	
	public void Pause()
	{
		task.Pause();
	}
	
	public void Resume()
	{
		task.Resume();
	}

	private void OnDisable()
	{
		onSpawn = null;
	}
}
