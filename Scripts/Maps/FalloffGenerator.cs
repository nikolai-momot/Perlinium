using UnityEngine;

public static class FalloffGenerator {
	
	public static void GenerateFalloffMap(float[,] originalMap, AnimationCurve falloffCurve) {
        int size = originalMap.GetLength(0);
        
        for (int i = 0; i < size; i++) {
			for (int j = 0; j < size; j++)
            {
                ApplyFalloffToNoise(originalMap, falloffCurve, size, i, j);
            }
        }
	}

    /// <summary>
    /// Generates falloff value and subtracts it from the noise map value
    /// </summary>
    /// <param name="originalMap"></param>
    /// <param name="falloffCurve"></param>
    /// <param name="size"></param>
    /// <param name="i"></param>
    /// <param name="j"></param>
    static void ApplyFalloffToNoise(float[,] originalMap, AnimationCurve falloffCurve, int size, int i, int j)
    {
        float x = Normalize(i, size);
        float y = Normalize(j, size);

        float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));

        float falloffValue = falloffCurve.Evaluate(value);

        originalMap[i, j] = Mathf.Clamp01( originalMap[i,j] - falloffValue);
    }

    /// <summary>
    /// Normalizes the value between -1 and 1
    /// </summary>
    /// <param name="point"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    static float Normalize(int point, int size) {
        return point / (float)size * 2 - 1;
    }
}
