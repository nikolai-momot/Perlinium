using UnityEngine;
using System;

public static class FalloffGenerator {
	
	public static void GenerateFalloffMap(int size, float[,] originalMap, AnimationCurve falloffCurve) {
        float[,] falloffMap = new float[size, size];

        Array.Copy(originalMap, falloffMap, size);

		for (int i = 0; i < size; i++) {
			for (int j = 0; j < size; j++) {
				float x = i / (float)(size) * 2 - 1;
				float y = j / (float)(size) * 2 - 1;

				float value = Mathf.Max (Mathf.Abs (x), Mathf.Abs (y));

                falloffMap[i, j] = falloffCurve.Evaluate(value);

                originalMap[i, j] = Mathf.Clamp01(originalMap[i, j] - falloffMap[i, j]);
            }
        }
	}
}
