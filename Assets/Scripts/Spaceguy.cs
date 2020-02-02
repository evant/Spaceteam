using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Spaceguy : MonoBehaviour
{
    public float speed = 6.0f;
    public float reach = 0.8f;
    public bool deadzo = false;
    public float respawnTime = 8.0f;

    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private GameObject reticle;
    private GameObject repairBar;
    private GameObject currentTarget;
    private Animator animator;
    private PlayerInput playerInput;

    private Texture2D mColorSwapTex;
    private Color[] mSpriteColors;
    private float repairProgress;
    private float respawnRemaining;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;

        animator = GetComponent<Animator>();

        reticle = FindChildTagged("Reticle");
        repairBar = FindChildTagged("Repair");

        var spriteRenderer = GetComponent<SpriteRenderer>();
        Texture2D colorSwapTex = new Texture2D(256, 1, TextureFormat.RGBA32, false, false);
        colorSwapTex.filterMode = FilterMode.Point;
        for (int i = 0; i < colorSwapTex.width; ++i)
        {
            colorSwapTex.SetPixel(i, 0, new Color(0.0f, 0.0f, 0.0f, 0.0f));
        }
        spriteRenderer.material.SetTexture("_SwapTex", colorSwapTex);
        mSpriteColors = new Color[colorSwapTex.width];
        mColorSwapTex = colorSwapTex;

        playerInput = GetComponent<PlayerInput>();
        switch (playerInput.playerIndex)
        {
            case 0:
                setShirtColor(new Color(0.988f, 0.996f, 1f), new Color(0.859f, 0.859f, 0.859f));
                break;
            case 1:
                setShirtColor(new Color(0.996f, 0.91f, 0.2f), new Color(0.851f, 0.776f, 0.176f));
                break;
            case 2:
                setShirtColor(new Color(0.216f, 1f, 0.149f), new Color(0.161f, 0.741f, 0.11f));
                break;
            case 3:
                setShirtColor(new Color(0.976f, 0.161f, 1f), new Color(0.71f, 0.114f, 0.725f));
                break;
        }


        Debug.Log("on player joined: " + playerInput.playerIndex);
    }

    public async void SetDead(bool dead)
    {
        
        GetComponent<Animator>().SetBool("alive", !dead);
        // If we're coming back to life, wait a bit before letting us move
        if (!dead)
        {
            await Task.Delay(300);
        }
        deadzo = dead;
        currentTarget = null;

        reticle.transform.localPosition = Vector3.zero;
        reticle.SetActive(false);
        respawnRemaining = respawnTime;
    }
    
    public enum SwapIndex
    {
        Shirt1 = 0xfc,
        Shirt2 = 0xdb,
    }

    private void SwapColor(SwapIndex index, Color color)
    {
        mSpriteColors[(int)index] = color;
        mColorSwapTex.SetPixel((int)index, 0, color);
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

    private void setShirtColor(Color color1, Color color2) { 
        SwapColor(SwapIndex.Shirt1, color1);
        SwapColor(SwapIndex.Shirt2, color2);
        mColorSwapTex.Apply();

        reticle.GetComponent<SpriteRenderer>().color = color1;
    }

    private void Update()
    {
        if(!deadzo)
        {
            var action = playerInput.currentActionMap["action"].ReadValue<float>() > 0.5f;
            UpdateMovement(action);
            UpdateTargeting(action);
        }
        else
        {
            respawnRemaining -= Time.deltaTime;
            if(respawnRemaining < 0)
            {
                SetDead(false);
            }
        }
    }

    public enum Direction { 
        DOWN = 1,
        UP = 2,
        RIGHT = 3,
        LEFT = 4,
    }

    private void UpdateMovement(bool action)
    {
        var moveDirection = playerInput.currentActionMap["move"].ReadValue<Vector2>();
        if (moveDirection.magnitude > 0.0f)
        {
            var canMove = true;
            var movement = moveDirection * speed * Time.deltaTime;
            var center = new Vector2(transform.position.x, transform.position.y - 0.2f);
            var hit = Physics2D.Raycast(center, moveDirection.normalized);
            if (hit.collider != null)
            {
                if (hit.distance < movement.magnitude + 0.2)
                {
                    canMove = false;
                }
            }

            if (canMove && !action)
            {
                transform.Translate(movement);
            }

            if (movement.y < 0)
            {
                animator.SetInteger("direction", (int) Direction.DOWN);
            }
            else if (movement.y > 0)
            {
                animator.SetInteger("direction", (int) Direction.UP);
            }
            else if (movement.x > 0)
            {
                animator.SetInteger("direction", (int) Direction.RIGHT);
            }
            else
            {
                Debug.Log("left!");
                animator.SetInteger("direction", (int) Direction.LEFT);
            }
        }
        else
        {
            animator.SetInteger("direction", 0);
        }
        animator.SetBool("shooting", action);
    }

    private void UpdateTargeting(bool action)
    {
        FindNextTarget();

        var currentProblem = currentTarget?.GetComponent<Problem>();

        if (currentProblem != null && action)
        {
            if (repairProgress == 0)
            {
                repairProgress = 100f;
            }
            else
            {
                repairProgress -= currentProblem.repairAmount * Time.deltaTime;
            }
            repairBar.transform.localScale = new Vector3(repairProgress / 100f, 0.1f, 1f);
            if (repairProgress <= 0)
            {
                Destroy(currentTarget);
                currentTarget = null;
            }
        }
        else
        {
            repairProgress = 0;
            repairBar.transform.localScale = Vector3.zero;
        }
        if (currentTarget != null && action)
        {
            var targetDirection = Vector2.SignedAngle(currentTarget.transform.position - transform.position, Vector2.up);

            if (targetDirection >= -45 && targetDirection <= 45)
            {
                animator.SetInteger("direction", (int)Direction.UP);
            }
            else if (targetDirection >= 180 - 45 && targetDirection <= 180 + 45)
            {
                animator.SetInteger("direction", (int)Direction.DOWN);
            }
            else if (targetDirection > 0)
            {
                animator.SetInteger("direction", (int)Direction.RIGHT);
            }
            else
            {
                animator.SetInteger("direction", (int)Direction.LEFT);
            }
        }
    }

private void FindNextTarget()
    {
        var hazards = GameObject.FindGameObjectsWithTag("Hazard");
        float closestDistance = float.PositiveInfinity;
        GameObject closestObject = null;
        foreach (var hazard in hazards)
        {
            var alien = hazard.GetComponent<Alien>();
            if (alien != null && alien.dieing)
                continue;   // Don't target dieing aliens you monster!

            var distanace = Vector3.Distance(hazard.transform.position, transform.position);
            
            if (distanace < closestDistance)
            {
                closestObject = hazard;
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

        var targetAlien = currentTarget?.GetComponent<Alien>();
        if(targetAlien != null && playerInput.currentActionMap["action"].ReadValue<float>() > 0.5f)
        {
            targetAlien.Die();
            currentTarget = null;
        }
    }
}
