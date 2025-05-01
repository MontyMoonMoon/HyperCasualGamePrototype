using UnityEngine;

public class DebrisSpawner : MonoBehaviour {

	#region Game Objects & Components
	[Header("Game Objects & Components")]
	public GameObject DebrisPrefab;

	public GameObject Player;

	public Transform DebrisContainer;

	#endregion

	#region Spawn Values

	[Header("Scroll-spawned Debris")]
	[Range(0, 40)]
	public float DistanceQuota = 15f;

	public float LastSpawn = 0f;

	[Range(0, 5)]
	public float QuotaOffsetRange = 0f;

	[Range(0, 5)]
	public float SpawnOffsetRange = 0f;

	[Range(0, 15)]
	public float LowY = 6f;

	[Range(0, 30)]
	public float HighY = 18f;

	[Range(0, 6)]
	public int MinDebris = 1;

	[Range(0, 6)]
	public int MaxDebris = 3;

	[Header("Bottom-spawned Debris")]
	[Range(-50, 0)]
	public float LowX = -25f;

	[Range(-50, 0)]
	public float HighX = -10f;

	[Range(0, 10)]
	public float SpawnInterval = 1f;

	[Range(0, 10)]
	public float DriftSpeedMultiplier = 1f;

	[Header("Despawn Values")]
	[Range(-50, -30)]
	public float DespawnX = -40f;

	[Range(18, 30)]
	public float DespawnY = 30f;

	#endregion

	#region Spawn Methods

	private void ScrollSpawn(bool debug = false) {
		int debrisCount = Random.Range(MinDebris, MaxDebris + 1);

		float[] column = new float[debrisCount];
		float gap = (HighY - LowY) / (debrisCount + 1);

		Vector2 spawnerPos = transform.position;

		//1: Spawn 1 debris in the middle.
		if (debrisCount == 1) column[0] = HighY - (HighY - LowY) / 2;

		//2: Spawn 2 debris on the 2 edges.
		else if (debrisCount == 2) column = new float[] { LowY, HighY };

		//X: Spawn X debris spread evenly between LowY and HighY.
		else for (int i = 0; i < debrisCount; i++) column[i] = LowY + i * gap;

		float spawnX = Random.Range(-QuotaOffsetRange, QuotaOffsetRange) + spawnerPos.x;

		for (int i = 0; i < debrisCount; i++) {
			float spawnY = column[i] + spawnerPos.y + LowY
				+ Random.Range(-SpawnOffsetRange, SpawnOffsetRange);
			Vector3 spawnPos = new(spawnX, spawnY);
			Instantiate(DebrisPrefab, spawnPos, Quaternion.identity, DebrisContainer);
		}

		//Vector3 spawnPos = new(spawnX, column, 0f);
		//Instantiate(DebrisPrefab, spawnPos, Quaternion.identity, DebrisContainer);

		if (debug) Debug.Log($"[DebrisSpawner] Spawned {debrisCount} debris {gap:F2} units apart.");
	}

	#endregion

	#region Unity

	private void Update() {

		#region Follow player movement

		transform.position = new(Player.transform.position.x + 40, transform.position.y);

		#endregion

		#region Scroll-spawning Debris

		//if (Player.GetComponent<Rigidbody2D>().velocity.x < 0.1f) return;

		float playerX = Player.transform.position.x;

		float distanceSinceLastSpawn = playerX - LastSpawn;

		//Debug.Log($"Distance: {distanceSinceLastSpawn:F2} / {DistanceQuota:F2}");

		if (distanceSinceLastSpawn > DistanceQuota) {
			LastSpawn = playerX;
			ScrollSpawn(true);
		}

		#endregion
	}

	#endregion
}
