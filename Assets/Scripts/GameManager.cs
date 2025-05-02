using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class GameManager : MonoBehaviour {

	#region Game Objects & Components

	[Header("Game Objects & Components")]
	public Player Player;

	public DebrisSpawner Spawner;

	public TextMeshProUGUI ScoreText;

	public AudioSource Music;

	public AudioClip[] MusicTracks;

	#endregion

	#region Scoring

	[Header("Score")]
	public int Score = 0;

	public int HighScore = 0;

	public int Multiplier = 1;

	public int MultiplierShield = 5;

	public float TimeSpent = 0f;

	public float ScoreRatio = 0f;

	private string SavePath;

	public void AddScore(bool debug = false) {
		Score += Multiplier;
		ScoreRatio = Score / TimeSpent;
		if (debug) Debug.Log($"[GameManager] +1 Score! ({Score})");

		if (Score > HighScore) {
			HighScore = Score;
			if (debug) Debug.Log($"[GameManager] New high score! ({HighScore})");

			File.WriteAllText(SavePath, Score.ToString());
		}
	}

	public void AddScoreMultiplier(bool debug = false) {
		Multiplier++;
		if (debug) Debug.Log($"[GameManager] +1 Multiplier! (x{Multiplier})");

		if (Multiplier >= MultiplierShield) {
			Spawner.MinDebris = 3;
			Spawner.MaxDebris = 5;
		}
	}

	public void ResetMultiplier(bool debug = false) {
		Multiplier = 1;
		if (debug) Debug.Log($"[GameManager] Reset multiplier to x1.");

		Spawner.MinDebris = 1;
		Spawner.MaxDebris = 3;
	}

	public void UpdateScoreText() {
		string text
		= $"Score: {Score}  +{Multiplier}\n"
		+ $"Time: {TimeSpent:F2}\n"
		+ $"High: {HighScore}" + (Score == HighScore ? "!!!\n" : "\n")
		+ (Multiplier >= MultiplierShield ? "SHIELDED!" : "");

		ScoreText.text = text;
	}

	#endregion

	#region Difficulty Scaling

	[Header("Difficulty Scaling")]
	public float Difficulty = 1f;

	public float DifficultyCap = 5f;

	public float CrankedMultiplier = 1.5f;

	public float TimeCap = 300f;

	#endregion

	#region Utilities

	public void Restart() {
		SceneManager.LoadScene(0); //Reloads this scene.
		//TODO: Make this a soft restart. Don't reload the scene.
	}

	private void PlayMusic() {
		if (!Music.enabled || Music.isPlaying) return;

		if (MusicTracks.Length > 0) {
			AudioClip newTrack;

			do {
				//Randomize the track until it is different from the current track.
				newTrack = MusicTracks[UnityEngine.Random.Range(0, MusicTracks.Length)];

				//If no music is playing, assign the new track.
				if (Music.clip == null) {
					Music.clip = newTrack;
					break;
				}
			}
			while (Music.clip == newTrack);

			//They're all from Helltaker lmao
			Debug.Log($"[LevelManager] Now playing: {Music.clip.name}");

			Music.Play();
		}

		else Debug.LogWarning("[LevelManager] No music tracks found.");
	}

	#endregion

	public void Start() {
		string docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		string game = $"{docs}/{Application.productName}";
		SavePath = $"{game}/save";

		//Create directory if missing.
		if (!Directory.Exists(game)) {
			Directory.CreateDirectory(game);
			Debug.Log($"[Recorder] Created {game}.");
		}

		//Create save file if missing.
		if (!File.Exists(SavePath)) {
			File.WriteAllText(SavePath, "0");
			Debug.Log($"[Recorder] Created save file.");
		}

		//Retrieve high score from save file.
		HighScore = int.Parse(File.ReadAllText(SavePath));
	}

	public void Update() {

		#region Game Controls

		if (Input.GetKeyDown(KeyCode.Escape)) {
			Debug.Log("[GameManager] Quitting game.");
			Application.Quit();
		}

		if (Input.GetKeyDown(KeyCode.R)) {
			Debug.Log("[GameManager] Restarting game.");
			Restart();
		}

		PlayMusic();

		#endregion

		#region Time Scaling

		if (Player == null) return;

		UpdateScoreText();

		TimeSpent += Time.deltaTime;

		if (TimeSpent < TimeCap) {
			Difficulty = Mathf.Lerp(1f, DifficultyCap, TimeSpent / TimeCap);
		}

		else if (TimeSpent > TimeCap && Difficulty <= DifficultyCap) {
			Debug.Log($"[GameManager] Time cap reached. Cranking difficulty.");
			Difficulty *= CrankedMultiplier;
		}

		#endregion
	}
}
