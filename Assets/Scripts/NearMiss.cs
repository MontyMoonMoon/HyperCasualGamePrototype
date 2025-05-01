using UnityEngine;

public class NearMiss : MonoBehaviour
{
	public GameManager Manager;

	public void Start() {
		Manager = GameObject.Find("Game Manager").GetComponent<GameManager>();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.GetComponent<Player>() != null) {
			Manager.AddScoreMultiplier();

			//TODO: Insert eye candy here
		}
	}
}
