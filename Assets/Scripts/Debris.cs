using UnityEngine;

public class Debris : MonoBehaviour
{
	#region Drift and Rotation

	[Header("Drift and Rotation")]
	[Range(0f, 20f)]
	public float DriftSpeedMin;

	[Range(0f, 20f)]
	public float DriftSpeedMax;

	private float DriftSpeed;

	[Range(-60f, 60f)]
	public float RotationSpeed;

	#endregion

	#region Debris

	[Header("Debris")]
	public DebrisType Type;

	public enum DebrisType { Normal, Fragile, Hazard }

	public Rigidbody2D Body;

	#endregion

	#region Collision Behavior

	void Disintegrate(bool debug = false) {
		//TODO: Crack into 3 pieces and floats away
		Destroy(gameObject);
	}

	void Explode(Player player, bool debug = false) {
		//TODO: Explode and kills the player
		Destroy(gameObject);
		//Player.Kill();
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

		DriftSpeed = Random.Range(DriftSpeedMin, DriftSpeedMax);
		transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
	}

	private void Update() {
		transform.Rotate(0, 0, RotationSpeed * Time.deltaTime);
		Body.AddForce(DriftSpeed * Time.deltaTime * Vector2.up, ForceMode2D.Force);

		CheckDespawn(true);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.TryGetComponent<Player>(out var player)) {

			//Detach if it's the debris the player is grabbing onto.
			if (player.Debris == gameObject) player.Detach();

			//React based on the type of debris.
			switch (Type) {
				case DebrisType.Normal:
					Debug.Log($"[Debris] Player passed through {name}.");
					break;

				case DebrisType.Fragile:
					Disintegrate(true);
					break;

				case DebrisType.Hazard:
					Explode(player, true);
					break;
			}
		}
	}

	#endregion
}
