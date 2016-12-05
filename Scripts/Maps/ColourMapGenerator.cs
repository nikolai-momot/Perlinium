using System.Collections.Generic;
using UnityEngine;

public static class ColourMapGenerator
{
    /// <summary>
    /// Generates a colour map for the provided solarbody
    /// </summary>
    /// <param name="noiseMap"></param>
    /// <param name="solarBody"></param>
    /// <param name="falloffCurve"></param>
    /// <returns></returns>
    public static Color[] GetColourMap(float[,] noiseMap, SolarBody solarBody, AnimationCurve falloffCurve)
    {
        List<PaletteColour> colours = GetPalette(solarBody);

        ModifyMap(noiseMap, solarBody, falloffCurve);
        
        return FillColourMap(noiseMap, colours);
    }

    /// <summary>
    /// Set the colour values for every point in the noise map
    /// </summary>
    /// <param name="noiseMap"></param>
    /// <param name="colours"></param>
    /// <returns></returns>
    static Color[] FillColourMap(float[,] noiseMap, List<PaletteColour> colours)
    {
        int mapSize = noiseMap.GetLength(0);

        Color[] colourMap = new Color[mapSize * mapSize];

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                SetColour(noiseMap, colours, colourMap, y, x);
            }
        }

        return colourMap;
    }

    /// <summary>
    /// Modifies the noise map values according to the solarbody type
    /// </summary>
    /// <param name="noiseMap"></param>
    /// <param name="solarBody"></param>
    /// <param name="falloffCurve"></param>
    static void ModifyMap(float[,] noiseMap, SolarBody solarBody, AnimationCurve falloffCurve)
    {
        bool useFalloff = true;
    
        if (solarBody.bodyType == BodyType.GasGiant)
            GasGiantGenerator.GenerateGasGiant(noiseMap);
        else if(useFalloff)
            FalloffGenerator.GenerateFalloffMap(noiseMap, falloffCurve);
    }

    /// <summary>
    /// Gets a  colour palette according to the solarBody's body type
    /// </summary>
    /// <param name="solarBody"></param>
    /// <returns></returns>
    static List<PaletteColour> GetPalette(SolarBody solarBody)
    {
        Palette palette = GameObject.FindObjectOfType<PaletteManager>().getPalette(solarBody.bodyType);

        return palette.colours;
    }

    /// <summary>
    /// Sets the Colour value colour map based of the height
    /// </summary>
    /// <param name="noiseMap"></param>
    /// <param name="colours"></param>
    /// <param name="colourMap"></param>
    /// <param name="y"></param>
    /// <param name="x"></param>
    static void SetColour(float[,] noiseMap, List<PaletteColour> colours, Color[] colourMap, int y, int x)
    {
        int size = noiseMap.GetLength(0);

        foreach (PaletteColour colour in colours)
        {
            if (noiseMap[x, y] <= colour.height)
            {
                colourMap[y * size + x] = colour.colour;
                break;
            }
        }
    }
}
