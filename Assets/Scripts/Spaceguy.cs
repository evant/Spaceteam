using UnityEngine;

public class Spaceguy : MonoBehaviour
{
    public float speed = 6.0f;
    public float reach = 0.8f;
    public LayerMask blockingLayer;
    
    private BoxCollider2D boxCollider;
    private GameObject fire;
    private GameObject reticle;
    private GameObject currentTarget;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;

        fire = transform.GetChild(0).gameObject;
        reticle = FindChildTagged("Reticle");
    }

    private GameObject FindChildTagged(string tag)
    {
        foreach(Transform child in transform)
        {
            if (child.tag == tag)
                return child.gameObject;
        }

        return null;
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
                if (hit.distance < movement.magnitude + 0.5)
                {
                    canMove = false;
                }
            }
            else
            {
                fire.transform.position = transform.position;
            }

            if (canMove)
            {
                transform.Translate(movement);
            }
        }

        var fires = GameObject.FindGameObjectsWithTag("Hazard");
        float closestDistance = float.PositiveInfinity;
        GameObject closestObject = null;
        foreach(var fire in fires)
        {
            var distanace = Vector3.Distance(fire.transform.position, transform.position);
            if(distanace < closestDistance)
            {
                closestObject = fire;
                closestDistance = distanace;
            }
        }

        if (closestDistance < reach)
        {
            if (currentTarget != closestObject)
            {
                currentTarget = closestObject;
                reticle.SetActive(true);
                reticle.GetComponent<Animation>().Play("Target Animation");
            }
            reticle.transform.position = currentTarget.transform.position;
        }
        else
        {
            currentTarget = null;
            reticle.transform.localPosition = Vector3.zero;
            reticle.SetActive(false);
        }
    }
}
