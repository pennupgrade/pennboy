using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PoissonDisk
{

    public static PDPoints GeneratePoints(float radius, Vector2 sampleRegionSize, Dictionary<Vector2, PDPoints> edgeChunks, Vector2 position, int samples = 30)
    {

        float cellSize = radius / Mathf.Sqrt(2);

        int[,] grid = new int[Mathf.CeilToInt(sampleRegionSize.x / cellSize), Mathf.CeilToInt(sampleRegionSize.y / cellSize)];
        List<Vector2> points = new List<Vector2>();
        List<Vector2> spawnPoints = new List<Vector2>();

        spawnPoints.Add(sampleRegionSize / 2);
        while (spawnPoints.Count > 0)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            Vector2 spawnCentre = spawnPoints[spawnIndex];
            bool candidateAccepted = false;

            for (int i = 0; i < samples; i++)
            {
                float angle = Random.value * Mathf.PI * 2;
                Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                Vector2 candidate = spawnCentre + dir * Random.Range(radius, 2 * radius);
                if (IsValid(candidate, sampleRegionSize, cellSize, radius, points, grid, edgeChunks))
                {
                    points.Add(candidate);
                    spawnPoints.Add(candidate);
                    grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize)] = points.Count;
                    candidateAccepted = true;
                    break;
                }
            }
            if (!candidateAccepted)
            {
                spawnPoints.RemoveAt(spawnIndex);
            }

        }

        return new PDPoints(grid, points, position);
    }

    static bool IsValid(Vector2 candidate, Vector2 sampleRegionSize, float cellSize, float radius, List<Vector2> points, int[,] grid, Dictionary<Vector2, PDPoints> edgeChunks)
    {
        if (candidate.x >= 0 && candidate.x < sampleRegionSize.x && candidate.y >= 0 && candidate.y < sampleRegionSize.y)
        {
            int cellX = (int)(candidate.x / cellSize);
            int cellY = (int)(candidate.y / cellSize);
            int searchStartX = Mathf.Max(0, cellX - 2);
            int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);
            int searchStartY = Mathf.Max(0, cellY - 2);
            int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);

            for (int x = searchStartX; x <= searchEndX; x++)
            {
                for (int y = searchStartY; y <= searchEndY; y++)
                {
                    int pointIndex = grid[x, y] - 1;
                    if (pointIndex != -1)
                    {
                        float sqrDst = (candidate - points[pointIndex]).sqrMagnitude;
                        if (sqrDst < radius * radius)
                        {
                            return false;
                        }
                    }
                }
            }

            Vector2[] directions = { 
                new Vector2(-1, 0),
                new Vector2(1, 0),
                new Vector2(0, -1),
                new Vector2(0, 1),
                new Vector2(-1,-1),
                new Vector2(-1,1),
                new Vector2(1,-1),
                new Vector2(1,1)
            };


            foreach (var dir in directions)
            {
                int newX = cellX + (int)dir.x * 2;
                int newY = cellY + (int)dir.y * 2;
                if (newX < 0 || newX >= grid.GetLength(0) || newY < 0 || newY >= grid.GetLength(1))
                {
                    if (edgeChunks.TryGetValue(dir, out var chunk))
                    {
                        int[,] chunkGrid = chunk.grid;
                        List<Vector2> chunkPts = chunk.points;

                        //int rcellX = (int)(candidateRelative.x / cellSize);
                        //int rcellY = (int)(candidateRelative.y / cellSize);

                        //Debug.Log((rcellX, rcellY));
                        ////Debug.Log(grid.GetLength(0));

                        //int chunkStartX = Mathf.Min(grid.GetLength(0) - 1, rcellX - 2);
                        //int chunkEndX = Mathf.Max(rcellX + 2, 0);
                        //int chunkStartY = Mathf.Min(grid.GetLength(0) - 1, rcellY - 2);
                        //int chunkEndY = Mathf.Max(rcellY + 2, 0);

                        //for (int x = chunkStartX; x <= chunkEndX; x++)
                        //{
                        //    for (int y = chunkStartY; y <= chunkEndY; y++)
                        //    {
                        //        int pointIndex = chunkGrid[x, y] - 1;
                        //        if (pointIndex != -1)
                        //        {
                        //            float sqrDst = (candidateRelative - chunkPts[pointIndex]).sqrMagnitude;
                        //            if (sqrDst < radius * radius)
                        //            {
                        //                Debug.Log("Pt removed");
                        //                return false;
                        //            }
                        //        }
                        //    }
                        //}
                    }

                }
            }

            return true;
        }
        return false;
    }
}