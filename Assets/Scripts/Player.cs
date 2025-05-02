using Unity.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
	#region Game Objects & Components

	[Header("Game Objects & Components")]
    public Rigidbody2D Body;
	public LayerMask DebrisLayer;
	public GameObject Debris { get; private set; }
	public GameManager Manager;

	public SpriteRenderer Sprite;
	public Sprite[] Sprites;

	#endregion

	#region Grappling Settings

	/// <summary>
	/// How fast the player moves towards the target.
	/// </summary>
	[Header("Grappling Settings")]
	[Range(0, 5000)]
	public float MinSpeed = 550;

	[Range(0, 5000)]
	public float MaxSpeed = 3000;

	public float Speed = 0;

	/// <summary>
	/// The amount of time the player has to wait before grabbing again.
	/// </summary>
	[Range(0f, 2f)]
	public float Cooldown = 0.3f;
	private float CooldownRemaining = 0f;

	/// <summary>
	/// How much drag the player is experiencing when moving towards a target.
	/// </summary>
	[Range(0f, 5f)]
	public float AttachedDrag = 1f;

	/// <summary>
	/// How much drag the player is experiencing when detached and floating away.
	/// </summary>
	[Range(0f, 5f)]
	public float DetachedDrag = 2f;

	private float DragMultiplier = 1f;

	#endregion

	#region Gravity Gun Methods

	/// <summary>
	/// Grabs the target object under the mouse cursor.
	/// </summary>
	/// <param name="debug"></param>
	private void Attach(bool debug = false) {
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Collider2D hit = Physics2D.OverlapPoint(mousePos, DebrisLayer);

		//If clicked on a grab-able object...
		if (hit != null) {
			Body.drag = AttachedDrag * DragMultiplier;

			Debris = hit.gameObject;

			CooldownRemaining = Cooldown;

			if (debug) Debug.Log($"[Player] Grabbing onto {Debris.name}.");
		}
	}

	/// <summary>
	/// Moves towards the target linearly over time.
	/// </summary>
	private void Move() {
		if (Debris == null) return;
		Vector2 direction = (Debris.transform.position - transform.position).normalized;
		Body.velocity = Speed * Time.fixedDeltaTime * direction;
	}

	/// <summary>
	/// Detaches from the target.
	/// </summary>
	public void Detach(bool debug = false) {
		if (debug) Debug.Log($"[Player] Detaching from {Debris.name}.");
		Body.drag = DetachedDrag * DragMultiplier;
		Debris = null;
	}

	#endregion

	#region Player & Scoring Methods

	public void Kill(bool debug = false) {

		//TODO: Record the time & score.

		//TODO: Display the death menu.

		if (debug) Debug.Log($"[Player] Killing {name}.");
		Destroy(gameObject);
	}

	#endregion

	#region Unity

	private void Start() {
	}

	void Update() {
		//Cooldown
		if (CooldownRemaining > 0f) CooldownRemaining -= Time.deltaTime;
		else CooldownRemaining = 0f;

		//On left-click...
		if (Input.GetMouseButtonDown(0) && CooldownRemaining == 0) Attach(false);

		//Difficulty-scaled Speed
		float t = Mathf.Clamp01((Manager.Difficulty - 1f) / (Manager.DifficultyCap - 1f));
		Speed = Mathf.Lerp(MinSpeed, MaxSpeed, t);

		//Difficulty-scaled Drag
		DragMultiplier = Mathf.Lerp(Manager.Difficulty, Manager.DifficultyCap, t);

		//Update sprite based on heading
		if (Body.velocity.normalized.y > 0.25) Sprite.sprite = Sprites[1];
		else if (Body.velocity.normalized.y < -0.25) Sprite.sprite = Sprites[2];
		else Sprite.sprite = Sprites[0];
	}

	private void FixedUpdate() {
		Move();
	}

	#endregion
}

