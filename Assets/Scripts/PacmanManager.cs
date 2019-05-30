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
    protected override IEnumerator Start()
    {
        base.Start();
        // yield return new WaitForSeconds(5.0f);
        return null;
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
            int m = -1;
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
               m = 2;
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                m = 0;
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                m = 3;
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                m = 1;

            if(m != -1) MoverA(m);
        }
    }
}
