using System;
using System.Collections.Generic;
using UnityEngine;

public static class MapGenerator
{
    public static Color[] GenerateColourMap(float[,] noiseMap, SolarBody solarBody, List<PaletteColour> regionsInUse, AnimationCurve falloffCurve) {
        int mapSize = noiseMap.GetLength(0);

        if(solarBody.bodyType == BodyType.GasGiant)
            GasGiantGenerator.GenerateGasGiant(noiseMap);

        FalloffGenerator.GenerateFalloffMap(mapSize, noiseMap, falloffCurve);
                
        
        Color[] colourMap = new Color[mapSize * mapSize];

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                float currentHeight = noiseMap[x, y];

                for (int z = 0; z < regionsInUse.Count; z++)
                {
                    if (currentHeight <= regionsInUse[z].height)
                    {
                        colourMap[y * mapSize + x] = regionsInUse[z].colour;
                        break;
                    }
                }
            }
        }

        return colourMap;
    }
}
