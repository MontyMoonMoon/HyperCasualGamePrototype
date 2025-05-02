using UnityEngine;

public class NearMiss : MonoBehaviour
{
	private GameManager Manager;

	public void Start() {
		Manager = GameObject.Find("Game Manager").GetComponent<GameManager>();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		Debug.Log("[NearMiss] Object detected!");
		if (other.gameObject.GetComponent<Player>() != null) {
			Debug.Log("[NearMiss] Near miss!");

			Manager.AddScoreMultiplier(true);

			//TODO: Insert eye candy here
		}
	}
}
