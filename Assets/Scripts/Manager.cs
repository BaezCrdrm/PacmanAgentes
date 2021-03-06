﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Transform MapaPrefab, PacmanPrefab, FantasmaPrefab;
    public int NumeroFantasmasGenerados = 3;
    public int NumeroDeFantasmasRodeandoAObjetivo = 2;

    MapGenerator mapa;
    PacmanManager pac;
    List<FantasmaManager> Fantasmas;
    bool PrimeraVuelta = true;
    bool Detener;
    AudioSource audioData;

    void Start()
    {
        Fantasmas = new List<FantasmaManager>();
        for (int i = 0; i < NumeroFantasmasGenerados; i++)
        {
            Fantasmas.Add(new FantasmaManager());
        }
        mapa = Instantiate(MapaPrefab, new Vector3(0, 0.5f, 0), Quaternion.identity).GetComponent<MapGenerator>();
        audioData = GetComponent<AudioSource>();
    }

    private Vector3 GetValidRandomVector3(bool fant = true)
    {
        Vector3 temp;
        bool evaluaFantasmas = false;
        do {
            temp = Utility.GetRandomVector3((int)mapa.mapSize.x, (int)mapa.mapSize.y);

            // Para pruebas
            //if(fant)
            //{
            //    temp = new Vector3(11 / 2, 0.5f, 0.5f);
            //} else
            //{
            //    // temp = new Vector3(15 / 2, 0.5f, 16 / 2 + 0.5f);
            //    temp = new Vector3(-13 / 2, 0.5f, 0.5f);
            //}
            // Fin de seccion de pruebas

            evaluaFantasmas = fant ? Utility.IsInPosition(temp, 
                Fantasmas, pac.transform.position) : false;
        } while(Utility.IsInPosition(temp, mapa.Obstaculos, (int)(mapa.mapSize.x / 2), (int)(mapa.mapSize.y / 2)) || 
            Utility.IsInPosition(temp, mapa.Inaccesibles, (int)(mapa.mapSize.x / 2), (int)(mapa.mapSize.y / 2)) ||
            evaluaFantasmas);

        return temp;
    }

    void LateUpdate()
    {
        if(PrimeraVuelta)
        {
            PrimeraVuelta = false;
            // Debug.Log(mapa.allTilesCoords.Count, this);

            pac = Instantiate(PacmanPrefab, GetValidRandomVector3(false), Quaternion.identity).GetComponent<PacmanManager>();
            pac.Mapa = mapa;

            for(int i = 0; i < Fantasmas.Count; i++)
            {
                Fantasmas[i] = Instantiate(FantasmaPrefab, GetValidRandomVector3(), Quaternion.identity).GetComponent<FantasmaManager>();
                Fantasmas[i].Objetivo = pac;
                Fantasmas[i].Mapa = mapa;
            }

            ActualizaUbicacionFantasmas();
            pac.Fantasmas = Fantasmas;
        } else
        {
            if (!Detener)
            {
                // Esta condición puede cambiar
                int total = Fantasmas.FindAll(p => p.Alcanzado == true).Count;
                ActualizaUbicacionFantasmas();
                pac.Fantasmas = Fantasmas;

                if (total >= NumeroDeFantasmasRodeandoAObjetivo || EsPacmanComido())
                {
                    Finalizar();
                    pac.Morir();
                }
                else Detener = false;
            }
        }
    }

    private bool EsPacmanComido()
    {
        return Fantasmas.FindAll(p => p.Posicion == pac.Posicion).Count > 0 ? true : false;
    }

    private void ActualizaUbicacionFantasmas()
    {
        Fantasmas.ForEach(i =>
        {
            i.Companeros = Fantasmas;
        });
    }

    private void Finalizar()
    {
        Detener = true;
        Fantasmas.ForEach(i => i.Alcanzado = true);
        pac.Alcanzado = true;
        audioData.Play(0);
    }
}
