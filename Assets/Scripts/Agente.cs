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

    protected virtual void Start()
    {
        Censo = new bool[8];
    }

    /// <summary>
    /// Actualiza la evaluación de los posibles 
    /// movimientos que el agente puede tomar.
    /// </summary>
    public void EvaluaMovimiento()
    {
        Censo = new bool[8];
        for(int i = 0; i < 8; i++)
        {
            Vector2 coords = ObtieneCoordenadasRelativas(i);
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

    private Vector2 ObtieneCoordenadasRelativas(int i)
    {
        Vector2 vals = new Vector2(0,0);

        switch (i)
        {
            case 0:
                vals.x = -1;
                vals.y = -1;
                break;

            case 1:
                vals.x = 0;
                vals.y = -1;
                break;

            case 2:
                vals.x = 1;
                vals.y = -1;
                break;

            case 3:
                vals.x = 1;
                vals.y = 0;
                break;

            case 4:
                vals.x = 1;
                vals.y = 1;
                break;

            case 5:
                vals.x = 0;
                vals.y = 1;
                break;

            case 6:
                vals.x = -1;
                vals.y = 1;
                break;

            case 7:
                vals.x = -1;
                vals.y = 0;
                break;
        }

        return vals;
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
