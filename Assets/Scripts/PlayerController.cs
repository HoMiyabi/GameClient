using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public float normalSpeed = 8;
    public float speedUpSpeed = 12;

    private HeroAnimations heroAnimations;

    private InputControls input;

    public float jumpForce = 15000;

    private void Awake()
    {
        heroAnimations = GetComponent<HeroAnimations>();
        input = new InputControls();
    }

    private void OnEnable()
    {
        input.Player.Enable();
        input.Player.Jump.started += context =>
        {
            print("Jump");
            var _rigidbody = GetComponent<Rigidbody>();
            if (_rigidbody != null)
            {
                _rigidbody.AddForce(Vector3.up * jumpForce);
            }
        };
    }

    private void Disable()
    {
        input.Player.Disable();
    }

    private void Update()
    {
        // Input
        bool bSpeedUp = input.Player.SpeedUp.IsPressed();
        bool bAttack = input.Player.Attack.IsPressed();
        Vector2 inputMove = input.Player.Move.ReadValue<Vector2>();
        Vector3 localMove = new Vector3(inputMove.x, 0f, inputMove.y);

        if (bAttack)
        {
            heroAnimations.PlayAttack1();
        }

        float targetSpeed = normalSpeed;
        if (bSpeedUp)
        {
            targetSpeed = speedUpSpeed;
        }

        if (localMove != Vector3.zero)
        {
            var tpCameraController = GetComponent<TPCameraController>();
            var rot = Quaternion.Euler(0f, tpCameraController.yaw, 0f);
            Vector3 wsMove = rot * localMove;

            heroAnimations.PlayRun();
            if (heroAnimations.state == HeroAnimations.HState.Run)
            {
                transform.position += targetSpeed * Time.deltaTime * wsMove;
            }

            // Rotate Player
            Vector3 wsForward = wsMove;
            Quaternion targetRot = Quaternion.LookRotation(wsForward, Vector3.up);
            transform.DOKill();
            transform.DORotateQuaternion(targetRot, 0.1f);
        }
        else
        {
            heroAnimations.PlayIdle();
        }
    }
}
