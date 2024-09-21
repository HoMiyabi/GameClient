using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class TPCameraController : MonoBehaviour
{
    [Header("跟随对象")]
    public Vector3 offset;

    [Header("灵敏度")]
    public float mouseSensitivity = 8f;
    public float pitchSensitivity = 0.022f;
    public float yawSensitivity = 0.022f;
    public float zoomSensitivity = 0.01f;

    [Header("距离")]
    public float targetDistance = 4.4f;
    public float curDistance = 4.4f;
    public float minDistance = 2f;
    public float maxDistance = 8f;
    public float zoomDuration = 0.1f;
    private TweenerCore<float, float, FloatOptions> zoomTweener;

    [Header("旋转")]
    public float pitch = 0f; // 俯仰角
    public float minPitch = -70f;
    public float maxPitch = 70f;
    public float yaw = 0f; // 偏航角

    private Camera _camera;
    private Vector3[] nearCorners;

    private bool freeMouse = true;

    private InputControls input;

    private void Awake()
    {
        _camera = Camera.main;

        _camera.transform.rotation = Quaternion.identity;
        pitch = 0f;
        yaw = 0f;

        nearCorners = new Vector3[4];

        _camera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), _camera.nearClipPlane,
            Camera.MonoOrStereoscopicEye.Mono, nearCorners);

        input = new InputControls();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    public void SetCamera(Camera cam)
    {
        _camera = cam;

        _camera.transform.rotation = Quaternion.identity;
        pitch = 0f;
        yaw = 0f;

        _camera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), _camera.nearClipPlane,
            Camera.MonoOrStereoscopicEye.Mono, nearCorners);
    }

    public void LateUpdate()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
        }

        // Input
        float dZoom = 0f;
        Vector2 dRot = Vector2.zero;

        freeMouse = input.Player.FreeMouse.IsPressed();
        if (freeMouse)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;

            dRot = input.Player.Rotate.ReadValue<Vector2>();
            dZoom = input.Player.Zoom.ReadValue<float>();
        }
        float dy = dRot.y;
        float dx = dRot.x;

        // Pitch
        pitch += -dy * pitchSensitivity * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Yaw
        yaw += dx * yawSensitivity * mouseSensitivity;
        while (yaw < -180f)
        {
            yaw += 360f;
        }
        while (yaw > 180f)
        {
            yaw -= 360f;
        }

        // Distance
        float newTargetDistance;
        newTargetDistance = targetDistance - dZoom * zoomSensitivity;
        newTargetDistance = Mathf.Clamp(newTargetDistance, minDistance, maxDistance);
        if (Mathf.Abs(newTargetDistance - targetDistance) > Mathf.Epsilon)
        {
            targetDistance = newTargetDistance;
            zoomTweener?.Kill();
            zoomTweener = DOTween.To(() => curDistance, x => curDistance = x,
                targetDistance, zoomDuration);
        }

        // Rotation
        _camera.transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Position

        var expectedPosition = transform.position + offset - _camera.transform.forward * curDistance;
        _camera.transform.position = expectedPosition;

        float minHitDistance = float.PositiveInfinity;
        foreach (Vector3 nearCorner in nearCorners)
        {
            var wsNearCorner = _camera.transform.TransformPoint(nearCorner);
            Vector3 cornerOffset = wsNearCorner - _camera.transform.position - _camera.transform.forward * _camera.nearClipPlane;
            Vector3 startPosition = transform.position + offset + cornerOffset;

            Debug.DrawLine(startPosition, wsNearCorner, Color.green);

            if (Physics.Linecast(startPosition, wsNearCorner, out var hit, ~LayerMask.GetMask("Actor")))
            {
                DebugUtils.DrawCross(hit.point, 0.2f, Color.red);
                minHitDistance = Mathf.Min(minHitDistance, hit.distance);
            }
        }

        // Debug.DrawLine(follow.position + offset, expectedPosition, Color.red);
        // if (Physics.Linecast(follow.position + offset, expectedPosition, out hit))
        // {
        //     minHitDistance = Mathf.Min(minHitDistance, hit.distance);
        // }

        if (!float.IsPositiveInfinity(minHitDistance))
        {
            _camera.transform.position = transform.position + offset - _camera.transform.forward * (minHitDistance + _camera.nearClipPlane);
        }
    }
}