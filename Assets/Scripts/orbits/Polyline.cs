using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polyline
{
    /// <summary>
    /// Line renderer data
    /// </summary>
    [System.Serializable, SerializeField]
    public struct LineRendererData
    {
        /// <summary>
        /// The start width of the line
        /// </summary>
        public float startWidth;

        /// <summary>
        /// The end width of the line
        /// </summary>
        public float endWidth;

        /// <summary>
        /// Should it loop?
        /// </summary>
        public bool loop;

        /// <summary>
        /// The material of the line
        /// </summary>
        public Material material;
    }

    /// <summary>
    /// The points of the polyline
    /// </summary>
    public List<Vector3> points;

    /// <summary>
    /// Creates a new polyline 
    /// </summary>
    /// <param name="points"></param>
    public Polyline(List<Vector3> points)
    {
        //Set points
        this.points = points;
    }

    /// <summary>
    /// Attaches + renders a polyline in the context of an object, using the LineRenderer component.
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="visOptions"></param>
    public void AttachToObject(GameObject gameObject, in Polyline.LineRendererData visOptions)
    {
        //First, attach the line renderer.
        var lineRenderer = gameObject.AddComponent<LineRenderer>();

        //Next, set the positions.
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());

        //Next, set options.
        lineRenderer.material = visOptions.material;
        lineRenderer.loop = visOptions.loop;
        lineRenderer.startWidth = visOptions.startWidth;
        lineRenderer.endWidth = visOptions.endWidth;

        //And I think thats it?
    }
}
