/*
 * This class implements a Thermal Erosion Algorithm as described in this paper:
 * http://micsymposium.org/mics_2011_proceedings/mics2011_submission_30.pdf
 * Has the smoothness paramater and the iterations paramater
 * Smoothness changes how easily it smooths the land
 * Iterations changes number of times the algorithm runs
 */

using UnityEngine;
using System.Collections;

public class ThermalErosion : Erosion{
	override public void Erode(GameObject inTerrain){
        Terrain ter = inTerrain.GetComponent<Terrain>();
        TerrainData terrainData = ter.terrainData;
        w = terrainData.heightmapWidth;
        h = terrainData.heightmapWidth;
        heights = terrainData.GetHeights(0, 0, w, h);

        float talus = smoothness / (float)w;

        for (int iterCount = 0; iterCount < iterations; iterCount++)
        {
            for (int x = 1; x < (w - 1); x++)
            {
                for (int y = 1; y < (h - 1); y++)
                {
                    int lowestX = 0;
                    int lowestY = 0;
                    float newHeight;
                    float currentHeight = heights[x, y];
                    float maxDifference = float.MinValue;

                    for (int i = -1; i <= 1; i += 1)
                    {
                        for (int j = -1; j <= 1; j += 1)
                        {
                            float currentDifference = currentHeight - heights[x + i, y + j];

                            if (currentDifference > maxDifference)
                            {
                                maxDifference = currentDifference;

                                lowestX = i;
                                lowestY = j;
                            }
                        }
                    }

                    if (maxDifference > talus)
                    {
                        newHeight = maxDifference / 2.0f;

                        heights[x, y] -= newHeight;
                        heights[x + lowestX, y + lowestY] += newHeight;
                    }
                }
            }
        }

        terrainData.SetHeights(0, 0, heights);

        if (doSmooth)
            Smoother.Smoothen(inTerrain, 1);  
    }
}
