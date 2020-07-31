using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformer : MonoBehaviour
{
    public float Speed = 300f;
    public float Jump = 1f;

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private BoxCollider2D _collider;
    private bool contactDetected = false;

    void Start()
    {
        // References
        this._rigidbody = this.GetComponent<Rigidbody2D>();
        this._animator = this.GetComponent<Animator>();
        this._collider = this.GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        // Get horizontal movement delta from key
        float deltaX = Input.GetAxis("Horizontal") * (this.Speed * Time.smoothDeltaTime);

        // Apply this delta to Rigidbody2D's velocity, perserving gravity
        this._rigidbody.velocity = new Vector2(deltaX, this._rigidbody.velocity.y);

        // Detect contact to surface
        this.DetectContact();

        // Check if player is grounded and idle
        if (this.contactDetected && deltaX == 0)
        {
            // Set gravity to zero, so no sliding on slope
            this._rigidbody.gravityScale = 0;
        }
        else
        {
            this._rigidbody.gravityScale = 1;
        }

        // Jump only when key is pressed and grounded to surface
        if (Input.GetKeyDown(KeyCode.Space) && this.contactDetected)
        {
            this._rigidbody.AddForce(Vector2.up * this.Jump, ForceMode2D.Impulse);
        }

        // If player is on the move...
        if (UnityEngine.Mathf.Approximately(UnityEngine.Mathf.Abs(deltaX), 0f) == false)
        {
            // Set animation-controller variable to delta x
            this._animator.SetFloat("Speed", UnityEngine.Mathf.Abs(deltaX));

            // Face animation-clip towards direction indicated by delta x
            this.transform.localScale = new Vector3(UnityEngine.Mathf.Sign(deltaX), 1, 1);
        }
        else
        {
            // If player is not moving, set animation-controller variable to zero
            this._animator.SetFloat("Speed", 0f);
        }
    }

    // Called to detect overlap between two rectangles
    private void DetectContact()
    {
        // Get a reference to the bounding rectangle of the Collider
        Bounds box = this._collider.bounds;

        // Create a new tiny rectangle right below the Collider rectangle
        Vector2 max = new Vector2(box.max.x, (box.max.y - box.size.y) - 0.1f);
        Vector2 min = new Vector2(box.min.x, box.min.y - 0.2f);

        // Check to see if the new rectangle overlaps with another rectangle
        Collider2D contact = Physics2D.OverlapArea(min, max);

        // If there was an overlap...
        if (contact != null)
        {
            // Set the flag
            this.contactDetected = true;
        }
        else
        {
            this.contactDetected = false;
        }
    }
}
