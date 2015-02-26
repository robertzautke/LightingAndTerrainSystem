// This class has a utility for smoothing the map. 
// Potentially could add different types of smoothing.

using UnityEngine;
using System.Collections;

public class Smoother {
    public static void Smoothen(GameObject inTerrain, int iterations)
    {
        Terrain ter = inTerrain.GetComponent<Terrain>();
        TerrainData terrainData = ter.terrainData;
        int w = terrainData.heightmapWidth;
        int h = terrainData.heightmapWidth;
        float[,] heights = terrainData.GetHeights(0, 0, w, h);

        for (int count = 0; count < iterations; count++)
        {
            for (int i = 1; i < w - 1; ++i)
            {
                for (int j = 1; j < h - 1; ++j)
                {
                    float total = 0.0f;
                    for (int u = -1; u <= 1; u++)
                    {
                        for (int v = -1; v <= 1; v++)
                        {
                            total += heights[i + u, j + v];
                        }
                    }

                    heights[i, j] = total / 9.0f;
                }
            }
        }

        terrainData.SetHeights(0, 0, heights);
    }
}
