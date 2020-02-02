using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Alien : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float reach = 0.3f;

    private Vector2 moveDirection;
    public bool dieing = false;

    void Start()
    {
         
    }
    
    void Update()
    {
        if (dieing)
            return;

        var spaceguys = FindObjectsOfType<Spaceguy>();
        float closestDistance = float.PositiveInfinity;
        Spaceguy closestGuy = null;
        foreach (var guy in spaceguys)
        {
            if (guy.deadzo)
                continue;

            var distanace = Vector3.Distance(guy.transform.position, transform.position);
            if (distanace < closestDistance)
            {
                closestGuy = guy;
                closestDistance = distanace;
            }
        }

        if(closestDistance < reach)
        {
            closestGuy.SetDead(true);
            // Walk in the opposite direciton for a bit now that you got em.
            moveDirection = -moveDirection;
            closestGuy = null;
        }

        if (closestGuy != null)
        {
            Vector3 direction = closestGuy.transform.position - transform.position;
            direction.Normalize();
            moveDirection = direction;
        }

        GetComponent<SpriteRenderer>().flipX = moveDirection.x < 0;
    }

    public async void Die()
    {
        dieing = true;
        GetComponent<Animator>().SetTrigger("Die");
        await Task.Delay(1500);
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (!dieing)
        {
            moveAlien(moveDirection);
        }
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
