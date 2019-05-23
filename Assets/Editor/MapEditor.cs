using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(MapGenerator))]
public class MapEditor : Editor
{
    // Start is called before the first frame update
    public override void OnInspectorGUI()
    {
        MapGenerator map = target as MapGenerator;
        if(DrawDefaultInspector())
        {
            map.IniciarObstaculos();
            map.GenerateMap();
        }

        if(GUILayout.Button("Recargar Mapa"))
        {
            map.IniciarObstaculos();
            map.GenerateMap();
        }
    }
}
