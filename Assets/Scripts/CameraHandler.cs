using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
    // Start is called before the first frame update
  {
    public Transform player;
    public float smoothSpeed = 0.125f;
    private float lowestY; // Highest point reached so far

    void Start()
    {
        lowestY = player.position.y;
    }

    void LateUpdate()
    {
        if (player.position.y > lowestY)
        {
            lowestY = player.position.y;
        }

        Vector3 desiredPosition = new Vector3(0, lowestY, -10f); // Keep X=0 and Z=-10 for 2D
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
