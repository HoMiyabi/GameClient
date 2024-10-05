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

    private CharacterController characterController;

    private float verticalVelocity = 0f;
    public float verticalVelocityMin = -30f;

    public float jumpSpeed = 8f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        heroAnimations = GetComponent<HeroAnimations>();
        input = new InputControls();
    }

    private void OnEnable()
    {
        input.Player.Enable();
    }

    private void OnDisable()
    {
        input.Player.Disable();
    }

    private void Gravity()
    {
        if (characterController.isGrounded)
        {
            if (input.Player.Jump.IsPressed())
            {
                verticalVelocity = jumpSpeed;
            }
            else
            {
                verticalVelocity = -0.1f;
            }
        }
        else
        {
            verticalVelocity += -9.8f * Time.deltaTime;
            verticalVelocity = Mathf.Max(verticalVelocity, verticalVelocityMin);
        }
    }

    private void Update()
    {
        Gravity();
        characterController.Move(new Vector3(0, verticalVelocity * Time.deltaTime, 0));

        // Input
        bool bSpeedUp = input.Player.SpeedUp.IsPressed();
        bool bAttack = input.Player.Attack.IsPressed();
        Vector2 inputMove = input.Player.Move.ReadValue<Vector2>();
        Vector3 localMove = new Vector3(inputMove.x, 0f, inputMove.y);

        if (bAttack)
        {
            heroAnimations.PlayAttack();
        }

        float targetSpeed = normalSpeed;
        if (bSpeedUp)
        {
            targetSpeed = speedUpSpeed;
        }

        if (localMove != Vector3.zero)
        {
            var tpCameraController = GetComponent<ThirdPersonCameraController>();
            var rot = Quaternion.Euler(0f, tpCameraController.yaw, 0f);
            Vector3 wsMove = rot * localMove;

            heroAnimations.PlayRun();
            if (heroAnimations.state == HeroAnimations.HState.Run)
            {
                characterController.Move(targetSpeed * Time.deltaTime * wsMove);
            }

            // Rotate Player
            Vector3 wsForward = wsMove;
            Quaternion targetRot = Quaternion.LookRotation(wsForward, Vector3.up);
            transform.DOKill();
            transform.DORotateQuaternion(targetRot, 0.1f);
        }
        // else
        // {
        //     heroAnimations.PlayIdle();
        // }
    }
}
