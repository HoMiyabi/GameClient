using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntity : MonoBehaviour
{
    public int entityId;
    public Vector3 position;
    public Vector3 direction;

    private void Update()
    {
        transform.position = position;
        transform.rotation = Quaternion.Euler(direction);
    }

    public void SetData(Proto.NEntity entity)
    {
        entityId = entity.Id;
        var p = entity.Position;
        var d = entity.Direction;
        position = new Vector3(p.X, p.Y, p.Z);
        direction = new Vector3(d.X, d.Y, d.Z);
        position *= 0.001f;
        direction *= 0.001f;
    }
}
