using UnityEngine;

public static class GasGiantGenerator
{

    public static void GenerateGasGiant(float[,] noiseMap)
    {
        int size = noiseMap.GetLength(0);

        float[] averages = GetRowAverages(noiseMap);

        for (int i = 0; i < size; i++)
        {
            float offset = (i == 0) ? 0f : averages[i - 1];

            for (int j = 0; j < size; j++)
            {
                noiseMap[j, i] = Mathf.Clamp01(noiseMap[j, i] - averages[i] + offset);
            }
        }
    }

    static float[] GetRowAverages(float[,] noiseMap) {
        float[] averages = new float[noiseMap.GetLength(0)];

        for (int i = 0; i < noiseMap.GetLength(0); i++)
            averages[i] = RowAverage(noiseMap, i);

        return averages;
    }

    static float RowAverage(float[,] noiseMap, int rowIndex) {
        return Mathf.Clamp01( Average(noiseMap, rowIndex) * RandomNoiseCoefficient() );
    }

    static float Average(float[,] noiseMap, int rowIndex)
    {
        return Sum(noiseMap, rowIndex) / noiseMap.GetLength(1);
    }

    static float Sum(float[,] noiseMap, int rowIndex)
    {
        float result = 0;

        for (int columnIndex = 0; columnIndex < noiseMap.GetLength(1); columnIndex++)
            result += noiseMap[rowIndex, columnIndex];

        return result;
    }

    static float RandomNoiseCoefficient() {
        return Random.Range(0f, 1f);
    }
}
