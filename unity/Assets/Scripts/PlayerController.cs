using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : OVRPlayerController
{
    public bool isGrounded;
    protected Vector3 directionVector = Vector3.zero;
    private float grav;

    public void Move(Vector3 direction)
    {
        Controller.Move(direction);
    }

    public new void Jump()
    {
        isGrounded = false;
        grav = 0;
        StartCoroutine(JumpAsync(Vector3.up * JumpForce, 0.5f));
    }

    private IEnumerator JumpAsync(Vector3 peakSpeed, float time)
    {
        float elapsed = 0;
        while (elapsed < time)
        {
            Vector3 offset = Vector3.Lerp(Vector3.zero, peakSpeed, Time.deltaTime * 2 / time);
            directionVector += elapsed <= time / 2 ? offset : -offset;
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private void Update()
    {
        if (transform.position.y < 0)
        {
            Vector3 pos = transform.position;
            pos.y = 0;
            transform.position = pos;
            directionVector.y = 0;
            isGrounded = true;
        }
        else if(!isGrounded)
        {
            if(grav < 0.0001f)
            {
                grav += 0.00001f;
            }
            directionVector.y -= grav;
        }
        transform.position += directionVector;
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }
}
