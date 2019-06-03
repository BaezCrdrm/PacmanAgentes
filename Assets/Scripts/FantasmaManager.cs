using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FantasmaManager : Agente
{
    public PacmanManager Objetivo { get; set; }
    public List<MapGenerator.Coordenada> Cerrados { get; set; }
    public List<FantasmaManager> Companeros { get; set; }

    public FantasmaManager() { }

    protected override void Start()
    {
        base.Start();
    }

    private void FixedUpdate()
    {
        if(!EnUso)
        {
            if(!Alcanzado)
                StartCoroutine(Rutina(0.25f));
        }
    }

    IEnumerator Rutina(float delay)
    {
        EnUso = true;
        if (Objetivo != null)
        {
            if (!PrimeraVuelta)
            {
                // Evaluar movimientos
                EvaluaMovimiento();

                // Obtener los posibles movimientos
                PosiblesMovimientos = SeleccionaMovimiento(Companeros);

                // Medir la distancia en todos los posibles movimientos
                //      Obtener las coordenadas de los posibles movimientos
                MapGenerator.Coordenada[] coordenadas = Mapa.PuedeMoverseA(Posicion, PosiblesMovimientos);

                // Elegir el más cercano (greedy)
                float mejor = Mapa.mapSize.x * 2 + 1f;
                Vector3 mejorCoordenada = new Vector3(this.Posicion.x, this.transform.position.y, this.Posicion.y);

                int j = -1;

                for (int i = 0; i < 4; i++)
                {
                    try
                    {
                        if (PosiblesMovimientos[i] &&
                            !Utility.IsInList(coordenadas[i], Cerrados))
                        {
                            Vector3 c = new Vector3(coordenadas[i].x, this.transform.position.y, coordenadas[i].y);
                            float val = Utility.ManhattanDistance(c, Objetivo.Posicion);

                            if (val < mejor)
                            {
                                mejor = val;
                                j = i;
                                mejorCoordenada = c;
                                Debug.Log(System.String.Format("Nuevo mejor valor: {0}", val));
                            }

                            if (Mathf.Abs(val) <= 1f)
                            {
                                //Alcanzado = true;
                                // Objetivo.Alcanzado = true;
                                Objetivo.setAlcanzadoPor(1);
                                Alcanzado = Objetivo.AlcanzadoPor >= 2 ? true : false;
                                break;
                            }
                            else Objetivo.setAlcanzadoPor(-1);
                        }
                    }
                    catch (Exception) { }
                }

                // Moverse al seleccinado
                //      Bloquear posición anterior
                Anterior = Posicion;
                Cerrados.Add(new MapGenerator.Coordenada((int)Posicion.x, (int)Posicion.y));
                MoverA(j);

                //if (Cerrados.Count >= (int)Mapa.mapSize.x * 2)
                //    Cerrados.RemoveAt(0);

                if (Cerrados.Count >= 3)
                    Cerrados.RemoveAt(0);

                //if(Utility.IsInPosition)
                //{

                //}

                // Repetir
            }
            else if (Alcanzado)
            {
                Debug.Log("Alcanzado: " + Alcanzado);
            }
            else
            {
                PrimeraVuelta = false;
                Cerrados = new List<MapGenerator.Coordenada>();
                delay = 0f;
            }

            yield return new WaitForSeconds(delay);
        }
        EnUso = false;
    }

}
