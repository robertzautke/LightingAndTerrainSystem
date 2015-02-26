/*
 * This class implements random noise,
 * I used this class as a starting place for my generation
 */

using UnityEngine;
using System.Collections;

public class RandomGenerator : Generator
{
    public float minY = 0;
    public float maxY = 1;

    override public void Generate(GameObject inTerrain)
    {
        Terrain ter = inTerrain.GetComponent<Terrain>();
        TerrainData terrainData = ter.terrainData;

        w = terrainData.heightmapWidth;
        h = terrainData.heightmapWidth;
        heights = terrainData.GetHeights(0, 0, w, h);

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                heights[x, y] = UnityEngine.Random.Range(minY, maxY);
            }
        }

        terrainData.SetHeights(0, 0, heights);
    }
}
