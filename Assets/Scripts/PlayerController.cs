using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 8;

    private HeroAnimations heroAnimations;

    private InputControls input;

    private void Awake()
    {
        heroAnimations = GetComponent<HeroAnimations>();
        input = new InputControls();
        input.Player.Enable();
    }

    private void Update()
    {
        // Input
        bool bAttack = input.Player.Attack.IsPressed();
        Vector2 inputMove = input.Player.Move.ReadValue<Vector2>();
        Vector3 localMove = new Vector3(inputMove.x, 0f, inputMove.y);

        if (bAttack)
        {
            heroAnimations.PlayAttack1();
        }

        if (localMove != Vector3.zero)
        {
            var tpCameraController = Camera.main.GetComponent<TPCameraController>();
            var rot = Quaternion.Euler(0f, tpCameraController.yaw, 0f);
            Vector3 wsMove = rot * localMove;

            heroAnimations.PlayRun();
            if (heroAnimations.state == HeroAnimations.HState.Run)
            {
                transform.position += speed * Time.deltaTime * wsMove;
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
