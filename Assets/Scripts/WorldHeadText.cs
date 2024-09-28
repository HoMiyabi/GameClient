using System;
using UnityEngine;
using UnityEngine.UI;

public class WorldHeadText : MonoBehaviour
{
    public Transform follow;
    public Vector3 offset;

    private void LateUpdate()
    {
        transform.position = follow.position + offset;
        transform.rotation = Camera.main.transform.rotation;
    }
}