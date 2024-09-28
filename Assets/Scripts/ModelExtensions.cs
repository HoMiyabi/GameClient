using Proto;
using UnityEngine;

public static class ModelExtensions
{
    // public static void SetFromProto(this Transform self, Proto.NEntity other)
    // {
    //     var p = other.Position;
    //     var d = other.Direction;
    //
    //     self.position = new Vector3(p.X * 0.001f, p.Y * 0.001f, p.Z * 0.001f);
    //
    //     self.rotation = Quaternion.Euler(d.X * 0.001f, d.Y * 0.001f, d.Z * 0.001f);
    // }

    public static NFloat3 Set(this NFloat3 self, Vector3 other)
    {
        self.X = other.x;
        self.Y = other.y;
        self.Z = other.z;
        return self;
    }

    public static ref Vector3 Set(this ref Vector3 self, NFloat3 other)
    {
        self.x = other.X;
        self.y = other.Y;
        self.z = other.Z;
        return ref self;
    }

    // public static void SetFromNative(this NInt3 self, Vector3 other)
    // {
    //     self.X = (int)(other.x * 1000);
    //     self.Y = (int)(other.y * 1000);
    //     self.Z = (int)(other.z * 1000);
    // }
    //
    // public static void SetFromProto(this ref Vector3 self, NInt3 other)
    // {
    //     self.x = other.X * 0.001f;
    //     self.y = other.Y * 0.001f;
    //     self.z = other.Z * 0.001f;
    // }
    //
    // public static void SetFromNative(this NEntity self, GameEntity other)
    // {
    //     self.EntityId = other.entityId;
    //     self.Position.SetFromNative(other.position);
    //     self.Direction.SetFromNative(other.direction);
    // }
    //
    // public static void SetFromProto(this GameEntity self, NEntity other)
    // {
    //     self.entityId = other.EntityId;
    //     self.position.SetFromProto(other.Position);
    //     self.direction.SetFromProto(other.Direction);
    // }
}