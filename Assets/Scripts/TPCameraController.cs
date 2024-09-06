using UnityEngine;

public class TPCameraController : MonoBehaviour
{
    [Header("跟随对象")]
    public Transform follow;
    public Vector3 offset;

    [Header("灵敏度")]
    public float mouseSensitivity = 40f;
    public float pitchSensitivity = 0.022f;
    public float yawSensitivity = 0.022f;
    public float wheelSensitivity = 0.2f;

    [Header("距离")]
    public float distance = 5f;
    public float minDistance = 2f;
    public float maxDistance = 8f;

    [Header("旋转")]
    public float pitch = 0f; // 俯仰角
    public float minPitch = -70f;
    public float maxPitch = 70f;
    public float yaw = 0f; // 偏航角

    private Camera _camera;
    private Vector3[] nearCorners;

    private void Start()
    {
        transform.rotation = Quaternion.identity;
        pitch = 0f;
        yaw = 0f;

        nearCorners = new Vector3[4];

        _camera = GetComponent<Camera>();
        _camera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), _camera.nearClipPlane,
            Camera.MonoOrStereoscopicEye.Mono, nearCorners);
    }

    public void LateUpdate()
    {
        // Pitch
        float dy = Input.GetAxis("Mouse Y");
        pitch += -dy * pitchSensitivity * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Yaw
        float dx = Input.GetAxis("Mouse X");
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
        float dWheel = Input.mouseScrollDelta.y;
        distance += -dWheel * wheelSensitivity;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        // Rotation
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Position
        if (follow != null)
        {
            var expectedPosition = follow.position + offset - transform.forward * distance;
            transform.position = expectedPosition;

            float minHitDistance = float.PositiveInfinity;
            foreach (Vector3 nearCorner in nearCorners)
            {
                var wsNearCorner = transform.TransformPoint(nearCorner);
                Vector3 cornerOffset = wsNearCorner - transform.position - transform.forward * _camera.nearClipPlane;
                Vector3 startPosition = follow.position + offset + cornerOffset;
                Debug.DrawLine(startPosition, wsNearCorner, Color.red);
                if (Physics.Linecast(startPosition, wsNearCorner, out var hit))
                {
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
                transform.position = follow.position + offset - transform.forward * (minHitDistance + _camera.nearClipPlane);
            }
        }
    }
}