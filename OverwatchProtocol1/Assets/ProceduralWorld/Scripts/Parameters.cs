using System;
using UnityEngine;

public class Parameters : MonoBehaviour
{
    [Tooltip("Width of each chunk of the map(250)")]
    public int mapWidth;
    [Tooltip("Height of each chunk of the map(250)")]
    public int mapHeight;
    [Tooltip("Scale of the mesh to match the player(2)")]
    public float meshScale;
    [Tooltip("Variety of the perlin noise(45)")]
    public float noiseScale;
    [Tooltip("(5)")]
    public int octaves;

    [Range(0, 1)]
    [Tooltip("(0.5)")]
    public float persistance;
    [Tooltip("(2)")]
    public float lacunarity;
    [Tooltip("Randomized seed of the world")]
    public int seed;
    [Tooltip("Offset of the perlin map(0,0)")]
    public Vector2 offset;
    [Tooltip("How much to multiply the heights by(45)")]
    public float meshHeightMultiplier;
    [Tooltip("1 = 1 chunk, 2 = 9 chunks(3x3), 3 = 25 chunks(5x5)")]
    public int mapSize;
    [Tooltip("Border Height(100)")]
    public float wallHeight;
    [Tooltip("Border thickness(5)")]
    public float wallThickness;
    [Tooltip("Shader used to apply texture to terrain")]
    public Shader shader;
    [Tooltip("Uses curves to customize heights")]
    public AnimationCurve meshHeightCurve;
    [Tooltip("Gradient color which will be used to color the terrain")]
    public Gradient gradient;
    [Tooltip("uv tile size(40)")]
    public float uvtilesize;
    [Tooltip("To spawn the models or not?")]
    public bool wantSpawn;
    [Tooltip("To randomly spawn in trees/rocks etc")]
    public Spawnable[] spawnableModels;
    [Tooltip("To build the navmesh surface?")]
    public bool spawnNavMesh;

    void OnValidate()
    {
        if (mapWidth < 1)
        {
            mapWidth = 1;
        }
        if (mapHeight < 1)
        {
            mapHeight = 1;
        }
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }
        if (meshScale <= 0)
        {
            meshScale = 1;
        }
        if (mapSize < 1)
        {
            mapSize = 1;
        }
        if (wallHeight < 0)
        {
            wallHeight = 0;
        }
        if (wallThickness < 0)
        {
            wallThickness = 0;
        }
    }
}

[Serializable]
public class WeightedModel
{
    public GameObject model;
    public float weight;
}


[Serializable]
public class Spawnable
{
    public string modelName;
    public float minSpawnHeight;
    public float maxSpawnHeight;
    public float spawnProbability;
    public Vector2 randomizedScaleFactor;
    public Vector3 spawnOffset;
    public WeightedModel[] model;
};