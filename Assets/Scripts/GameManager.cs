using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	#region Game Objects & Components

	[Header("Game Objects & Components")]
	public Player Player;

	public GameObject PlayerPrefab;

	public GameObject DebrisPrefab;

	#endregion

	#region Scoring

	public int Score = 0;

	public int Multiplier = 1;

	public float TimeSpent = 0f;

	public float ScoreRatio = 0f;

	public void AddScore(bool debug = false) {
		Score += Multiplier;
		ScoreRatio = Score / TimeSpent;
		if (debug) Debug.Log($"[GameManager] +1 Score! ({Score})");
	}

	public void AddScoreMultiplier(bool debug = false) {
		Multiplier++;
		if (debug) Debug.Log($"[GameManager] +1 Multiplier! (x{Multiplier})");
	}

	public void ResetMultiplier(bool debug = false) {
		Multiplier = 1;
		if (debug) Debug.Log($"[GameManager] Reset multiplier to x1.");
	}

	#endregion

	#region Utilities

	public void Restart() {
		SceneManager.LoadScene(0); //Reloads this scene.
		//TODO: Make this a soft restart. Don't reload the scene.
	}

	#endregion

	public void Update() {
		TimeSpent += Time.deltaTime;

		if (Input.GetKeyDown(KeyCode.Escape)) {
			Debug.Log("[GameManager] Quitting game.");
			Application.Quit();
		}

		if (Input.GetKeyDown(KeyCode.R)) {
			Debug.Log("[GameManager] Restarting game.");
			Restart();
		}
	}
}
