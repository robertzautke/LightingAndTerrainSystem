/*
 * This class implements the Perlin Noise Algorithm,
 * http://en.wikipedia.org/wiki/Perlin_noise
 * It has three parameters, ocataveCount, frequency, and persistence.
 * ocataveCount changes number of octaves to add to the noise
 * frequency changes the starting frequency that it makes the noise at
 * persistence changes how quickly amplitude changes.
 */
using UnityEngine;
using System.Collections;

public class PerlinNoiseGenerator : Generator
{
    public int ocataveCount = 8; // 1-16
    public float freq = 1.0f; //1-16
    public float persistence = 0.5f; // 0-1


    override public void Generate(GameObject inTerrain)
    {
        // Grab map data
        Terrain ter = inTerrain.GetComponent<Terrain>();
        TerrainData terrainData = ter.terrainData;

        w = terrainData.heightmapWidth;
        h = terrainData.heightmapWidth;
        heights = new float[w, h];

        PerlinNoise();

        terrainData.SetHeights(0, 0, heights);
    }

    private void PerlinNoise(){
	    int octaves = ocataveCount;

        float gain = persistence;
        float lacunarity = 2.0f;

        float min = float.MaxValue;
        float max = float.MinValue;

        // Set up gradients
	    float[,] gradients = new float[8,2];
        for (int i = 0; i < 8; i++)
	    {
		    gradients[i, 0] = Mathf.Cos(Mathf.PI / 4f * (float)i);
            gradients[i, 1] = Mathf.Sin(Mathf.PI / 4f * (float)i);
	    }

        // Set up random numbers
	    int[] permutations = new int[256];
        for (int i = 0; i < 256; i++)
		    permutations[i] = i;

        for (int i = 0; i < 256; i++)
	    {
		    int j = Random.Range(0,256);
		    int k = permutations[i];
		    permutations[i] = permutations[j];
		    permutations[j] = k;
	    }
	    
        // For each point:
	    for (int i = 0; i < h; i++)
	    {
            for (int j = 0; j < w; j++)
		    {
			    float amplitude = 1.0f;
                float frequency = freq / (float)w;
			    float heightValue = 0.0f;

                // Add each octave of noise
                for (int k = 0; k < octaves; k++)
			    {
                    int x = floor((float)j * frequency);
                    int y = floor((float)i * frequency);

                    float fracX = (float)j * frequency - (float)x;
                    float fracY = (float)i * frequency - (float)y;

                    int grad11 = permutations[(x + permutations[y & 255]) & 255] & 7;
                    int grad12 = permutations[(x + 1 + permutations[y & 255]) & 255] & 7;
                    int grad21 = permutations[(x + permutations[(y + 1) & 255]) & 255] & 7;
                    int grad22 = permutations[(x + 1 + permutations[(y + 1) & 255]) & 255] & 7;

                    float noise11 = dotproduct(gradients[grad11, 0], gradients[grad11, 1], fracX, fracY);
                    float noise12 = dotproduct(gradients[grad12, 0], gradients[grad12, 1], fracX - 1.0f, fracY);
                    float noise21 = dotproduct(gradients[grad21, 0], gradients[grad21, 1], fracX, fracY - 1.0f);
                    float noise22 = dotproduct(gradients[grad22, 0], gradients[grad22, 1], fracX - 1.0f, fracY - 1.0f);

				    fracX = fade(fracX);
				    fracY = fade(fracY);

                    float interpolatedx1 = lerp(noise11, noise12, fracX);
                    float interpolatedx2 = lerp(noise21, noise22, fracX);

                    float interpolatedxy = lerp(interpolatedx1, interpolatedx2, fracY);

				    heightValue += interpolatedxy * amplitude;
				    amplitude *= gain;
				    frequency *= lacunarity;
			    }

                heights[j, i] = heightValue;

			    if (heightValue < min)
				    min = heightValue;
			    else if (heightValue > max)
				    max = heightValue;
		    }
	    }

        // Normalize the points
        for (int x2 = 0; x2 < w; x2++)
        {
            for (int y2 = 0; y2 < h; y2++)
            {
                heights[x2, y2] = (heights[x2, y2] + 1f) / 2f;

            }
        }
    }

    // Helper points
    int floor(float value)
    {
	    return (value >= 0 ? (int)value : (int)value-1);
    }

    float dotproduct(float a, float b, float x, float y)
    {
	    return (a * x + b * y);
    }

    float lerp(float left, float right, float amount)
    {
	    return ( (1 - amount) * left + amount * right);
    }

    float fade(float x)
    {
	    return (x*x*x*(x*(6*x -15) + 10));
    }

}
