using UnityEngine;

public class DebrisSpawner : MonoBehaviour {

	#region Game Objects & Components
	[Header("Game Objects & Components")]
	public GameObject[] DebrisPrefabs;

	public Transform[] SpawnPoints;

	public GameObject Player;

	public Transform DebrisContainer;

	#endregion

	#region Spawn Values

	[Header("Scroll-spawned Debris")]
	[Range(0, 40)]
	public float DistanceQuota = 15f;

	public float LastSpawn = 0f;

	[Range(0, 5)]
	public float QuotaOffset = 0f;

	[Range(0, 5)]
	public float SpawnOffset = 0f;

	[Range(1, 5)]
	public int MinDebris = 1;

	[Range(1, 5)]
	public int MaxDebris = 3;

	[Header("Bottom-spawned Debris")]

	[Range(0, 10)]
	public float SpawnInterval = 1f;

	[Range(0, 10)]
	public float DriftSpeedMultiplier = 1f;

	[Header("Despawn Values")]
	[Range(-100, 100)]
	public float DespawnX = 60f;

	[Range(-100, 100)]
	public float DespawnY = 30f;

	#endregion

	#region Spawn Methods

	private void ScrollSpawn(bool debug = false) {
		int debrisCount = Random.Range(MinDebris, MaxDebris + 1);

		bool[] usedSpawns = new bool[] {false, false, false, false, false};

		for (int i = 0; i < debrisCount; i++) {

			//Assigns a random unoccupied spawn point to the debris.
			int spawnIndex;
			do spawnIndex = Random.Range(0, SpawnPoints.Length);
			while (usedSpawns[spawnIndex]);

			usedSpawns[spawnIndex] = true;

			GameObject newDebrisObj;

			//1: Spawn the default debris type (fragile).
			if (i == 0) {
				newDebrisObj = Instantiate(DebrisPrefabs[0], SpawnPoints[spawnIndex].position, Quaternion.identity, DebrisContainer);
			}

			//2+: Spawn a random debris type.
			else {
				int index = Random.Range(0, DebrisPrefabs.Length);

				newDebrisObj = Instantiate(DebrisPrefabs[index], SpawnPoints[spawnIndex].position, Quaternion.identity, DebrisContainer);
			}

			Debris debris = newDebrisObj.GetComponent<Debris>();

			debris.PointA = SpawnPoints[spawnIndex].position;
			debris.PointB = SpawnPoints[Random.Range(0, SpawnPoints.Length)].position;

			if (debug) Debug.Log($"[DebrisSpawner] Spawned a {newDebrisObj.name}");
		}

		if (debug) Debug.Log($"[DebrisSpawner] Finished batch of {debrisCount} debris.");
	}

	#endregion

	#region Unity

	private void Update() {

		if (Player == null) return;

		#region Follow player movement

		transform.position = new(Player.transform.position.x + 40, transform.position.y);

		#endregion

		#region Scroll-spawning Debris

		//if (Player.GetComponent<Rigidbody2D>().velocity.x < 0.1f) return;

		float playerX = Player.transform.position.x;

		float distanceSinceLastSpawn = playerX - LastSpawn;

		if (distanceSinceLastSpawn > DistanceQuota) {
			LastSpawn = playerX;
			ScrollSpawn();
		}

		#endregion
	}

	#endregion
}
