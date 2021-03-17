using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class PlanetAppearanceVisualiser : MonoBehaviour
{
    /// <summary>
    /// The reader
    /// </summary>
    public PlanetAppearanceReader reader;

    /// <summary>
    /// The parent gameobject for the instantiated planets
    /// </summary>
    public GameObject planetParentObj;

    /// <summary>
    /// Planet appearance options for visualisation
    /// </summary>
    [System.Serializable, SerializeField]
    public struct PlanetAppearanceOptions
    {
        /// <summary>
        /// The in-game scale of the planets
        /// </summary>
        public float diameterScale;

        /// <summary>
        /// Apply min max scale?
        /// </summary>
        public bool applyMinMaxScale;

        /// <summary>
        /// Min scale
        /// </summary>
        public float minScale;

        /// <summary>
        /// Max scale
        /// </summary>
        public float maxScale;
    }

    /// <summary>
    /// Appearance options instance
    /// </summary>
    public PlanetAppearanceOptions options;

    /// <summary>
    /// Has the CSV been read yet?
    /// </summary>
    private bool appearancesRead = false;

    /// <summary>
    /// Called when the appearances csv has been read.
    /// </summary>
    public void OnAppearancesRead()
    {
        //Set appearances to be read.
        appearancesRead = true;
    }

    /// <summary>
    /// Called when the planet objects have been created.
    /// </summary>
    public void OnPlanetsCreated()
    {
        //Appearances not read? ok die
        if(!appearancesRead)
            throw new UnityException("I haven't read the planet appearances csv yet!");

        //Loop through each
        foreach (Transform planetObj in planetParentObj.transform)
        {
            foreach (PlanetAppearance appearance in reader.planetAppearances)
            {
                if (planetObj.name.Equals(appearance.name))
                {
                    this.ApplyPlanetAppearance(planetObj.gameObject, appearance);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Applies a planet appearance onto a given object.
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="appearance"></param>
    protected void ApplyPlanetAppearance(GameObject gameObject, PlanetAppearance appearance)
    {
        //We need to set the diameter first. Find the max diameter for all the planets.
        //NOTE: this is bad practice because we're computing the max for each planet. it only 
        //      ideally needs to be done once.
        float maxDiameter = reader.planetAppearances.Max(x => x.diameter);

        //Normalise this diameter
        float normalisedDiameter = appearance.diameter / maxDiameter;

        //Should we apply min or max scale?
        if(options.applyMinMaxScale)
            normalisedDiameter = Mathf.Clamp(normalisedDiameter, options.minScale, options.maxScale);

        //Set the diameter of this object.
        gameObject.transform.localScale = Vector3.one * (normalisedDiameter * options.diameterScale);
    }
}