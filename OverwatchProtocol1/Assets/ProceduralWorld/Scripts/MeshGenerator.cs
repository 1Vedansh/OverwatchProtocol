using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class MeshGenerator : MonoBehaviour
{
    public Gradient gradient;
    public bool wantSpawn;
    public MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve, float uvtilesize, Spawnable[] spawnableModels, float meshScale)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        MeshData meshData = new MeshData(width, height);
        int vertexIndex = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier, topLeftZ - y);
                meshData.uvs[vertexIndex] = new Vector2((x / (float)width) * uvtilesize, (y / (float)height) * uvtilesize);
                if (x < width - 1 && y < height - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    meshData.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }

                if (wantSpawn)
                {
                    List<Spawnable> tempList = new List<Spawnable>();
                    foreach (Spawnable spawnable in spawnableModels)
                    {
                        if (spawnable.minSpawnHeight <= heightMap[x, y] && heightMap[x, y] <= spawnable.maxSpawnHeight)
                        {
                            tempList.Add(spawnable);
                        }
                    }
                    float tempRandom = Random.Range(0f, 100f);

                    for (int i = tempList.Count - 1; i >= 0; i--)
                    {
                        if (tempRandom > tempList[i].spawnProbability)
                        {
                            tempList.RemoveAt(i);
                        }
                    }
                    if (tempList.Count != 0)
                    {
                        float[] weightArray = new float[tempList.Count];
                        Vector3 pos = meshScale * (new Vector3((topLeftX + x), heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier, topLeftZ - y)) + transform.position;

                        for (int i = 0; i < weightArray.Length; i++)
                        {
                            weightArray[i] = tempList[i].spawnProbability;
                        }

                        int chosenIndex = weightedProbabilityCalculator(weightArray);

                        Spawnable chosenSpawnable = tempList[chosenIndex];

                        weightArray = new float[chosenSpawnable.model.Length];
                        for (int i = 0; i < weightArray.Length; i++)
                        {
                            weightArray[i] = chosenSpawnable.model[i].weight;
                        }

                        chosenIndex = weightedProbabilityCalculator(weightArray);
                        StartCoroutine(LOL(chosenSpawnable.model[chosenIndex].model, pos, meshScale, chosenSpawnable.randomizedScaleFactor, chosenSpawnable.spawnOffset));
                    }
                }

                vertexIndex++;
            }
        }

        meshData.colours(heightMap, width, height, gradient);

        return meshData;
    }

    int weightedProbabilityCalculator(float[] weightArray)
    {
        float totalWeight = 0f;
        foreach (float probability in weightArray)
        {
            totalWeight += probability;
        }
        float randomValue = Random.Range(0f, totalWeight);
        float cumulative = 0f;
        for (int i = 0; i < weightArray.Length; i++)
        {
            cumulative += weightArray[i];
            if (randomValue <= cumulative)
            {
                return i;
            }
        }
        return weightArray.Length - 1;
    }

    IEnumerator LOL(GameObject prefab, Vector3 pos, float meshScale, Vector2 randomizedScaleFactor, Vector3 offset)
    {
        yield return new WaitForSeconds(2f);
        if (Physics.Raycast(pos + Vector3.up * 20f, Vector3.down, out RaycastHit hit, 1000f))
        {
            GameObject temp = Instantiate(prefab, transform);
            temp.transform.position = hit.point + offset;
            temp.transform.localRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            temp.transform.Rotate(temp.transform.up, Random.Range(0f, 360f));
            temp.transform.localScale = Vector3.one * meshScale * Random.Range(randomizedScaleFactor.x,randomizedScaleFactor.y);
        }
        else
        {
            Debug.Log("FAILED");
        }
    }
}

public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;
    public Color[] vertexColor;


    int triangleIndex;

    public MeshData(int meshWidth, int meshHeight)
    {
        vertices = new Vector3[meshWidth * meshHeight];
        uvs = new Vector2[meshWidth * meshHeight];
        vertexColor = new Color[meshHeight * meshWidth];
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }

    public void colours(float[,] heightMap, int width, int height, Gradient gradient)
    {
        float maxHeight = float.MinValue;
        float minHeight = float.MaxValue;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                maxHeight = Mathf.Max(maxHeight, heightMap[x, y]);
                minHeight = Mathf.Min(minHeight, heightMap[x, y]);
            }
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float heightt = Mathf.InverseLerp(minHeight, maxHeight, heightMap[x, y]);
                vertexColor[y * width + x] = gradient.Evaluate(heightt);
            }
        }

    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.colors = vertexColor;
        mesh.RecalculateNormals(0);
        return mesh;
    }
}