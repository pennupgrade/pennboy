using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoissonDiskDraw : MonoBehaviour
{

    public float radius = 1;
    public Vector2 regionSize = Vector2.one;
    public int rejectionSamples = 30;
    public float displayRadius = 1;

    List<PDPoints> points = new List<PDPoints>();

    void OnValidate()
    {
        points.Clear();

        Dictionary<Vector2, PDPoints> emptyEdgeChunks = new Dictionary<Vector2, PDPoints>();
        Dictionary<Vector2, PDPoints> edgeChunks = new Dictionary<Vector2, PDPoints>();
        PDPoints pdPoints;

        pdPoints = PoissonDisk.GeneratePoints(radius, regionSize, emptyEdgeChunks, new Vector2(0,0), rejectionSamples);

        points.Add(pdPoints);


        edgeChunks[new Vector2(1, 0)] = pdPoints;

        pdPoints = PoissonDisk.GeneratePoints(radius, regionSize, edgeChunks, new Vector2(1, 0), rejectionSamples);

        points.Add(pdPoints);
    }

    void OnDrawGizmos()
    {
        foreach (PDPoints pdPoints in points)
        {
            List<Vector2> points = pdPoints.points;
            Vector2 position = pdPoints.position;
            Gizmos.DrawWireCube(regionSize / 2 + position, regionSize);
            if (points != null)
            {
                foreach (Vector2 point in points)
                {
                    Gizmos.DrawSphere(point + position, displayRadius);
                }
            }
        }
    }
}

public class PDPoints
{
    public int[,] grid;
    public List<Vector2> points;
    public Vector2 position;

    public PDPoints(int[,] grid, List<Vector2> points, Vector2 position)
    {
        this.grid = grid;
        this.points = points;
        this.position = position;
    }
}