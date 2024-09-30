using Proto;
using UnityEngine;

public static class ModelExtensions
{
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

    public static Vector3 Native(this NFloat3 self)
    {
        return new Vector3(self.X, self.Y, self.Z);
    }
}