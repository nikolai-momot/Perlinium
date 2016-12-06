using UnityEngine;

public static class NoiseGenerator {
	
	public static float[,] GenerateNoiseMap(float[,] noiseMap, int seed, float scale, int octaveCount, float persistance, float lacunarity, Vector2 offset)
    {
        float maxHeight = float.MinValue;
        float minHeight = float.MaxValue;

        Vector2[] octaves = SetOctaveOffsets(seed, octaveCount, offset);
        
        int size = noiseMap.GetLength(0);
        
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float noiseHeight = ProcessNoiseHeight(scale, persistance, lacunarity, octaves, i, j);

                UpdateMinMaxHeight(ref maxHeight, ref minHeight, noiseHeight);

                noiseMap[j, i] = noiseHeight;
            }
        }

        LerpMap(noiseMap, maxHeight, minHeight);

        return noiseMap;
    }
    
    static float ProcessNoiseHeight(float scale, float persistance, float lacunarity, Vector2[] octaves, int i, int j)
    {

        float amplitude = 1;
        float frequency = 1;    //The higher the frequency the further apart the sample point will be
        float noiseHeight = 0;

        foreach (Vector2 octave in octaves)
        {
            noiseHeight = CalculateHeight(i, j, scale, amplitude, frequency, noiseHeight, octave);

            amplitude *= persistance;
            frequency *= lacunarity;
        }

        return noiseHeight;
    }

    static void UpdateMinMaxHeight(ref float maxHeight, ref float minHeight, float noiseHeight)
    {
        if (noiseHeight > maxHeight)
        {
            maxHeight = noiseHeight;
        }
        else if (noiseHeight < minHeight)
        {
            minHeight = noiseHeight;
        }
    }

    /// <summary>
    /// The unity perlin noise method return the same value at 0 and 1
    /// To get a gradient value we have to create two fractions
    /// By 
    /// </summary>
    /// <param name="scale"></param>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <param name="amplitude"></param>
    /// <param name="frequency"></param>
    /// <param name="noiseHeight"></param>
    /// <param name="octave"></param>
    /// <returns></returns>
    static float CalculateHeight(int i, int j, float scale, float amplitude, float frequency, float noiseHeight, Vector2 octave)
    {
        float x = j / scale * frequency + octave.x;
        float y = i / scale * frequency + octave.y;

        float perlinValue = NormalizePerlinValue(x, y);

        noiseHeight += perlinValue * amplitude;

        return noiseHeight;
    }

    /// <summary>
    /// Normalizes the value between [-1,1]
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    static float NormalizePerlinValue(float x, float y)
    {
        return Mathf.PerlinNoise(x, y) * 2 - 1;
    }


    /// <summary>
    /// Uses a random number generator to create a random gradient vector
    /// Offsets the vector with the supplied offset vector
    /// And returns an the arrays of octaveVectors
    /// </summary>
    /// <param name="seed"></param>
    /// <param name="octaves"></param>
    /// <param name="offset"></param>
    /// <param name="octaveOffsets"></param>
    static Vector2[] SetOctaveOffsets(int seed, int octaveCount, Vector2 offset)
    {
        System.Random random = new System.Random(seed);

        Vector2[] octaves = new Vector2[octaveCount];

        for (int i = 0; i < octaves.GetLength(0); i++)
        {
            float x = random.Next(-100000, 100000) + offset.x;
            float y = random.Next(-100000, 100000) + offset.y;
            octaves[i] = new Vector2(x, y);
            Debug.Log("(x,y): "+ octaves[i].ToString());
        }

        return octaves;
    }

    /// <summary>
    /// Using inverse Lerp to find a value between [min,max]
    /// </summary>
    /// <param name="noiseMap"></param>
    /// <param name="maxHeight"></param>
    /// <param name="minHeight"></param>
    static void LerpMap(float[,] noiseMap, float maxHeight, float minHeight)
    {
        int mapSize = noiseMap.GetLength(0);

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minHeight, maxHeight, noiseMap[x, y]);
            }
        }
    }
}
