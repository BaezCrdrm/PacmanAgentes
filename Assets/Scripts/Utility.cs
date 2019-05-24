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
        return (_obj1.x == _obj2.x && _obj1.z == _obj2.z) ? true : false;
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

    public static bool IsInPosition(Vector3 _obj1, List<FantasmaManager> _obj2, Vector3 _pacman)
    {
        if(_obj2.Count > 0)
        {
            int total = _obj2.FindAll(p => p != null && p.transform.position.x == _obj1.x
                && p.transform.position.z == _obj1.z).Count;

            total += IsInPosition(_obj1, _pacman) ? 1 : 0;

            return total > 0 ? true : false;
        } else return false;
    }

    ///<summary>
    /// Función de movimiento presentado por la profesora.
    /// Selecciona los movimientos para rodear un obstáculo.
    ///</summary>
    public static int SeleccionaMovimiento(bool[] Censo)
    {
        bool[] PosiblesMovimientos = new bool[4];

        PosiblesMovimientos[0] = !Censo[1] || !Censo[2];
        PosiblesMovimientos[1] = !Censo[3] || !Censo[4];
        PosiblesMovimientos[2] = !Censo[5] || !Censo[6];
        PosiblesMovimientos[3] = !Censo[7] || !Censo[0];

        // printMovimientos();
        int val = -1;

        if (PosiblesMovimientos[0] == true && PosiblesMovimientos[1] == false)
            val = 3;
        else if (PosiblesMovimientos[1] == true && PosiblesMovimientos[2] == false)
            val = 5;
        else if (PosiblesMovimientos[2] == true && PosiblesMovimientos[3] == false)
            val = 7;
        else if (PosiblesMovimientos[3] == true && PosiblesMovimientos[0] == false)
            val = 1;
        else val = 1;

        return val;
    }

    public static float ManhattanDistance(Agente _obj1, Agente _obj2)
    {
        return Mathf.Abs(_obj1.Posicion.x - _obj2.Posicion.x) + 
            Mathf.Abs(_obj1.Posicion.y - _obj2.Posicion.y);
    }
}
