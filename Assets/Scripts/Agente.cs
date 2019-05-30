using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agente : MonoBehaviour
{
    public Vector2 Posicion { get; set; }
    public MapGenerator Mapa { get; set; }
    protected bool PrimeraVuelta = true;
    public bool[] Censo { get; protected set; }
    public bool[] PosiblesMovimientos { get; protected set; }

    protected virtual IEnumerator Start()
    {
        Censo = new bool[8];
        // Obtener coordenadas relativas al mapa
        ActualizaPosicion();

        return null;
    }

    protected void ActualizaPosicion()
    {
        float x = Mapa.mapSize.x / 2;
        float y = Mapa.mapSize.y / 2;

        Posicion = new Vector2(this.transform.position.x + x - 0.5f, this.transform.position.z + y - 0.5f);
    }

    /// <summary>
    /// Actualiza la evaluación de los posibles 
    /// movimientos que el agente puede tomar.
    /// 0 -> Este
    /// 1 -> Sur
    /// 2 -> Oeste
    /// 3 -> Norte
    ///
    /// </summary>
    public void EvaluaMovimiento()
    {
        Censo = new bool[8];
        for(int i = 0; i < 8; i++)
        {
            Vector2 coords = Utility.ObtieneCoordenadasRelativas(i);
            coords.x += Posicion.x;
            coords.y += Posicion.y;

            try
            {
                if(Mapa.PuedeMoverseA(coords))
                    Censo[i] = true;
                else Censo[i] = false;
            }
            catch (System.Exception ex)
            {
                Debug.Log("Ocurrió un error al evaluar movimiento\n" 
                    + ex.Message);
                Censo[i] = false;
            }
        }
    }

    public void MoverA(int _m)
    {
        /*
        0 -> Este
        1 -> Sur
        2 -> Oeste
        3 -> Norte
        */

        if (_m == 0)
        {
            Vector3 position = this.transform.position;
            position.x++;
            position = Mapa.AtraviesaTunel(position);
            this.transform.position = position;
        }
        if (_m == 1)
        {
            Vector3 position = this.transform.position;
            position.z--;
            this.transform.position = position;
        }
        if (_m == 2)
        {
            Vector3 position = this.transform.position;
            position.x--;
            position = Mapa.AtraviesaTunel(position);
            this.transform.position = position;
        }
        if (_m == 3)
        {
            Vector3 position = this.transform.position;
            position.z++;
            this.transform.position = position;
        }
    }
}
