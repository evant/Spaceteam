using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 1f;
    private Rigidbody2D rb2D;
    private Vector2 movement;

    void Start()
    {
         rb2D = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        Vector3 direction = player.position - transform.position;
        direction.Normalize();
        movement = direction;
    }

    private void FixedUpdate()
    {
        moveAlien(movement);
    }

    void moveAlien(Vector2 direction)
    {
        rb2D.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }
}
