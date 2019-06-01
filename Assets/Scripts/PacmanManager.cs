using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanManager : Agente
{
    public List<FantasmaManager> Fantasmas { get; set; }
    public bool Alcanzado { get; set; }
    public PacmanManager() { }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        // MovimientoManual();

        if (!EnUso)
        {
            if (!Alcanzado)
                StartCoroutine(Rutina(0.25f));
        }
    }

    IEnumerator Rutina(float delay)
    {
        EnUso = true;
        if (!PrimeraVuelta && !Alcanzado)
        {
            // Evaluar movimientos
            EvaluaMovimiento();

            // Obtener los posibles movimientos
            PosiblesMovimientos = Utility.SeleccionaMovimiento(Censo);

            // Medir la distancia en todos los posibles movimientos
            //      Obtener las coordenadas de los posibles movimientos
            MapGenerator.Coordenada[] coordenadas = Mapa.PuedeMoverseA(Posicion, PosiblesMovimientos);

            // Elegir el más cercano (greedy)
            float mejor = -1;
            Vector3 mejorCoordenada = new Vector3(this.Posicion.x, this.transform.position.y, this.Posicion.y);

            int j = -1;

            for (int i = 0; i < 4; i++)
            {
                try
                {
                    if (PosiblesMovimientos[i])
                    {
                        bool breakLoop = false;
                        Vector3 c = new Vector3(coordenadas[i].x, this.transform.position.y, coordenadas[i].y);

                        foreach (FantasmaManager fantasma in Fantasmas)
                        {
                            try
                            {
                                float val = Utility.ManhattanDistance(c, fantasma.Posicion);

                                if (val > mejor)
                                {
                                    mejor = val;
                                    j = i;
                                    mejorCoordenada = c;
                                    Debug.Log(System.String.Format("Nuevo mejor valor: {0}", val));
                                }
                            } catch(NullReferenceException nre)
                            {
                                Debug.Log("Error al seleccionar movimiento.\n" + nre.Message);
                            }
                        }

                        if (breakLoop) break;
                    }
                }
                catch (Exception) { }
            }

            // Moverse al seleccinado
            //      Bloquear posición anterior
            Anterior = Posicion;
            MoverA(j);

            // Repetir
        }
        else if (Alcanzado)
        {
            Debug.Log("Alcanzado: " + Alcanzado);
        }
        else
        {
            PrimeraVuelta = false;
            Alcanzado = false;
            delay = 0f;
        }

        yield return new WaitForSeconds(delay);
        EnUso = false;
    }

    private void MovimientoManual()
    {
        if (PrimeraVuelta)
        {
            PrimeraVuelta = false;
            // Código
        }
        else
        {
            int m = -1;
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                m = 2;
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                m = 0;
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                m = 3;
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                m = 1;

            if (m != -1) MoverA(m);
        }
    }
}
