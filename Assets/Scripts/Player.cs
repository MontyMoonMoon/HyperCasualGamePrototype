using UnityEngine;

public class Player : MonoBehaviour
{
	#region Game Objects & Components

	[Header("Game Objects & Components")]
    public Rigidbody2D Body;
	public LayerMask ObstacleLayer;
	private Transform Target;

	#endregion

	#region Grappling Settings

	[Header("Grappling Settings")]
	public float Strength = 10f;
	public float Cooldown = 0.2f;
	private float CooldownRemaining = 0f;

	#endregion

	#region Gravity Gun Methods

	/// <summary>
	/// Grabs the target object under the mouse cursor.
	/// </summary>
	/// <param name="debug"></param>
	private void GrabTarget(bool debug = false) {
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Collider2D hit = Physics2D.OverlapPoint(mousePos, ObstacleLayer);

		//If clicked on a grab-able object...
		if (hit != null) {
			Body.velocity = Vector2.zero;

			Target = hit.gameObject.transform;

			CooldownRemaining = Cooldown;

			if (debug) Debug.Log($"[Player] Grabbing onto {hit.transform.name}.");
		}
	}

	/// <summary>
	/// Moves towards the target linearly over time.
	/// </summary>
	private void Move() {
		if (Target == null) return;
		Vector2 direction = (Target.transform.position - transform.position).normalized;
		Body.velocity = Strength * Time.fixedDeltaTime * direction;
	}

	/// <summary>
	/// Detaches from the target.
	/// </summary>
	public void Detach(bool debug = false) {
		Target = null;
		if (debug) Debug.Log("[Player] Detached from target.");
	}

	#endregion

	#region Unity

	void Update() {
		//Cooldown
		if (CooldownRemaining > 0f) CooldownRemaining -= Time.deltaTime;
		else CooldownRemaining = 0f;

		//On left-click...
		if (Input.GetMouseButtonDown(0) && CooldownRemaining == 0) GrabTarget(true);
	}

	private void FixedUpdate() {
		Move();
	}

	#endregion
}

