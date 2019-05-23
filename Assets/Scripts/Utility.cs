using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static Vector3 GetRandomVector3(int _maxX, int _maxY)
    {
        float x = (float)Random.Range((-_maxX / 2) + 1, _maxX / 2);
        float y = (float)Random.Range((-_maxY / 2) + 1, _maxY / 2);

        if(_maxX % 2 == 0) x += 0.5f;
        if(_maxY % 2 == 0) y += 0.5f;
        
        return new Vector3(x, 0.5f, y);
    }

    public static bool IsInPosition(Vector3 _obj1, Vector3 _obj2)
    {
        return (_obj1.x == _obj2.x && _obj1.y == _obj2.y) ? true : false;
    }

    public static bool IsInPosition(Vector3 _obj1, List<Vector3> _obj2, int cx = 0, int cy = 0)
    {
        int total = _obj2.FindAll(p => p.x == _obj1.x && p.y == _obj1.z).Count;
        return total > 0 ? true : false;
    }

    public static bool IsInPosition(Vector3 _obj1, List<MapGenerator.Coordenada> _obj2, 
        int cx = 0, int cy = 0)
    {
        int x, y;
        // _obj1 = new Vector3(6f, 0.5f, 3.5f);
        if(_obj1.x % 2 != 0) _obj1.x -= 0.5f;
        if(_obj1.z % 2 != 0) _obj1.z -= 0.5f;

        x = (int)_obj1.x + cx;
        y = (int)_obj1.z + cy;

        int total = _obj2.FindAll(p => p.x == x && p.y == y).Count;
        return total > 0 ? true : false;
    }
}
