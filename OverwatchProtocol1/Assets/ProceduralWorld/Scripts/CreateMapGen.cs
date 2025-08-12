using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Profiling;

public class CreateMapGen : MonoBehaviour
{
    int mapWidth;
    int mapHeight;
    float meshScale;
    float noiseScale;
    int octaves;
    float persistance;
    float lacunarity;
    int seed;
    public Vector2 offset;
    float meshHeightMultiplier;
    AnimationCurve meshHeightCurve;
    MapDisplay display;
    Material myMat;
    Parameters param;
    Shader shader;
    float uvtilesize;
    Spawnable[] spawnableModels;

    public MeshGenerator meshGenerator;

    void Start()
    {
        param = transform.parent.transform.GetComponent<Parameters>();
        mapWidth = param.mapWidth;
        mapHeight = param.mapHeight;
        meshScale = param.meshScale;
        noiseScale = param.noiseScale;
        octaves = param.octaves;
        persistance = param.persistance;
        lacunarity = param.lacunarity;
        seed = param.seed;
        offset += param.offset;
        meshHeightMultiplier = param.meshHeightMultiplier;
        meshHeightCurve = param.meshHeightCurve;
        shader = param.shader;
        display = transform.GetComponent<MapDisplay>();
        myMat = new Material(shader);
        uvtilesize = param.uvtilesize;
        spawnableModels = param.spawnableModels;
        transform.GetChild(0).GetComponent<MeshRenderer>().material = myMat;
        transform.GetChild(0).transform.localScale = new Vector3(meshScale, meshScale, meshScale);
        meshGenerator.gradient = param.gradient;
        meshGenerator.wantSpawn = param.wantSpawn;
        if (param.spawnNavMesh)
        {
            StartCoroutine(buildNavMesh());
        }
        generateMap();
    }

    IEnumerator buildNavMesh()
    {
        yield return null;
        transform.GetChild(0).GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    public void generateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

        display.DrawMesh(meshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, uvtilesize, spawnableModels, meshScale));
    }
}