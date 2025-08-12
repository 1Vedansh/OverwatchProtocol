using Unity.VisualScripting;
using UnityEngine;

public class MakeBoundary : MonoBehaviour
{
    Parameters param;
    int mapSize;
    float meshScale;
    float width;
    float height;
    float wallHeight;
    float wallThickness;
    Transform[] walls;
    void Start()
    {
        param = transform.parent.GetComponent<Parameters>();
        mapSize = param.mapSize;
        meshScale = param.meshScale;
        width = param.mapWidth;
        height = param.mapHeight;
        wallHeight = param.wallHeight;
        wallThickness = param.wallThickness;
        walls = GetComponentsInChildren<Transform>();

        foreach (var wall in walls)
        {
            if (wall == transform) continue;
            setupWall(wall);
        }
    }

    void setupWall(Transform wall)
    {
        float rotation = wall.localRotation.eulerAngles.y;
        BoxCollider box = wall.transform.AddComponent<BoxCollider>();
        Vector3 size = box.size;
        if (rotation == 90 || rotation == 270)
        {
            wall.localPosition += wall.forward * (((2 * mapSize - 1) * (meshScale) * (width - 1)) / 2);
            size.x = (height - 1) * meshScale * (2*mapSize-1);
        }
        else
        {
            wall.localPosition += wall.forward * (((2 * mapSize - 1) * (meshScale) * (height - 1)) / 2);
            size.x = (width - 1) * meshScale * (2*mapSize-1);
        }
        size.z = wallThickness;
        size.y = wallHeight;
        box.size = size;
    }
}
