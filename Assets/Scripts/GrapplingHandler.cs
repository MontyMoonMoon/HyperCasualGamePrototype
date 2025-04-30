using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHandler : MonoBehaviour
{

    //Grapple Cooldown implemented
    //Slinghsot effect doable
    public Rigidbody2D rb;
    public LayerMask grappleLayer;
    public float grappleStrength = 10f;

    private SpringJoint2D spring;

    public float grappleCooldown = 2f;
    private float lastGrappleTime;
    private Camera cam;
    public float bounceFactor = 1.2f;


    void Start()

    //gets rb of player obj and camera for constraint method
    {
    rb = GetComponent<Rigidbody2D>();
    cam = Camera.main;
    }

    void FixedUpdate()
    {
    KeepPlayerInBounds();
    }

   //handles screen constraints players wills bounce off the screen
    void KeepPlayerInBounds()
    {
        Vector3 screenPos = cam.WorldToViewportPoint(transform.position);
        Vector2 velocity = rb.velocity;

    bool bounced = false;

    if (screenPos.x < 0f || screenPos.x > 1f)
    {
        velocity.x *= -bounceFactor;
        bounced = true;
    }

    if (screenPos.y < 0f || screenPos.y > 1f)
    {
        velocity.y *= -bounceFactor;
        bounced = true;
    }

    if (bounced)
    {
        rb.velocity = velocity;
    }
}
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // or touch input
     if (Input.GetMouseButtonDown(0)) // left click
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mousePos, grappleLayer);

        if (hit != null)
    {
        AttachGrapple(hit.transform.position);
    }
   
}

   if (Input.GetMouseButtonDown(0) && Time.time > lastGrappleTime + grappleCooldown)
    {
        lastGrappleTime = Time.time;
        DetachGrapple();
    }

        if (Input.GetMouseButtonUp(0))
        {
            DetachGrapple();
        }
    }

    void AttachGrapple(Vector2 point)
    {
        spring = gameObject.AddComponent<SpringJoint2D>();
        spring.connectedAnchor = point;
        spring.distance = Vector2.Distance(transform.position, point) * 0.5f; // slack amount
        spring.frequency = grappleStrength;
        spring.dampingRatio = 0.7f;
        spring.autoConfigureDistance = false;
    }

    void DetachGrapple()
    {
        if (spring != null)
        {
            Destroy(spring);
        }
    }
}

