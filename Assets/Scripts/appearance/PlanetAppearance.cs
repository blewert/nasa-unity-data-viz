using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetAppearance
{
    /// <summary>
    /// The diameter of the planet in km
    /// </summary>
    public float diameter;

    /// <summary>
    /// The name of the planet
    /// </summary>
    public string name;

    /// <summary>
    /// Constructs a new PlanetAppearance object
    /// </summary>
    /// <param name="name"></param>
    public PlanetAppearance(string name)
    {
        //Set the name
        this.name = name;
    }
}