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
	public int TotalAmount => amounts?.Sum() ?? 0;

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
public class EntityGenerator : MonoBehaviour
{
	public List<Entity> enemyPrefabs;
	[NonSerialized] public List<Entity> activeEnemies = new List<Entity>();
	
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
	[Tooltip("The number of enemies to spawn at one position. Will only cost one spawn from the config.")]
	public int spawnCount = 1;
	[Tooltip("The time interval to move to next spawn position, 0 means different spawn position every spawn.")]
	public float spawnInterval = 0;

	private float spawnTimer;
	
	protected float lastSpawnedAngle = 0;
	
	public Transform spawnTransform;

	private void OnEnable()
	{
		NewTask();
	}
	
	public void NewTask()
	{
		if (taskConfigs.Count > currentTaskIndex)
		{
			int index = currentTaskIndex;
			if (taskConfigs[index].TotalAmount == 0)
			{
				task = new LoopTask
				{
					interval = taskConfigs[index].duration,
					finishAction = NewTask,
				};
			}
			else
			{
				task = new LoopTask
				{
					interval = taskConfigs[index].duration / taskConfigs[index].TotalAmount,
					loop = taskConfigs[index].TotalAmount,
					loopAction = () => ProcessTask(taskConfigs[index]),
					finishAction = NewTask,
				};
			}
			task?.Start();
		}
		currentTaskIndex++;
	}

	public void ProcessTask(TaskConfig taskConfig)
	{
		if (!spawnTransform) return;

		if (taskConfig.TotalAmount == 0) return;
		
		int enemyIndex = taskConfig.Pop();
		if (enemyPrefabs.Count <= enemyIndex)
		{
			return;
		}
		Entity enemyPrefab = enemyPrefabs[enemyIndex];

		var spawnPosition = GetSpawnPosition();
		if (spawnCount > 1)
		{
			new LoopTask
			{
				interval = spawnInterval,
				loop = spawnCount,
				loopAction = () => Spawn(enemyPrefab, spawnPosition)
			}.Start();
		}
		else
		{
			Spawn(enemyPrefab, spawnPosition);
		}
	}

	/// <summary>
	/// Reset on disable
	/// </summary>
	public Action<Entity> onSpawn;

	public Entity Spawn(Entity prefab, Vector3 position)
	{
		Entity enemy = PoolManager.Instance.New(prefab);
		enemy.transform.position = position;
		activeEnemies.Add(enemy);
		enemy.onDeinit += () =>
		{
			activeEnemies.Remove(enemy);
		};
		enemy.Init();
		onSpawn?.Invoke(enemy);
		return enemy;
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
		var spawnPosition = spawnTransform.position + spawnDirection * randomDistance;
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
