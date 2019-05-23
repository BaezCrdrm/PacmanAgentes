using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Transform MapaPrefab, PacmanPrefab, FantasmaPrefab;
    public int NumeroFantasmasGenerados = 3;
    MapGenerator mapa;
    PacmanManager pac;
    List<FantasmaManager> Fantasmas;
    bool PrimeraVuelta = true;

    void Start()
    {
        Fantasmas = new List<FantasmaManager>();
        for (int i = 0; i < NumeroFantasmasGenerados; i++)
        {
            Fantasmas.Add(new FantasmaManager());
        }
        mapa = Instantiate(MapaPrefab, new Vector3(0, 0.5f, 0), Quaternion.identity).GetComponent<MapGenerator>();
    }

    private Vector3 GetValidRandomVector3(bool fant = true)
    {
        Vector3 temp;
        bool evaluaFantasmas = false;
        do {
            temp = Utility.GetRandomVector3((int)mapa.mapSize.x, (int)mapa.mapSize.y);

            evaluaFantasmas = fant ? Utility.IsInPosition(temp, 
                Fantasmas, pac.transform.position) : false;
        } while(Utility.IsInPosition(temp, mapa.Obstaculos, (int)(mapa.mapSize.x / 2), (int)(mapa.mapSize.y / 2)) || 
            Utility.IsInPosition(temp, mapa.Inaccesibles, (int)(mapa.mapSize.x / 2), (int)(mapa.mapSize.y / 2)) ||
            evaluaFantasmas);

        return temp;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(PrimeraVuelta)
        {
            PrimeraVuelta = false;
            Debug.Log(mapa.allTilesCoords.Count, this);

            pac = Instantiate(PacmanPrefab, GetValidRandomVector3(false), Quaternion.identity).GetComponent<PacmanManager>();
            pac.Mapa = mapa;

            for(int i = 0; i < Fantasmas.Count; i++)
            {
                Fantasmas[i] = Instantiate(FantasmaPrefab, GetValidRandomVector3(), Quaternion.identity).GetComponent<FantasmaManager>();
                Fantasmas[i].Objetivo = pac;
                Fantasmas[i].Mapa = mapa;
            }
        } else
        {
            
        }
    }
}
