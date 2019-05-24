using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanManager : Agente
{
    public List<FantasmaManager> Fantasmas { get; set; }
    public bool Alcanzado { get; set; }
    public PacmanManager()
    {
        Fantasmas = new List<FantasmaManager>();
        Alcanzado = false;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if(PrimeraVuelta)
        {
            PrimeraVuelta = false;
            // Código
        } else
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                Vector3 position = this.transform.position;
                position.x--;
                position = Mapa.AtraviesaTunel(position);
                this.transform.position = position;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                Vector3 position = this.transform.position;
                position.x++;
                position = Mapa.AtraviesaTunel(position);
                this.transform.position = position;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                Vector3 position = this.transform.position;
                position.z++;
                this.transform.position = position;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                Vector3 position = this.transform.position;
                position.z--;
                this.transform.position = position;
            }
        }
    }
}
