using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class VectorExtensionMethods
{
    public static Vector3 AddXYZ(this Vector3 _vec3, float _x = 0, float _y = 0, float _z = 0)
    {
        return new Vector3(_vec3.x + _x, _vec3.y + _y, _vec3.z + _z);
    }

    public static Vector3 AddX(this Vector3 _vec3, float _dx)
    {
        return new Vector3(_vec3.x + _dx, _vec3.y, _vec3.z);
    }

    public static Vector3 AddY(this Vector3 _vec3, float _dy)
    {
        return new Vector3(_vec3.x, _vec3.y + _dy, _vec3.z);
    }

    public static Vector3 AddZ(this Vector3 _vec3, float _dz)
    {
        return new Vector3(_vec3.x, _vec3.y, _vec3.z + _dz);
    }

    public static Vector2 AddXY(this Vector2 _vec2, float _x = 0, float _y = 0)
    {
        return new Vector2(_vec2.x + _x, _vec2.y + _y);
    }



    public static Vector3 SetX(this Vector3 _vec3, float _x)
    {
        return new Vector3(_x, _vec3.y, _vec3.z);
    }

    public static Vector3 SetY(this Vector3 _vec3, float _y)
    {
        return new Vector3(_vec3.x, _y, _vec3.z);
    }

    public static Vector3 SetZ(this Vector3 _vec3, float _z)
    {
        return new Vector3(_vec3.x, _vec3.y,  _z);
    }

    public static Vector3 Format(this Vector3 _vec3, int _digits)
    {
        return new Vector3((float)System.Math.Round(_vec3.x, _digits),
                           (float)System.Math.Round(_vec3.y, _digits),
                           (float)System.Math.Round(_vec3.z, _digits));
    }
}