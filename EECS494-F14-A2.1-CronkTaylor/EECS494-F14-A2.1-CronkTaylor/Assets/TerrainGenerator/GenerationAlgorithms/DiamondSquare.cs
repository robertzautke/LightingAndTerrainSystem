/*
 * This class implements the Diamond Square Algorithm,
 * http://en.wikipedia.org/wiki/Diamond-square_algorithm
 * It has two parameters, featuresize and scaleMod.
 * The feature size changes the size of starting square
 * and the scale mod changes the rate at which the scale changes between steps
 */

using UnityEngine;
using System.Collections;

public class DiamondSquare : Generator
{
    public int featureSize = 512;
    public float scaleMod = 1;
    private float scaleSave;

    override public void Generate(GameObject inTerrain)
    {
        // Grab the terrain data
        Terrain ter = inTerrain.GetComponent<Terrain>();
        TerrainData terrainData = ter.terrainData;

        scaleSave = scaleMod;

        // Get info about map
        w = terrainData.heightmapWidth - 1;
        h = terrainData.heightmapWidth - 1;
        heights = new float[w + 1,h + 1];

        DiamondSquareGen();

        terrainData.SetHeights(0, 0, heights);
        scaleMod = scaleSave;
    }

    // Main algorithm
    private void DiamondSquareGen()
    {
        // Set the intial squares to numbers
        for (int y = 0; y < w; y += featureSize)
        {
            for (int x = 0; x < w; x += featureSize)
            {
                setSample(x, y, getNumber());
            }
        }

        int stepSize = featureSize;
        float scale = 1.0f / (float)w;
        do
        {
            // Diamond step
            int halfStep = stepSize / 2;
            for (int y = 0; y < w; y += stepSize)
            {
                for (int x = 0; x < w; x += stepSize)
                {
                    float a = sample(x, y);
                    float b = sample(x + stepSize, y);
                    float c = sample(x, y + stepSize);
                    float d = sample(x + stepSize, y + stepSize);

                    float e = (a + b + c + d) / 4.0f + getNumber() * stepSize * scale;
                    setSample(x + halfStep, y + halfStep, e);
                }
            }

            // Square Step
            for (int y = 0; y < w; y += stepSize)
            {
                for (int x = 0; x < w; x += stepSize)
                {
                    float a = sample(x, y);
                    float b = sample(x + stepSize, y);
                    float c = sample(x, y + stepSize);
                    float d = sample(x + halfStep, y + halfStep);
                    float e = sample(x + halfStep, y - halfStep);
                    float f = sample(x - halfStep, y + halfStep);

                    float H = (a + b + d + e) / 4.0f + getNumber() * stepSize * scale * 0.5f;
                    float g = (a + c + d + f) / 4.0f + getNumber() * stepSize * scale * 0.5f;
                    setSample(x + halfStep, y, H);
                    setSample(x, y + halfStep, g);
                }
            }
            stepSize /= 2;
            scale *= (scaleMod + 0.8f);
            scaleMod *= 0.3f;

        } while (stepSize > 1);

        // Normalize some of the points. Needs a re-write
        float max = 0;
        float min = heights[0, 0];
        for (int y = 0; y < h; y += 1)
        {
            for (int x = 0; x < w; x += 1)
            {
                if (heights[x, y] > max)
                {
                    max = heights[x, y];
                }
                if (heights[x, y] < min)
                    min = heights[x, y];
            }
        }
        if (min < 0)
        {
            min *= -1;
            max += min;
        }
        else
        {
            min = 0;
        }
        for (int y = 0; y < h; y += 1)
        {
            for (int x = 0; x < w; x += 1)
            {
                heights[x, y] += min;
                heights[x, y] /= max;

            }
        }

        // Correct some height points on the edges that don't correctly get set.
        for (int x = 0; x < w + 1; x++)
        {
            heights[x, h] = heights[x, h - 1];
        }
        for (int y = 0; y < h + 1; y++)
        {
            heights[w, y] = heights[w - 1, y];
        }
    }

    // Get a random number in the range I'm looking for.
    private float getNumber()
    {
        return UnityEngine.Random.Range(0.0f, 1.0f) * 2 - 1;
    }

    // Return the sample at this point. These are done for map wrapping.
    private float sample(int x, int y)
    {
        return heights[(x & (w - 1)), (y & (h - 1))];
    }

    // set the sample at this point.
    private void setSample(int x, int y, float value)
    {
        heights[(x & (w - 1)), (y & (h - 1))] = value;
    }
}
