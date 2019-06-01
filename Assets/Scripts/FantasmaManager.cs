using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FantasmaManager : Agente
{
    public PacmanManager Objetivo { get; set; }
    public List<MapGenerator.Coordenada> Cerrados { get; set; }

    public FantasmaManager() { }

    protected override IEnumerator Start()
    {
        base.Start();
        //while(true)
        //{
        //    if (Objetivo != null)
        //    {
        //        if (!PrimeraVuelta)
        //        {
        //            // Modificar la cuestión del censo y la selección de 
        //            // documentos


        //            // Evaluar movimientos
        //            EvaluaMovimiento();

        //            // Obtener los posibles movimientos
        //            PosiblesMovimientos = Utility.SeleccionaMovimiento(Censo);

        //            // Medir la distancia en todos los posibles movimientos
        //            //      Obtener las coordenadas de los posibles movimientos
        //            MapGenerator.Coordenada[] coordenadas = Mapa.PuedeMoverseA(Posicion, PosiblesMovimientos);

        //            // Elegir el más cercano (greedy)
        //            float mejor = Mapa.mapSize.x * 2 + 1f;
        //            Vector3 mejorCoordenada = new Vector3(this.Posicion.x, this.transform.position.y, this.Posicion.y);

        //            int j = -1;

        //            for (int i = 0; i < 4; i++)
        //            {
        //                try
        //                {
        //                    float val = Utility.ManhattanDistance(this, Objetivo);
        //                    // Poner aquí la condición para evitar que vuelva a la posición inmediata anterior.
        //                    if (val < mejor)
        //                    {
        //                        mejor = val;
        //                        j = i;
        //                        mejorCoordenada = new Vector3(coordenadas[i].x, this.transform.position.y, coordenadas[i].y);
        //                        Debug.Log(System.String.Format("Nuevo mejor valor: {0}", val));
        //                    }
        //                }
        //                catch (Exception ex) { }
        //            }
        //            // Moverse al seleccinado
        //            //      Bloquear posición anterior
        //            MoverA(j);
        //            Posicion = mejorCoordenada;

        //            // Repetir
        //            yield return new WaitForSeconds(5.0f);
        //        }
        //        else PrimeraVuelta = false;
        //    }
        //}
        return null;
    }

    void LateUpdate()
    {
        if (Objetivo != null)
        {
            if (!PrimeraVuelta && !Alcanzado)
            {
                // Modificar la cuestión del censo y la selección de 
                // documentos


                // Evaluar movimientos
                EvaluaMovimiento();

                // Obtener los posibles movimientos
                PosiblesMovimientos = Utility.SeleccionaMovimiento(Censo);

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
                                Alcanzado = true;
                                Objetivo.Alcanzado = true;
                                break;
                            }
                        }
                    }
                    catch (Exception ex) { }
                }

                // Moverse al seleccinado
                //      Bloquear posición anterior
                Anterior = Posicion;
                Cerrados.Add(new MapGenerator.Coordenada((int)Posicion.x, (int)Posicion.y));
                MoverA(j);
                // Posicion = mejorCoordenada;
                ActualizaPosicion();

                if (Cerrados.Count >= (int)Mapa.mapSize.x * 2)
                    Cerrados.RemoveAt(0);
                
                //if(Utility.IsInPosition)
                //{

                //}

                // Repetir
            }
            else if(Alcanzado)
            {
                Debug.Log("Alcanzado: " + Alcanzado);
            }
            else
            {
                PrimeraVuelta = false;
                Cerrados = new List<MapGenerator.Coordenada>();
            }
        }
    }
}
