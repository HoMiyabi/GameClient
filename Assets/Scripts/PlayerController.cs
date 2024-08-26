using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 10;
    private void Update()
    {
        var move = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            move.x += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            move.x -= 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            move.z += 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            move.z -= 1;
        }
        move.Normalize();
        transform.position += speed * Time.deltaTime * move;
    }
}
