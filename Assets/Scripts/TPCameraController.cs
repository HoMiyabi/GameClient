using UnityEngine;

public class TPCameraController : MonoBehaviour
{
    public Transform follow;

    [Header("灵敏度")]
    public float mouseSensitivity = 1f;
    public float pitchSensitivity = 0.022f;
    public float yawSensitivity = 0.022f;
    public float wheelSensitivity = 0.1f;

    [Header("距离")]
    public float distance = 5f;
    public float minDistance = 2f;
    public float maxDistance = 8f;

    public Vector3 offset;
    public float hitGoOutDistance = 0f;

    [Header("旋转")]
    public float pitch = 0f; // 俯仰角
    public float minPitch = -85f;
    public float maxPitch = 85f;
    public float yaw = 0f; // 偏航角

    private void Start()
    {
        transform.rotation = Quaternion.identity;
        pitch = 0f;
        yaw = 0f;
    }

    public void LateUpdate()
    {
        // Pitch
        float dy = Input.GetAxis("Mouse Y");
        if (dy != 0f)
        {
            pitch += -dy * pitchSensitivity * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        // Yaw
        float dx = Input.GetAxis("Mouse X");
        if (dx != 0f)
        {
            yaw += dx * yawSensitivity * mouseSensitivity;
            while (yaw < -180f)
            {
                yaw += 360f;
            }
            while (yaw > 180f)
            {
                yaw -= 360f;
            }
        }

        // Distance
        float dWheel = Input.mouseScrollDelta.y;
        if (dWheel != 0f)
        {
            distance += -dWheel * wheelSensitivity;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
        }

        // Rotation
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Position
        if (follow != null)
        {
            var expectedPosition = follow.position + offset - transform.forward * distance;
            if (Physics.Linecast(follow.position + offset, expectedPosition, out RaycastHit hit))
            {
                transform.position = hit.point + hit.normal * hitGoOutDistance;
            }
            else
            {
                transform.position = expectedPosition;
            }
        }
    }
}