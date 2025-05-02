using UnityEngine;

public class Debris : MonoBehaviour
{
	#region Game Objects & Components

	[Header("Game Objects & Components")]

	public Rigidbody2D Body;

	public DebrisType Type;

	public enum DebrisType { Normal, Fragile, Hazard }

	public GameManager Manager;

	#endregion

	#region Drift and Rotation


	[Header("Drift and Rotation")]
	[Range(0f, 1f)]
	public float DriftingChance = 0.1f;

	private bool Drifting = false;

	[Range(0f, 5f)]
	public float DriftSpeedMin;

	[Range(0f, 5f)]
	public float DriftSpeedMax;

	public float TurnRange = 0.25f;

	private float DriftSpeed;

	public Vector2 PointA;

	public Vector2 PointB;

	private Vector2 Target;

	[Range(-60f, 60f)]
	public float RotationSpeed;

	private void Drift() {
		if (!Drifting || PointA == PointB) return;

		Vector2 position = transform.position;
		float distanceA = Vector2.Distance(position, PointA);
		float distanceB = Vector2.Distance(position, PointB);
		float travel = DriftSpeed * Time.deltaTime;

		//If this is close to PointA, move towards PointB.
		//Automatically engages on first frame.
		if (distanceA < TurnRange && Target != PointB) Target = PointB;

		//If this is close to PointB, move towards PointA.
		else if (distanceB < TurnRange && Target != PointA) Target = PointA;

		transform.position = Vector2.Lerp(position, Target, travel);
	}

	#endregion

	#region Collision Behavior

	void Disintegrate(bool debug = false) {
		//TODO: Crack into 3 pieces and floats away
		Destroy(gameObject);
	}

	void Explode(bool debug = false) {
		//TODO: Explode
		Destroy(gameObject);
	}

	#endregion

	#region Despawning

	private GameObject Spawner;
	private DebrisSpawner Script;

	private void CheckDespawn(bool debug = false) {
		Vector2 debrisPos = transform.position;
		Vector2 spawnerPos = Spawner.transform.position;

		if (spawnerPos.x - debrisPos.x > Script.DespawnX) {
			Destroy(gameObject);
			if (debug) Debug.Log($"[Debris] {name} despawned due to side scroll.");
		}

		else if (debrisPos.y - spawnerPos.y > Script.DespawnY) {
			Destroy(gameObject);
			if (debug) Debug.Log($"[Debris] {name} despawned due to drift.");
		}
	}

	#endregion

	#region Unity

	private void Start() {
		Spawner = GameObject.Find("Debris Spawner");
		Script = Spawner.GetComponent<DebrisSpawner>();
		Manager = GameObject.Find("Game Manager").GetComponent<GameManager>();

		//Difficulty-scaled drift chance
		float t = Mathf.Clamp01((Manager.Difficulty - 1f) / (Manager.DifficultyCap - 1f));
		DriftingChance = Mathf.Lerp(DriftingChance, 1f, t);
		Drifting = Random.Range(0f, 1f) < DriftingChance;

		//Difficulty-scaled drift speed
		float driftSpeedNow = Mathf.Lerp(DriftSpeedMin, DriftSpeedMax, t);
		DriftSpeed = Random.Range(driftSpeedNow, DriftSpeedMax);

		transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
	}

	private void Update() {
		transform.Rotate(0, 0, RotationSpeed * Time.deltaTime);

		Drift();

		CheckDespawn(true);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.TryGetComponent<Player>(out var player)) {

			//Detach if it's the debris the player is grabbing onto.
			if (player.Debris == gameObject) player.Detach();

			//React based on the type of debris.
			switch (Type) {
				case DebrisType.Normal:
					//Debug.Log($"[Debris] Player passed through {name}.");

					break;

				case DebrisType.Fragile:
					Manager.AddScore();
					Disintegrate(true);

					break;

				case DebrisType.Hazard:
					if (Manager.Multiplier >= Manager.MultiplierShield) 
						Manager.ResetMultiplier();

					else player.Kill();

					Explode(true);

					break;
			}
		}
	}

	#endregion
}
