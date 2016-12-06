using UnityEngine;

public static class FalloffGenerator {
	
    /// <summary>
    /// Calculates and subtracts a falloff value for each point in a noise map
    /// </summary>
    /// <param name="originalMap"></param>
    /// <param name="falloffCurve"></param>
	public static void ApplyFalloffMap(float[,] originalMap, AnimationCurve falloffCurve) {
        int size = originalMap.GetLength(0);
        
        for (int i = 0; i < size; i++) {
			for (int j = 0; j < size; j++)
            {
                float falloffValue = CalculateFalloffValue(originalMap, falloffCurve, size, i, j);

                originalMap[i, j] = ApplyFalloffValue(originalMap[i, j], falloffValue);
            }
        }
	}

    /// <summary>
    /// Gets 2 floats on [-1,1], chooses the max, and evaluates with the falloff curve
    /// </summary>
    /// <param name="originalMap"></param>
    /// <param name="falloffCurve"></param>
    /// <param name="size"></param>
    /// <param name="i"></param>
    /// <param name="j"></param>
    static float CalculateFalloffValue(float[,] originalMap, AnimationCurve falloffCurve, int size, int i, int j)
    {
        float x = Normalize(i, size);
        float y = Normalize(j, size);

        float value = GetMax(x, y);

        return falloffCurve.Evaluate(value);
    }

    /// <summary>
    /// Subtracts the falloff from the original map value and clamps it between [0,1]
    /// </summary>
    /// <param name="originalValue"></param>
    /// <param name="falloffValue"></param>
    /// <returns></returns>
    static float ApplyFalloffValue(float originalValue, float falloffValue)
    {
        return Mathf.Clamp01(originalValue - falloffValue);
    }

    /// <summary>
    /// Returns the max of two floats between [0,1]
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    static float GetMax(float x, float y)
    {
        return Mathf.Max( Mathf.Abs(x), Mathf.Abs(y) );
    }

    /// <summary>
    /// Normalizes the value between [-1,1]
    /// </summary>
    /// <param name="point"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    static float Normalize(int point, int size) {
        return point / (float) size * 2 - 1;
    }
}
