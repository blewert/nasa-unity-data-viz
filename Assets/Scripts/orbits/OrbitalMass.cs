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
        this.orbitalElements = orbitalElements;

        //Initialise
        this.Initialise();
    }

    // 
    // THESE FUNCTIONS BELOW (apart from Update) WERE NOT MADE BY ME
    // THEY ARE ENTIRELY MADE BY @alexisgea. See here for more details:
    // <https://github.com/alexisgea/Solar-System-Simulation/blob/master/Assets/Scripts/Space%20elements/OrbitalBody.cs>
    // 
    // They're just here until I can understand how to calculate this myself. It's been a great help though!
    //

    /// <summary>
    /// Computes the Eccentric Anomaly. Angles to be passed in Radians.
    /// The Newton method used to solve E implies getting a first guess and itterating until the value is precise enough.
    /// </summary>
    /// <returns>The Eccentric Anomaly.</returns>
    /// <param name="e">Eccentricity.</param>
    /// <param name="M">Mean anomaly.</param>
    /// <param name="dp">Decimal precision.</param>
    private float EccentricAnomaly(float M, int dp = 5)
    {
        // Mathematical Model is as follow:
        // E(n+1) = E(n) - f(E) / f'(E)
        // f(E) = E - e * sin(E) - M
        // f'(E) = 1 - e * cos(E)
        // we are happy when f(E)/f'(E) is small enough.

        int maxIter = 20;  // we make sure we won't loop too much
        int i = 0;
        float e = this.orbitalElements.eccentricity;
        float precision = Mathf.Pow(10, -dp);
        float E, F;

        // If the eccentricity is high we guess the Mean anomaly for E, otherwise we guess PI.
        E = (e < 0.8) ? M : Mathf.PI;
        F = E - e * Mathf.Sin(M) - M;  //f(E)

        // We will interate until f(E) higher than our wanted precision (as devided then by f'(E)).
        while ((Mathf.Abs(F) > precision) && (i < maxIter))
        {
            E = E - F / (1f - e * Mathf.Cos(E));
            F = E - e * Mathf.Sin(E) - M;
            i++;
        }

        return E;
    }

    /// <summary>
    /// Computes the True Anomaly. Angles to be passed in Radians.
    /// </summary>
    /// <returns>The True Anomaly.</returns>
    /// <param name="E">Eccentric Anomaly.</param>
    private float TrueAnomaly(float E)
    {
        // from wikipedia we can find several way to solve TA from E.
        // I tried sin(TA) = (sqrt(1-e*e) * sin(E))/(1 -e*cos(E)) but it didn't work properly for some reason,
        // so I sued the following as one of the my sources(jgiesen.de/Kepler) tan(TA) = (sqrt(1-e*e) * sin(E)) / (cos(E) - e).

        float e = this.orbitalElements.eccentricity;
        float numerator = Mathf.Sqrt(1f - e * e) * Mathf.Sin(E);
        float denominator = Mathf.Cos(E) - e;
        float TA = Mathf.Atan2(numerator, denominator);

        return TA;
    }

    /// <summary>
    /// Compute a point's position in a given orbit. All angles are to be passed in Radians.
    /// </summary>
    /// <returns>The point position.</returns>
    /// <param name="M">Mean anomaly.</param>
    public Vector3 GetPosition(float M, float scale = 1f)
    {
        float e = orbitalElements.e;
        float a = orbitalElements.a; // semiMajorAxis
        float N = orbitalElements.ascendingLongitude * Mathf.Deg2Rad; // not const as might vary with precession
        float w = orbitalElements.perihelionLongitude * Mathf.Deg2Rad;
        float i = orbitalElements.i * Mathf.Deg2Rad;

        float E = EccentricAnomaly(M);
        float TA = TrueAnomaly(E);
        float focusRadius = a * (1 - Mathf.Pow(e, 2f)) / (1 + e * Mathf.Cos(TA));

        // parametric equation of an elipse using the orbital elements
        float X = scale * focusRadius * (Mathf.Cos(N) * Mathf.Cos(TA + w) - Mathf.Sin(N) * Mathf.Sin(TA + w)) * Mathf.Cos(i);
        float Y = scale * focusRadius * Mathf.Sin(TA + w) * Mathf.Sin(i);
        float Z = scale * focusRadius * (Mathf.Sin(N) * Mathf.Cos(TA + w) + Mathf.Cos(N) * Mathf.Sin(TA + w)) * Mathf.Cos(i);

        Vector3 orbitPoint = new Vector3(X, Y, Z);

        return orbitPoint;
    }

    protected void Initialise()
    {
        //Set-up according to initial orbital elements
        transform.Rotate(Vector3.forward * (orbitalElements.inclination));
        transform.Rotate(Vector3.up * orbitalElements.ascendingLongitude);

    }

    public float speed = 50;

    public Vector3 ComputeOrbitPositionRaw(float deg, float scale = 1f)
    {
        var position = GetPosition(deg * Mathf.Deg2Rad, scale);
        return transform.parent.TransformPoint(position);
    }

    public Vector3 ComputeOrbitPosition(float globalScale, float deg)
    {
        return ComputeOrbitPositionRaw(Time.time * deg, globalScale);        
    }

    public void Update(float globalScale)
    {
        transform.position = this.ComputeOrbitPosition(globalScale, speed);
    }
}
