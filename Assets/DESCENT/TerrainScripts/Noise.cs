using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class Noise
{

    public enum NormalizeMode { Local, Global };

    public enum BiomeMode { Sand, Snow };

    public static (float[,], float[,]) GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, float slope, Vector2 offset, NormalizeMode normalizeMode, BiomeMode biomeMode, AnimationCurve probCurve)
    {
        float halfWidth = mapWidth / 2f;
        Func<int, int, float, float> slopeFunc = (x, y, height) => (y - halfWidth - offset.y) / mapHeight * slope + height;
        static double LogisticFunc(double x, double k, double x0) => 1 / (1 + Math.Exp(-k * (x - x0)));


        float[,] heightMap = NoiseMap(mapWidth, mapHeight, seed, scale, octaves, persistance, lacunarity, offset, normalizeMode, biomeMode, slopeFunc);
        float[,] probMap = NoiseMap(mapWidth, mapHeight, seed, scale, octaves, persistance, lacunarity, offset, NormalizeMode.Local, biomeMode);

        float[,] treeProbMap = NoiseMap(mapWidth, mapHeight, seed+1, scale/1.5f, 1, persistance, lacunarity, offset, NormalizeMode.Local, biomeMode, (x, y, h) => h);

        for (int i = 0; i < probMap.GetLength(0); i++)
        {
            for (int j = 0; j < probMap.GetLength(1); j++)
            {
                probMap[i, j] = probCurve.Evaluate(probMap[i, j]) * (float)LogisticFunc(treeProbMap[i, j], 20.0, 0.5);
            }
        }

        return (heightMap, probMap);    
    }

    static float[,] NoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset, NormalizeMode normalizeMode, BiomeMode biomeMode, Func<int, int, float, float> func = null)
    {
        if (func == null)
        {
            func = (_, _, h) => h;
        }

        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];

        float maxPossibleHeight = 0;
        float amplitude = 1;
        float frequency = 1;

        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) - offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);

            maxPossibleHeight += amplitude;
            amplitude *= persistance;
        }

        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        float maxLocalNoiseHeight = float.MinValue;
        float minLocalNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {

                amplitude = 1;
                frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfWidth + octaveOffsets[i].x) / scale * frequency;
                    float sampleY = (y - halfHeight + octaveOffsets[i].y) / scale * frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;

                    // Sand Stuff
                    if (biomeMode == BiomeMode.Sand)
                    {
                        float sampleSandX = (x - halfWidth + octaveOffsets[i].x + seed) / scale * frequency / 2;
                        float sampleSandY = (y - halfWidth + octaveOffsets[i].y + seed) / scale * frequency / 2;
                        float perlinSandValue = Mathf.PerlinNoise(sampleSandX, sampleSandY) * 2 - 1;
                        perlinValue = Mathf.Abs(perlinValue);
                        perlinValue = Mathf.Lerp(1, 0, perlinValue);
                        noiseHeight += perlinSandValue;
                    }

                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                noiseHeight = func(x, y, noiseHeight);

                if (noiseHeight > maxLocalNoiseHeight)
                {
                    maxLocalNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minLocalNoiseHeight)
                {
                    minLocalNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                if (normalizeMode == NormalizeMode.Local)
                {
                    noiseMap[x, y] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap[x, y]);
                }
                else
                {
                    float normalizedHeight = (noiseMap[x, y] + 1) / (2f * maxPossibleHeight);
                    noiseMap[x, y] = normalizedHeight;
                }
            }
        }

        return noiseMap;
    }
}
