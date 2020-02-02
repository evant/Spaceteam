using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    public float moveSpeed = 1f;

    private Vector2 moveDirection;

    void Start()
    {
         
    }
    
    void Update()
    {
        var spaceguys = FindObjectsOfType<Spaceguy>();
        float closestDistance = float.PositiveInfinity;
        GameObject closestObject = null;
        foreach (var guy in spaceguys)
        {
            var distanace = Vector3.Distance(guy.transform.position, transform.position);
            if (distanace < closestDistance)
            {
                closestObject = guy.gameObject;
                closestDistance = distanace;
            }
        }

        Vector3 direction = closestObject.transform.position - transform.position;
        direction.Normalize();
        moveDirection = direction;

        GetComponent<SpriteRenderer>().flipX = direction.x < 0;
    }

    private void FixedUpdate()
    {
        moveAlien(moveDirection);
    }

    void moveAlien(Vector2 direction)
    {
        var canMove = true;
        var movement = moveDirection * moveSpeed * Time.fixedDeltaTime;
        var center = new Vector2(transform.position.x, transform.position.y - 0.2f);
        var hit = Physics2D.Raycast(center, moveDirection.normalized);

        if (hit.collider != null)
        {
            if (hit.distance < movement.magnitude + 0.2)
            {
                canMove = false;
            }
        }

        if (canMove)
        {
            transform.Translate(movement);
        }
    }
}
