using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

[Serializable]
public struct TaskConfig
{
	public List<int> amounts;
	public float duration;
	public int TotalAmount => amounts.Sum();

	/// <summary>
	/// 
	/// </summary>
	/// <returns> The index of the popped amount. </returns>
	public int Pop()
	{
		var result = amounts.GetRandomWeightedIndex();
		amounts[result]--;
		return result;
	}
}

/// <summary>
/// Generate enemies based on task configs.
/// The spawn position will be distributed in a circle around the player.
/// </summary>
public class EnemyGenerator : Singleton<EnemyGenerator>
{
	public List<Enemy> enemyPrefabs;
	Dictionary<Enemy, EntityPool<Enemy>> enemyPools;
	[NonSerialized] public List<Enemy> activeEnemies = new List<Enemy>();
	
	public List<TaskConfig> taskConfigs;
	public int currentTaskIndex = 0;
	
	public LoopTask task;
	

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
	
	
	public Transform spawnTransform;
	
	protected override void Awake()
	{
		base.Awake();
		enemyPools = new();
		foreach (Enemy enemyPrefab in enemyPrefabs)
		{
			enemyPools.Add(enemyPrefab, new EntityPool<Enemy>(enemyPrefab, transform));
		}
	}

	void Start()
	{
		//NewTask();
	}

	public void NewTask()
	{
		if (taskConfigs.Count > currentTaskIndex)
		{
			int index = currentTaskIndex;
			task = new LoopTask
			{
				interval = taskConfigs[index].duration / taskConfigs[index].TotalAmount,
				loop = taskConfigs[index].TotalAmount,
				loopAction = () => ProcessTask(taskConfigs[index]),
				finishAction = NewTask,
			};
			task.Start();
		}
		currentTaskIndex++;
	}

	public void ProcessTask(TaskConfig taskConfig)
	{
		if (!spawnTransform) return;
		
		int enemyIndex = taskConfig.Pop();
		if (enemyPrefabs.Count <= enemyIndex)
		{
			return;
		}
		Enemy enemyPrefab = enemyPrefabs[enemyIndex];
		if (!enemyPools.ContainsKey(enemyPrefab))
		{
			Debug.LogError("Enemy prefab not found in the pool.");
			return;
		}
		
		Enemy enemy = enemyPools[enemyPrefab].Get();
		Vector3 spawnPosition = GetSpawnPosition();
		activeEnemies.Add(enemy);
		enemy.transform.position = spawnPosition;
		onSpawn?.Invoke(enemy);
		enemy.onDeinit += () =>
		{
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
		distribution *= new System.Random().Next(2) == 0 ? 1 : -1;
		lastSpawnedAngle += distribution;
		Vector3 spawnDirection = Quaternion.Euler(0f, 0f, lastSpawnedAngle) * Vector2.right;
		Vector3 spawnPosition = spawnTransform.position + spawnDirection * randomDistance;
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
