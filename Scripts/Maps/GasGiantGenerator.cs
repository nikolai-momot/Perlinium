using UnityEngine;

public static class GasGiantGenerator
{
    /// <summary>
    /// Creates averages of each rows height to simulate Gas Giant Rings
    /// </summary>
    /// <param name="noiseMap"></param>
    public static void ApplyGasGiantMap(float[,] noiseMap)
    {
        int size = noiseMap.GetLength(0);

        float[] averages = GetRowAverages(noiseMap);

        for (int i = 0; i < size; i++)
        {
            float offset = (i == 0) ? 0f : averages[i - 1];

            for (int j = 0; j < size; j++)
            {
                noiseMap[j, i] = Mathf.Clamp01(averages[i] + offset);
            }
        }
    }

    /// <summary>
    /// Gets a modified average for each row in the nosie map
    /// </summary>
    /// <param name="noiseMap"></param>
    /// <returns></returns>
    static float[] GetRowAverages(float[,] noiseMap) {
        float[] averages = new float[noiseMap.GetLength(0)];

        for (int i = 0; i < noiseMap.GetLength(0); i++)
            averages[i] = ModifiedRowAverage(noiseMap, i);

        return averages;
    }

    /// <summary>
    /// Multiplies the row average by a random float value to create varience in rings
    /// </summary>
    /// <param name="noiseMap"></param>
    /// <param name="rowIndex"></param>
    /// <returns></returns>
    static float ModifiedRowAverage(float[,] noiseMap, int rowIndex) {
        return Mathf.Clamp01( RowAverage(noiseMap, rowIndex) * RandomNoiseCoefficient() );
    }

    /// <summary>
    /// Gets the sum of the row's values and devides it by the row length
    /// </summary>
    /// <param name="noiseMap"></param>
    /// <param name="rowIndex"></param>
    /// <returns></returns>
    static float RowAverage(float[,] noiseMap, int rowIndex)
    {
        return Sum(noiseMap, rowIndex) / noiseMap.GetLength(1);
    }

    /// <summary>
    /// Gets the sum of the row's noise map values
    /// </summary>
    /// <param name="noiseMap"></param>
    /// <param name="rowIndex"></param>
    /// <returns></returns>
    static float Sum(float[,] noiseMap, int rowIndex)
    {
        float result = 0;

        for (int columnIndex = 0; columnIndex < noiseMap.GetLength(1); columnIndex++)
            result += noiseMap[rowIndex, columnIndex];

        return result;
    }

    /// <summary>
    /// Uses a random float value to offset coefficient the average
    /// </summary>
    /// <returns></returns>
    static float RandomNoiseCoefficient() {
        return Random.Range(0f, 1f);
    }
}
