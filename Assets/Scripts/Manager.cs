using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Transform MapaPrefab, PacmanPrefab, FantasmaPrefab;
    public int FantasmasGenerados;
    MapGenerator mapa;
    PacmanManager pac;
    FantasmaManager f1;
    bool PrimeraVuelta = true;

    void Start()
    {
        mapa = Instantiate(MapaPrefab, new Vector3(0, 0.5f, 0), Quaternion.identity).GetComponent<MapGenerator>();
    }

    private Vector3 GetValidRandomVector3()
    {
        Vector3 temp;
        do {
            temp = Utility.GetRandomVector3((int)mapa.mapSize.x, (int)mapa.mapSize.y);
        } while(Utility.IsInPosition(temp, mapa.Obstaculos, (int)(mapa.mapSize.x / 2), (int)(mapa.mapSize.y / 2)) || 
            Utility.IsInPosition(temp, mapa.Inaccesibles, (int)(mapa.mapSize.x / 2), (int)(mapa.mapSize.y / 2)));

        return temp;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(PrimeraVuelta)
        {
            Debug.Log(mapa.allTilesCoords.Count, this);

            pac = Instantiate(PacmanPrefab, GetValidRandomVector3(), Quaternion.identity).GetComponent<PacmanManager>();
            pac.Mapa = mapa;
            f1 = Instantiate(FantasmaPrefab, GetValidRandomVector3(), Quaternion.identity).GetComponent<FantasmaManager>();
            f1.Objetivo = pac;
            f1.Mapa = mapa;

            PrimeraVuelta = false;
        } else
        {
            
        }
    }
}
