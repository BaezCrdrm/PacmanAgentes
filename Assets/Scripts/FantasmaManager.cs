using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FantasmaManager : Agente
{
    public PacmanManager Objetivo { get; set; }

    public FantasmaManager() { }

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if(Objetivo != null)
        {
            
        }
    }
}
