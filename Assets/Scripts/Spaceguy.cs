using UnityEngine;

public class Spaceguy : MonoBehaviour
{
    public float speed = 6.0f;
    public LayerMask blockingLayer;
    
    private BoxCollider2D boxCollider;
    private GameObject fire;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;

        fire = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        var moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (moveDirection.magnitude > 0.0f)
        {
            var canMove = true;
            var movement = moveDirection * speed * Time.deltaTime;
            var hit = Physics2D.Raycast(transform.position, moveDirection.normalized);
            if (hit.collider != null)
            {
                fire.transform.position = hit.point;
                if(hit.distance < movement.magnitude)
                {
                    canMove = false;
                }
            }
            else
            {
                fire.transform.position = transform.position;
            }

            if(canMove)
            {
                transform.Translate(movement);
            }
        }
    }
}
