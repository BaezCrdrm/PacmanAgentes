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
        base.OnInspectorGUI();
        MapGenerator map = target as MapGenerator;
        map.IniciarObstaculos();
        map.GenerateMap();
    }
}
