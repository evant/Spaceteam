using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeBehavior : MonoBehaviour
{
    public static ShakeBehavior instance;
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.3f;
    private float dampingSpeed = 2.0f;
    Vector3 initialPosition;

    // Start is called before the first frame update
    void Start() 
    {
        initialPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update() 
    {
        if (shakeDuration > 0) 
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude; 
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else 
        {
            shakeDuration = 0f;
            transform.localPosition = initialPosition;
        }
    }

    public void TriggerShake() 
    {
        shakeDuration = 2.0f;
    }

}
