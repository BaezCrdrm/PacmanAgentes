using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanManager : Agente
{
    public int distanciaSegura = 8;
    public List<FantasmaManager> Fantasmas { get; set; }
    public int AlcanzadoPor { get; private set; }
    public PacmanManager() { }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Cerrados = new List<MapGenerator.Coordenada>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // MovimientoManual();

        if (!EnUso)
        {
            if (!Alcanzado)
            {
                if (AlcanzadoPor <= 2)
                    StartCoroutine(Rutina(0.25f));
            }
        }
    }

    IEnumerator Rutina(float delay)
    {
        EnUso = true;
        if (!PrimeraVuelta)
        {
            // Evaluar movimientos
            EvaluaMovimiento();

            // Obtener los posibles movimientos
            PosiblesMovimientos = SeleccionaMovimiento(Fantasmas);

            // Medir la distancia en todos los posibles movimientos
            //      Obtener las coordenadas de los posibles movimientos
            MapGenerator.Coordenada[] coordenadas = Mapa.PuedeMoverseA(Posicion, PosiblesMovimientos);

            // Elegir el más cercano (greedy)
            int j = -1;
            float mejor = -1;
            bool enTunel = Mapa.Tunel.FindAll(p => p.x == Posicion.x && p.y == Posicion.y).Count > 0;
            if (enTunel)
            {
                if (Posicion.x > 16) j = 2;
                else if (Posicion.x < 2) j = 0;
            }

            int dsegura = 0;
            List<int> PosiblesMovimientosLibres = new List<int>();

            for (int i = 0; i < 4; i++)
            {
                dsegura = 0;
                try
                {
                    if (PosiblesMovimientos[i] &&
                            !Utility.IsInList(coordenadas[i], Cerrados))
                    {
                        //if (!enTunel)
                        //{
                        Vector3 c = new Vector3(coordenadas[i].x, this.transform.position.y, coordenadas[i].y);

                        foreach (FantasmaManager fantasma in Fantasmas)
                        {
                            try
                            {
                                float val = Utility.ManhattanDistance(c, fantasma.Posicion);

                                if (val <= distanciaSegura)
                                {
                                    if (val >= mejor)
                                    {
                                        mejor = val;
                                        PosiblesMovimientosLibres.Add(i);
                                        Debug.Log(System.String.Format("Nuevo mejor valor: {0}", val));
                                    }
                                }
                                else
                                {
                                    // Elegir posicion aleatoria
                                    dsegura++;
                                }
                            }
                            catch (NullReferenceException nre)
                            {
                                Debug.Log("Error al seleccionar movimiento.\n" + nre.Message);
                            }
                        }
                        // }
                        //else
                        //{
                        //    j = Posicion.x <= 2 ? 0 : 2;
                        //    if(Posicion.x <= 2)
                        //    break;
                        //}
                    }
                }
                catch (Exception) { }
            }

            // Obtiene movimiento aleatorio si es que la distancia segura
            // está activada
            if (dsegura >= Fantasmas.Count)
            {
                for (int i = 0; i < 4; i++)
                {
                    int randomValue = UnityEngine.Random.Range(0, 4);
                    if (PosiblesMovimientos[randomValue])
                    {
                        j = randomValue;
                        break;
                    }
                }
            }
            else
            {
                // Obtiene movimiento válido (totalmente filtrado) ALEATORIO.
                if (PosiblesMovimientosLibres.Count > 1)
                {
                    int randomIndex = UnityEngine.Random.Range(0, PosiblesMovimientosLibres.Count);
                    j = PosiblesMovimientosLibres[randomIndex];
                }
                else j = PosiblesMovimientosLibres[0];
            }

            // Moverse al seleccinado
            //      Bloquear posición anterior
            Anterior = Posicion;
            Cerrados.Add(new MapGenerator.Coordenada((int)Posicion.x, (int)Posicion.y));
            MoverA(j);

            if (Cerrados.Count > 0)
                Cerrados.Clear();

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

    public void setAlcanzadoPor(int n)
    {
        AlcanzadoPor += n;
        Alcanzado = AlcanzadoPor >= 2 ? true : false;
        if (AlcanzadoPor < 0) AlcanzadoPor = 0;
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
