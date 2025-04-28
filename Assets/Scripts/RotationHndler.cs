using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationHndler : MonoBehaviour
{
    
    public float rotationSpeed = 20f; // degrees per second??

     private Transform player;
    public float driftSpeed = 0.5f;


     void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
         driftSpeed = Random.Range(0.2f, 0.6f); // random drift speed
    }


    void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

          if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)direction * driftSpeed * Time.deltaTime;
        }
    }

    
}
