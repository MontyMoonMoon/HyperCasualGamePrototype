using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimHandler : MonoBehaviour
{

    public Transform player; // Reference to the player transform

    public float radius = 5f; // Radius of the circular area

    public float offsetAngle = 0f;


     void Update()
    {
        // Get mouse position in world space
         // Get mouse world position
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        // Direction from player to mouse
        Vector3 direction = (mousePos - player.position).normalized;

        // Gun position around player
        transform.position = player.position + direction * radius;

        // Rotate gun to face mouse
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle + offsetAngle);

           if (direction.x < 0)
        {
            transform.localScale = new Vector3(1, -1, 1); // flip Y
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
