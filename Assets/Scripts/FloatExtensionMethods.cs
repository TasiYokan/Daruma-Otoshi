using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class FloatExtensionMethods
{
    public static float EPS = 0.00001f;

    public static int Sgn(this float x)
    {
        return (x > EPS ? 1 : 0) - (x < -EPS ? 1 : 0);
    }
    
    public static bool FloatEquals(this float _lhs, float _rhs)
    {
        return (_lhs - _rhs).Sgn() == 0 ? true : false;
    }

    public static bool FloatLess(this float _lhs, float _rhs)
    {
        return (_lhs - _rhs).Sgn() < 0 ? true : false;
    }

    public static bool FloatNoGreater(this float _lhs, float _rhs)
    {
        return (_lhs - _rhs).Sgn() <= 0 ? true : false;
    }

    public static bool FloatGreater(this float _lhs, float _rhs)
    {
        return (_lhs - _rhs).Sgn() > 0 ? true : false;
    }

    public static bool FloatNoLess(this float _lhs, float _rhs)
    {
        return (_lhs - _rhs).Sgn() >= 0 ? true : false;
    }
}
