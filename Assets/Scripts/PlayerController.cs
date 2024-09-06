using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10;

    private void Update()
    {
        var input = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.A))
        {
            input.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            input.x += 1;
        }
        if (Input.GetKey(KeyCode.W))
        {
            input.z += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            input.z -= 1;
        }
        if (input != Vector3.zero)
        {
            input.Normalize();

            var tpCameraController = Camera.main.GetComponent<TPCameraController>();
            var rot = Quaternion.Euler(0f, tpCameraController.yaw, 0f);
            var move = rot * input;

            transform.position += speed * Time.deltaTime * move;

            // Rotate Player
            Quaternion targetRot = rot * Quaternion.LookRotation(input, Vector3.up);
            transform.DOKill();
            transform.DORotateQuaternion(targetRot, 0.1f);
        }
    }
}
