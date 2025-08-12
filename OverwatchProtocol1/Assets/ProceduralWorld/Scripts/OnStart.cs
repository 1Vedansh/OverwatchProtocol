using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;

public class OnStart : MonoBehaviour
{
    private int mapWidth;
    private int mapHeight;
    private float meshScale;
    private int mapSize;
    public GameObject mapBlueprint;
    private Parameters param;

    void Start()
    {
        param = transform.parent.GetComponent<Parameters>();
        mapWidth = param.mapWidth;
        mapHeight = param.mapHeight;
        meshScale = param.meshScale;
        mapSize = param.mapSize;
        setup();
    }

    public void setup()
    {
        Vector2[] plots = new Vector2[(2 * mapSize - 1) * (2 * mapSize - 1) - 1];
        for (int c = 0, i = -(mapSize - 1); i <= mapSize - 1; i++)
        {
            for (int j = -(mapSize - 1); j <= mapSize - 1; j++)
            {
                if (i != 0 || j != 0)
                {
                    plots[c++] = new Vector2(i, j);
                }
            }
        }

        foreach (Vector2 dir in plots)
        {
            GameObject GO = Instantiate(mapBlueprint, transform.parent);
            GO.transform.localPosition = new Vector3((mapWidth - 1) * meshScale * dir.x, 0, (mapHeight - 1) * meshScale * dir.y);
            CreateMapGen genMap = GO.transform.GetComponent<CreateMapGen>();
            genMap.offset = new Vector2((mapWidth - 1) * dir.x, (mapHeight - 1) * dir.y);
        }
    }
}
