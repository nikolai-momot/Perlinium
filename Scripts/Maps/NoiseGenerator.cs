using UnityEngine;

public static class NoiseGenerator {
	
	public static float[,] GenerateNoiseMap(float[,] noiseMap, int seed, float scale, int octaveCount, float persistance, float lacunarity, Vector2 offset)
    {
        float maxHeight = float.MinValue;
        float minHeight = float.MaxValue;

        Vector2[] octaves = new Vector2[octaveCount];

        SetOctaveOffsets(seed, offset, octaves);
        
        int size = noiseMap.GetLength(0);
        
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                foreach(Vector2 octave in octaves)
                {
                    noiseHeight = CalculateHeight(scale, i, j, amplitude, frequency, noiseHeight, octave);

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxHeight)
                {
                    maxHeight = noiseHeight;
                }
                else if (noiseHeight < minHeight)
                {
                    minHeight = noiseHeight;
                }

                noiseMap[j, i] = noiseHeight;
            }
        }

        LerpMap(noiseMap, maxHeight, minHeight);

        return noiseMap;
    }

    private static float CalculateHeight(float scale, int i, int j, float amplitude, float frequency, float noiseHeight, Vector2 octave)
    {
        float x = j / scale * frequency + octave.x;
        float y = i / scale * frequency + octave.y;

        float perlinValue = Mathf.PerlinNoise(x, y) * 2 - 1;

        noiseHeight += perlinValue * amplitude;

        return noiseHeight;
    }

    /// <summary>
    /// Normailizing noise map with the newly derived max and minimum values
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="seed"></param>
    /// <param name="octaves"></param>
    /// <param name="offset"></param>
    /// <param name="octaveOffsets"></param>
    static void SetOctaveOffsets(int seed, Vector2 offset, Vector2[] octaves)
    {
        System.Random random = new System.Random(seed);
        
        for (int i = 0; i < octaves.GetLength(0); i++)
        {
            float x = random.Next(-100000, 100000) + offset.x;
            float y = random.Next(-100000, 100000) - offset.y;
            octaves[i] = new Vector2(x, y);
        }
    }
}
