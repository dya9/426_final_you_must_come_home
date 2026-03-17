// lets make him move
// using __ imports namespace
// Namespaces are collection of classes, data types
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// MonoBehavior is the base class from which every Unity Script Derives
public class PlayerMovement : MonoBehaviour
{
    public float speed = 25.0f;
    public float rotationSpeed = 90;
    public float force = 700f;

    Rigidbody rb;
    Transform t;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        t = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // Time.deltaTime represents the time that passed since the last frame
        //the multiplication below ensures that GameObject moves constant speed every frame
        if (Keyboard.current.wKey.isPressed)
            rb.linearVelocity += t.forward * speed * Time.deltaTime;
        else if (Keyboard.current.sKey.isPressed)
            rb.linearVelocity -= t.forward * speed * Time.deltaTime;

        // Quaternion returns a rotation that rotates x degrees around the x axis and so on
        if (Keyboard.current.dKey.isPressed)
            t.rotation *= Quaternion.Euler(0f, rotationSpeed * Time.deltaTime, 0f);
        else if (Keyboard.current.aKey.isPressed)
            t.rotation *= Quaternion.Euler(0f, -rotationSpeed * Time.deltaTime, 0f);
        
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            rb.AddForce(t.up * force);
    }
}