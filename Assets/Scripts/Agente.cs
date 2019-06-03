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
    public Vector2 Anterior { get; set; }
    public bool Alcanzado { get; set; }
    public bool EnUso { get; set; }

    protected virtual void Start()
    {
        Censo = new bool[8];
        // Obtener coordenadas relativas al mapa
        ActualizaPosicion();
        Anterior = Posicion;
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
    public void EvaluaMovimiento(List<MapGenerator.Coordenada> coordenadas = null)
    {
        Censo = new bool[8];
        for(int i = 0; i < 8; i++)
        {
            Vector2 coords = Utility.ObtieneCoordenadasRelativas(i);
            coords.x += Posicion.x;
            coords.y += Posicion.y;

            try
            {
                // Poner aquí Condición de la posición anterior
                if(Mapa.PuedeMoverseA(coords))
                {
                    if (coordenadas == null)
                        Censo[i] = true;
                    else if (coordenadas != null && !Utility.IsInList(
                            new MapGenerator.Coordenada((int)coords.x, (int)coords.y),
                            coordenadas))
                        Censo[i] = true;
                    else Censo[i] = false;
                }
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

    private bool EstaEnUltimaPosicion(Vector2 coordenada)
    {
        if (Anterior.x == coordenada.x && Anterior.y == coordenada.y)
            return true;
            else return false;
    }

    ///<summary>
    /// Función de movimiento presentado por la profesora.
    /// Selecciona los movimientos para rodear un obstáculo.
    ///</summary>
    public bool[] SeleccionaMovimiento(List<FantasmaManager> _fantasmas)
    {
        bool[] PosiblesMovimientos = new bool[4];

        PosiblesMovimientos[0] = Censo[3] && MovimientoLibreDeFantasmas(_fantasmas, 3);
        PosiblesMovimientos[1] = Censo[5] && MovimientoLibreDeFantasmas(_fantasmas, 5);
        PosiblesMovimientos[2] = Censo[7] && MovimientoLibreDeFantasmas(_fantasmas, 7);
        PosiblesMovimientos[3] = Censo[1] && MovimientoLibreDeFantasmas(_fantasmas, 1);

        // printMovimientos();

        return PosiblesMovimientos;
    }

    protected bool MovimientoLibreDeFantasmas(List<FantasmaManager> _fantasmas, int _movimiento)
    {
        int totalFantasmas = 0;
        int[] Movs = { _movimiento + 1 > 7 ? 0 : _movimiento + 1, _movimiento - 1 };

        if (GetType() == typeof(PacmanManager))
        {
            // Checa Posibles intersecciones de diagonales
            for (int i = 0; i < Movs.Length; i++)
            {
                Vector2 coords = Utility.ObtieneCoordenadasRelativas(Movs[i]);
                coords.x += Posicion.x;
                coords.y += Posicion.y;

                totalFantasmas += _fantasmas.FindAll(p => p.Posicion == coords).Count;
            }
        }

        // Checa posibles intersecciones rectas
        for (int k = 1; k <= 2; k++)
        {
            Vector2 coords = Utility.ObtieneCoordenadasRelativas(_movimiento);
            coords.x = (coords.x * k) + Posicion.x;
            coords.y = (coords.y * k) + Posicion.y;

            totalFantasmas += _fantasmas.FindAll(p => p.Posicion == coords).Count;

            if(GetType() == typeof(FantasmaManager)) break;
        }
        
        return totalFantasmas > 0 ? false : true;
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

        ActualizaPosicion();
    }
}
