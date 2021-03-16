using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetVisualiser : MonoBehaviour
{
    /// <summary>
    /// Where are all the planets created under in the hierarchy? well, here.
    /// </summary>
    /// <returns></returns>
    protected GameObject planetParentObj = default(GameObject);

    /// <summary>
    /// Where are all the planet objects? well, here.
    /// </summary>
    /// <typeparam name="GameObject"></typeparam>
    /// <returns></returns>
    [HideInInspector]
    public List<OrbitalMass> planets = new List<OrbitalMass>();

    /// <summary>
    /// Called when the orbit data has been read from KeplerianOrbitReader.cs.
    /// </summary>
    public void OnOrbitsDataRead()
    {
        //Get the orbits!
        var orbits = GetComponent<KeplerianOrbitReader>().orbits;
        
        //Create planets from these orbits
        this.CreatePlanetsFromOrbits(in orbits);

        //Create orbit paths
        this.CreateOrbitPathVisualisation();
    }

    public void CreateOrbitPathVisualisation()
    {
        foreach(var planet in planets)
        {
            //Last position
            var lastPos = default(Vector3);

            //For each planet
            for(float a = 0f; a < 360f; a += 1f)
            {
                //Compute mean anomaly pos
                var pos = planet.ComputeOrbitPositionRaw(a);

                //Now render the line
                if(lastPos != default(Vector3))
                    Debug.DrawLine(lastPos, pos, Color.yellow, 10f);

                //Set last position to current position
                lastPos = pos;
            }
        }
    }

    /// <summary>
    /// Creates planets in-game from the orbital elements that were read.
    /// </summary>
    /// <param name="orbits"></param>
    public void CreatePlanetsFromOrbits(in List<KeplerianOrbit> orbits)
    {
        //Create a new parent object
        planetParentObj = new GameObject("planets");

        foreach(var orbit in orbits)
        {
            //Create a new object
            var planetObj = CreatePlanet(in orbit);

            //Add to list of planets
            planets.Add(planetObj);
        }
    }

    /// <summary>
    /// Creates a planet from a given orbit
    /// </summary>
    /// <param name="orbit"></param>
    public OrbitalMass CreatePlanet(in KeplerianOrbit orbit)
    {
        //For now just create a sphere and return it
        var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        //Set name to orbit name
        obj.name = orbit.name;

        //Set parent
        obj.transform.SetParent(planetParentObj.transform);

        //And return!
        return new OrbitalMass(in obj, in orbit);
    }

    public void Update()
    {
        //No planets? do nothing
        if(planets.Count == 0)
            return;

        //Otherwise, orbit!
        foreach(var planet in planets)
            planet.Update();
    }
}
