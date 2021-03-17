using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetAppearanceReader : CSVDataReader 
{
    /// <summary>
    /// The number of columns in the CSV.
    /// </summary>
    public const uint PLANET_APPEARANCE_NUMBER_OF_COLUMNS = 3;

    /// <summary>
    /// Function to call when finished reading
    /// </summary>
    [SerializeField]
    public UnityEngine.Events.UnityEvent onFinishedRead;

    /// <summary>
    /// The list of "appearance" values for each planet.
    /// </summary>
    /// <typeparam name="PlanetAppearance"></typeparam>
    /// <returns></returns>
    public List<PlanetAppearance> planetAppearances = new List<PlanetAppearance>();
    
    public override void ParseLine(in string line)
    {
        //Split the line
        var splitLine = line.Split(',');

        //There should be 7 elements
        if (splitLine.Length != PLANET_APPEARANCE_NUMBER_OF_COLUMNS)
            throw new UnityException($"Couldn't parse CSV file for orbital elements, expected {PLANET_APPEARANCE_NUMBER_OF_COLUMNS} columns but found {splitLine.Length}.");

        //Otherwise, parse!
        string name = splitLine[0].Trim();
        float diameter = float.Parse(splitLine[1]);
        string texturePath = splitLine[2].Trim();

        //Make a new planetappearance object
        var appearance = new PlanetAppearance(name);
        appearance.diameter = diameter;
        appearance.texturePath = texturePath;

        //Add to list
        planetAppearances.Add(appearance);
    }

    public override void Close()
    {
        //Call base close
        base.Close();

        //And invoke finished reading callback
        onFinishedRead.Invoke();
    }
}
