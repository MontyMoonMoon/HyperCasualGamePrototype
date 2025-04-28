using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHandler : MonoBehaviour
{

    //Grapple Cooldown Missing
    //Slinghsot effect doable
    public Rigidbody2D rb;
    public LayerMask grappleLayer;
    public float grappleStrength = 10f;

    private SpringJoint2D spring;

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

