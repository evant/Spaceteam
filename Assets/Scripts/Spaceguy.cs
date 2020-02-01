using UnityEngine;

public class Spaceguy : MonoBehaviour
{
    public float speed = 6.0f;
    public LayerMask blockingLayer;
    private Vector3 moveDirection = Vector3.zero;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveDirection *= speed * Time.deltaTime;

        var start = transform.position;

        RaycastHit2D hit;
        boxCollider.enabled = false;
        hit = Physics2D.Raycast(start, moveDirection);
        boxCollider.enabled = true;

        if (hit.transform == null)
        {
            rb2D.transform.Translate(moveDirection);
            transform.Translate(moveDirection);
        }
    }
}
