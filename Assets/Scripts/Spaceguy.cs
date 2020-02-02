using UnityEngine;
using UnityEngine.InputSystem;

public class Spaceguy : MonoBehaviour
{
    public float speed = 6.0f;
    public float reach = 0.8f;

    public Color shirtColor1;
    public Color shirtColor2;
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private GameObject reticle;
    private GameObject currentTarget;
    private Animator animator;
    private PlayerInput playerInput;

    private Texture2D mColorSwapTex;
    private Color[] mSpriteColors;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;

        reticle = FindChildTagged("Reticle");
        reticle.GetComponent<SpriteRenderer>().color = shirtColor1;

        animator = GetComponent<Animator>();

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

        SwapColor(SwapIndex.Shirt1, shirtColor1);
        SwapColor(SwapIndex.Shirt2, shirtColor2);
        colorSwapTex.Apply();

        playerInput = GetComponent<PlayerInput>();
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

    private void Update()
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

            if (canMove)
            {
                transform.Translate(movement);
            }

            if (movement.y < 0)
            {
                animator.SetInteger("direction", 1);
            }
            else if (movement.y > 0)
            {
                animator.SetInteger("direction", 2);
            }
            else if (movement.x > 0)
            {
                animator.SetInteger("direction", 3);
            }
            else
            {
                animator.SetInteger("direction", 4);
            }
        }
        else
        {
            animator.SetInteger("direction", 0);
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
