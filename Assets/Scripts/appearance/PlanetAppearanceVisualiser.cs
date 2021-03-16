using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetAppearanceVisualiser : MonoBehaviour
{
    /// <summary>
    /// The reader
    /// </summary>
    public PlanetAppearanceReader reader;

    /// <summary>
    /// The parent gameobject for the instantiated planets
    /// </summary>
    public PlanetVisualiser planetVisualiserField;

    /// <summary>
    /// Planet appearance options for visualisation
    /// </summary>
    [System.Serializable, SerializeField]
    public struct PlanetAppearanceOptions
    {
        public float diameterScale;
    }

    /// <summary>
    /// Appearance options instance
    /// </summary>
    public PlanetAppearanceOptions options;

    public void OnAppearancesRead()
    {
        //Loop through each
        foreach(Transform planetObj in planetVisualiserField.planetParentObj.transform)
        {
            foreach(PlanetAppearance appearance in reader.planetAppearances)
            {
                if(planetObj.name.Equals(appearance.name))
                {
                    Debug.Log("A match!");
                    break;
                }
            }
        }
    }
}