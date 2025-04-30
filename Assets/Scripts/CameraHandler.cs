using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour {
	[Header("Camera Settings")]
	public GameObject Player;

	private Vector3 PlayerPos;

	[Range(0f, 100f)]
    public float Speed = 10f;

	[Range(-100, 100)]
	public float XOffset = 12f;

	private float LowestX = 0;

	[Range(-20, 20)]
	public float MinY = -5f;

	[Range(-20, 20)]
	public float MaxY = 5f;

    void Update() {
		//Update LowestX
		if (Player != null) PlayerPos = Player.transform.position;

		//Clamp the X position
		if (PlayerPos.x > LowestX) LowestX = PlayerPos.x;

		//Clamp the Y position
		float clampedY = Mathf.Clamp(PlayerPos.y, MinY, MaxY);

		Vector3 targetPosition = new(LowestX + XOffset, clampedY, -10f);
        Vector3 currentPosition = Vector3.Lerp(transform.position, targetPosition, Speed * Time.deltaTime);

        transform.position = currentPosition;
    }
}
