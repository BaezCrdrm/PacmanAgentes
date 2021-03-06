﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Transform obstaclePrefab;
    public Transform tilePrefab;
    public Vector2 mapSize;
    [Range(0, 1)]
    public float outlinePercent;
    public List<Coordenada> allTilesCoords { get; set; }
    public List<Coordenada> Obstaculos { get; set; }
    public List<Coordenada> Inaccesibles { get; set; }
    public Transform[,] tileMap { get; set; }
    public List<Vector2> Tunel { get; set; }

    public MapGenerator() { }

    // Start is called before the first frame update
    void Start()
    {
        IniciarObstaculos();
        GenerateMap();
    }

    public void IniciarObstaculos()
    {
        Obstaculos = new List<Coordenada>();
        Inaccesibles = new List<Coordenada>();
        if(mapSize.x <= 19 && mapSize.y <= 22)
        {
            // Borde alto y bajo
            for(int i = 0; i < mapSize.x; i++)
            {
                Obstaculos.Add(new Coordenada(i,0));
                Obstaculos.Add(new Coordenada(i,(int)mapSize.y - 1));
            }
            

            // Lados
            for(int i = 1; i < 8; i++)
            {
                Obstaculos.Add(new Coordenada(0,i));
                Obstaculos.Add(new Coordenada(18,i));

                if(i < 7)
                {
                    int _y = (int)mapSize.y - 1 + (i * -1);
                    Obstaculos.Add(new Coordenada(0, _y));
                    Obstaculos.Add(new Coordenada(18, _y));
                }
            }

            // Saliente en lados
            for(int i = 0; i < 4; i++)
            {
                Obstaculos.Add(new Coordenada(i, 14));
                Obstaculos.Add(new Coordenada(i, 12));
                Obstaculos.Add(new Coordenada(i, 10));
                Obstaculos.Add(new Coordenada(i, 8));

                int _x = (int)mapSize.x - 1 + (i * -1);
                Obstaculos.Add(new Coordenada(_x, 14));
                Obstaculos.Add(new Coordenada(_x, 12));
                Obstaculos.Add(new Coordenada(_x, 10));
                Obstaculos.Add(new Coordenada(_x, 8));

                if(i == 3)
                {
                    Obstaculos.Add(new Coordenada(i, 13));
                    Obstaculos.Add(new Coordenada(i, 9));
                    Obstaculos.Add(new Coordenada(_x, 13));
                    Obstaculos.Add(new Coordenada(_x, 9));
                }
            }
        
            // Especiales
            Obstaculos.Add(new Coordenada(9, (int)mapSize.y - 2));
            Obstaculos.Add(new Coordenada(9, (int)mapSize.y - 3));
            Obstaculos.Add(new Coordenada(9, (int)mapSize.y - 4));

            // Figura 1
            Obstaculos.Add(new Coordenada(5, 16));
            Obstaculos.Add(new Coordenada(5, 15));
            Obstaculos.Add(new Coordenada(5, 14));
            Obstaculos.Add(new Coordenada(6, 14));
            Obstaculos.Add(new Coordenada(7, 14));
            Obstaculos.Add(new Coordenada(5, 13));
            Obstaculos.Add(new Coordenada(5, 12));

            // Figura 2
            Obstaculos.Add(new Coordenada(13, 16));
            Obstaculos.Add(new Coordenada(13, 15));
            Obstaculos.Add(new Coordenada(13, 14));
            Obstaculos.Add(new Coordenada(12, 14));
            Obstaculos.Add(new Coordenada(11, 14));
            Obstaculos.Add(new Coordenada(13, 13));
            Obstaculos.Add(new Coordenada(13, 12));

            // Figura 3
            Obstaculos.Add(new Coordenada(7, 8));
            Obstaculos.Add(new Coordenada(8, 8));
            Obstaculos.Add(new Coordenada(9, 8));
            Obstaculos.Add(new Coordenada(9, 7));
            Obstaculos.Add(new Coordenada(9, 6));
            Obstaculos.Add(new Coordenada(10, 8));
            Obstaculos.Add(new Coordenada(11, 8));

            // Figura 4
            Obstaculos.Add(new Coordenada(11, 2));
            Obstaculos.Add(new Coordenada(12, 2));
            Obstaculos.Add(new Coordenada(13, 2));
            Obstaculos.Add(new Coordenada(13, 3));
            Obstaculos.Add(new Coordenada(13, 4));
            Obstaculos.Add(new Coordenada(14, 2));
            Obstaculos.Add(new Coordenada(15, 2));

            // Líneas
            for(int i = 0; i < 2; i++)
            {
                int val = i == 0 ? 5 : 13;
                for(int j = 8; j <= 10; j++) { 
                    Obstaculos.Add(new Coordenada(val, j));
                }
            }

            Obstaculos.Add(new Coordenada(1, 4));
            Obstaculos.Add(new Coordenada((int)mapSize.x - 2, 4));

            for(int i = 0; i < 3; i++)
            {
                Inaccesibles.Add(new Coordenada(i, 9));
                Inaccesibles.Add(new Coordenada(i, 13));
                Inaccesibles.Add(new Coordenada((int)mapSize.x - 1 + (i * -1), 9));
                Inaccesibles.Add(new Coordenada((int)mapSize.x - 1 + (i * -1), 13));
            }
        }
    }

    public void GenerateMap()
    {
        allTilesCoords = new List<Coordenada>();
        tileMap = new Transform[(int)mapSize.x, (int)mapSize.y];
        for(int x = 0; x < mapSize.x; x++)
        {
            for(int y = 0; y < mapSize.y; y++)
            {
                allTilesCoords.Add(new Coordenada(x, y));
            }
        }

        string holderName = "Mapa generado";
        if(transform.Find(holderName))
        {
            // Se utiliza porque se llama del editor
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;
        
        for(int x = 0; x < mapSize.x; x++)
        {
            for(int y = 0; y < mapSize.y; y++)
            {
                // Posición en la que aparecerá el tile.
                Vector3 tilePosition = new Vector3(-mapSize.x/2 + 0.5f + x, 0, -mapSize.y/2 + 0.5f + y);
                // Hace una instancia de tile prefab
                Transform tile = Instantiate(tilePrefab, tilePosition, 
                        Quaternion.Euler(Vector3.right*90)) 
                    as Transform;
                tile.localScale = Vector3.one * (1 - outlinePercent);
                tile.parent = mapHolder;
            }
        }

        foreach (Coordenada coord in Obstaculos)
        {
            Vector3 posicion = obtienePosicion(coord.x, coord.y);
            Transform obstaculo = Instantiate(obstaclePrefab, posicion + Vector3.up * 0.5f, Quaternion.identity) as Transform;
            obstaculo.parent = mapHolder;
        }

        Tunel = new List<Vector2>();
        for (int i = 0; i < 3; i++)
        {
            Tunel.Add(new Vector2(i, 11));
            Tunel.Add(new Vector2(18 - i, 11));
        }
    }

    Vector3 obtienePosicion(int x, int y)
    {
        return new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y);
    }

    /// <summary>
    /// Verifica si un ente cualquiera puede moverse a 
    /// una posición propuesta.
    /// </summary>
    /// <return>
    /// Verdadero si puede hacer el movimiento.
    /// </return>
    public bool PuedeMoverseA(Vector2 _coords)
    {
        try
        {
            Vector3 vals = new Vector3(_coords.x, 0, _coords.y);
            if(!Utility.IsInPosition(vals, Obstaculos)
                && !Utility.IsInPosition(vals, Inaccesibles))
                return true;
            else return false;
        }
        catch (System.Exception)
        {
            return false;
        }
    }

    public Coordenada[] PuedeMoverseA(Vector2 _coords, bool[] _pm)
    {
        // try
        // {
        //     Vector3 vals = new Vector3(_coords.x, 0, _coords.y);
        //     if(!Utility.IsInPosition(vals, Obstaculos)
        //         && !Utility.IsInPosition(vals, Inaccesibles))
        //         return true;
        //     else return false;
        // }
        // catch (System.Exception)
        // {
        //     return false;
        // }
        Coordenada[] movimientosValidos = new Coordenada[4];

        int j = 3;
        for(int i = 1; i <= 7; i += 2)
        {
            if(_pm[j])
            {
                Vector2 posicion = Utility.ObtieneCoordenadasRelativas(i);
                int x = (int)(_coords.x + posicion.x);
                int y = (int)(_coords.y + posicion.y);
                bool obst = Utility.IsInPosition(
                    new Vector3(x, 0, y), Obstaculos);
                bool ina = Utility.IsInPosition(
                    new Vector3(x, 0, y), Inaccesibles);

                if (!obst && !ina)
                    movimientosValidos[j] = new Coordenada(x, y);
            }

            if(j == 3) j = 0;
            else j++;
        }

        return movimientosValidos;
    }

    /// <summary>
    /// Regresa el vector correcto para lograr 
    /// "atravesar el tunel".
    ///</summary>
    public Vector3 AtraviesaTunel(Vector3 _posicion)
    {
        int salida = (int)(mapSize.x / 2);
        if(_posicion.x > salida && _posicion.z == 0.5)
            _posicion.x = -salida;
        else if(_posicion.x < -salida && _posicion.z == 0.5)
            _posicion.x = salida;

        return _posicion;
    }

    // Genera las coordenadas para los tiles en el arreglo
    public struct Coordenada
    {
        public int x;
        public int y;

        public Coordenada(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }
}
