using UnityEngine;

public class Spaceguy : MonoBehaviour
{
    public float speed = 6.0f;
    private Vector3 moveDirection = Vector3.zero;

    private void Start()
    {
    }

    private void Update()
    {
        moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveDirection *= speed;
        gameObject.transform.position += moveDirection * Time.deltaTime;
    }
}
