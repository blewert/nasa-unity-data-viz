using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeplerianOrbitReader : CSVDataReader
{
    /// <summary>
    /// There should only ever be THIS number of columns in the CSV.
    /// </summary>
    protected const uint KEPLERIAN_ORBIT_NUMBER_OF_COLUMNS = 7;

    /// <summary>
    /// Parsed orbits
    /// </summary>
    [HideInInspector]
    public List<KeplerianOrbit> orbits = new List<KeplerianOrbit>();

    [Header("[callbacks]")]

    /// <summary>
    /// Function to call when finished reading
    /// </summary>
    [SerializeField]
    public UnityEngine.Events.UnityEvent onFinishedRead;

    public override void ParseLine(in string line)
    {
        //Split the line
        var splitLine = line.Split(',');

        //There should be 7 elements
        if(splitLine.Length != KEPLERIAN_ORBIT_NUMBER_OF_COLUMNS)
            throw new UnityException($"Couldn't parse CSV file for orbital elements, expected {KEPLERIAN_ORBIT_NUMBER_OF_COLUMNS} columns but found {splitLine.Length}.");

        //Otherwise, parse!
        string name = splitLine[0].Trim();
        float a = float.Parse(splitLine[1]);
        float e = float.Parse(splitLine[2]);
        float i = float.Parse(splitLine[3]);
        float omega = float.Parse(splitLine[4]);
        float notOmega = float.Parse(splitLine[5]);
        float L = float.Parse(splitLine[6]);

        //Make a keplerian orbit
        var orbit = new KeplerianOrbit();

        //Set them all
        orbit.name = name;
        orbit.semiMajorAxis = a;
        orbit.eccentricity = e;
        orbit.inclination = i;
        orbit.ascendingLongitude = omega;
        orbit.perihelionLongitude = notOmega;
        orbit.meanLongitude = L;

        //Add to the list!
        orbits.Add(orbit);
    }

    public override void Close()
    {
        //Call base close
        base.Close();

        //And invoke finished reading callback
        onFinishedRead.Invoke();
    }
}
