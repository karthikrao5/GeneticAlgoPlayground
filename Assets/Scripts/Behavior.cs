using System;
using UnityEngine;

public class Behavior : MonoBehaviour
{
    public float speed; //Floating point variable to store the player's movement speed.

    private Vector2 velocity;

    private BoxCollider2D boxCollider;
    
    private Vector2 screenBounds;


    // Use this for initialization
    void Start()
    {
        velocity = Vector2.zero;
        boxCollider = GetComponent<BoxCollider2D>();
        screenBounds =
            Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxis("Horizontal");

        //Store the current vertical input in the float moveVertical.
        float moveVertical = Input.GetAxis("Vertical");

        //Use the two store floats to create a new Vector2 variable movement.
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        velocity.x = Mathf.MoveTowards(velocity.x, speed * movement.x, Time.deltaTime);
        velocity.y = Mathf.MoveTowards(velocity.y, speed * movement.y, Time.deltaTime);

        transform.Translate(velocity * Time.deltaTime);

        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);

        foreach (Collider2D hit in hits)
        {
            if (hit != boxCollider)
            {
                ColliderDistance2D distance2D = hit.Distance(boxCollider);

                if (distance2D.isOverlapped)
                {
                    transform.Translate(distance2D.pointA - distance2D.pointB);
                }
            }
        }
    }
    
    void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, -screenBounds.x, screenBounds.x);
        viewPos.y = Mathf.Clamp(viewPos.y, -screenBounds.y, screenBounds.y);
        transform.position = viewPos;
    }
}