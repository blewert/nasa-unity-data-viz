using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KeplerianOrbit
{
    // name, a, e, i, omega deg, omega ~deg, deg L
    //..

    /// <summary>
    /// The "name" of this orbit, i.e. whose orbit this actually is
    /// </summary>
    /// <returns></returns>
    public string name = default(string);

    /// <summary>
    /// The semi-major axis. Half of the major axis (longest axis).
    /// </summary>
    /// <returns></returns>
    public float semiMajorAxis = default(float);

    /// <summary>
    /// Eccentricity: how elliptical the orbit is.
    /// </summary>
    /// <returns></returns>
    public float eccentricity = default(float);

    /// <summary>
    /// The inclination of the orbital against the equatorial plane.
    /// </summary>
    /// <returns></returns>
    public float inclination = default(float);

    /// <summary>
    /// Ascending node longitude.
    /// </summary>
    /// <returns></returns>
    public float ascendingLongitude = default(float);

    /// <summary>
    /// Longitude of the perihelion (closest apsis)
    /// </summary>
    /// <returns></returns>
    public float perihelionLongitude = default(float);

    /// <summary>
    /// The mean longitude.
    /// </summary>
    /// <returns></returns>
    public float meanLongitude = default(float);


    //Abbreviations / aliases
    //..

    #region Aliases

    ///
    public float a 
    {
        get { return this.semiMajorAxis; }
    }

    public float e
    {
        get { return this.eccentricity; }
    }

    public float i
    {
        get { return this.inclination; }
    }

    public float omega
    {
        get { return this.ascendingLongitude; }
    }

    // "~omega"
    public float notOmega
    {
        get { return this.perihelionLongitude; }
    }

    public float L
    {
        get { return this.meanLongitude; }
    }

    #endregion
}
