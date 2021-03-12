using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalMass
{
    /// <summary>
    /// The object in the scene
    /// </summary>
    public GameObject gameObject;

    /// <summary>
    /// The orbital elements for this mass
    /// </summary>
    public KeplerianOrbit orbitalElements;

    /// <summary>
    /// The transform of this obj
    /// </summary>
    /// <value></value>
    public Transform transform 
    {
        get { return gameObject.transform; } 
    }

    /// <summary>
    /// Constructs a new orbital mass
    /// </summary>
    /// <param name="gameObject"></param>
    public OrbitalMass(in GameObject gameObject, in KeplerianOrbit orbitalElements)
    {
        //Set gameobject ref to gameobject passed
        this.gameObject = gameObject;
    }
}
