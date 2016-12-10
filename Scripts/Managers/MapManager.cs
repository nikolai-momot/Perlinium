using UnityEngine;
using System.Collections.Generic;


[RequireComponent(typeof(PaletteManager))]
public class MapManager : MonoBehaviour {	
    public int mapSize = 241;

    [Range(0.001f, 500)]
    public float noiseScale;

	[Range(1,5)]
	public int octaves;

	[Range(0,1)]
	public float persistance;

    [Range(0,1)]
	public float lacunarity;
	
	public AnimationCurve falloffCurve;

	public int seed;
    
	Vector2 offset;

    SolarManager solarManager;

	void Awake() {
        solarManager = FindObjectOfType<SolarManager>();

        GenerateMaps();
	}
    
    void OnValidate()
    {
        GenerateMaps();
    }

    /// <summary>
    /// Offsets the map of the sun texture to simulate movement
    /// </summary>
    /// <param name="solarBody"></param>
    public void MoveSunMap(SolarBody solarBody)
    {
        offset.x += 0.1f;
        offset.y += 0.1f;

        GenerateMap(solarBody);
    }
    
    /// <summary>
    /// Generates a texture map for every solarbody in the system
    /// </summary>
    public void GenerateMaps()
    {
        if (solarManager == null)
            solarManager = FindObjectOfType<SolarManager>();

        SolarBody sun = GameObject.Find("Sun").GetComponent<SolarBody>(); 
        
        GenerateMap(sun);

        GenerateSatelliteMaps(solarManager.GetSolarBodies());
	}

    /// <summary>
    /// Generates a texture map for every sattelite of the given planet
    /// </summary>
    /// <param name="solarBodies"></param>
    void GenerateSatelliteMaps(List<SolarBody> solarBodies) {
        foreach (SolarBody solarBody in solarBodies) {
            
            if (solarBody == null) {
                solarManager.RemoveSolarBody(solarBody);
                continue;
            }

            GenerateMap(solarBody);

            GenerateSatelliteMaps(solarBody.satellites);
        }
    }

    /// <summary>
    /// Modifies the seedOffest of the given solarbody
    /// </summary>
    /// <param name="solarBody"></param>
    /// <returns></returns>
    public int GetSatelliteSeed(SolarBody solarBody) {
        return solarBody.seedOffset ^ 3 + seed;
    }

    /// <summary>
    /// Draws a colour map and uses it to generate a texture map for the given solarbody
    /// </summary>
    /// <param name="solarBody"></param>
    public void GenerateMap(SolarBody solarBody)
    {
        Color[] colourMap = DrawMap(solarBody);

        CreateTexture(colourMap, solarBody);
    }

    /// <summary>
    /// Creates a noise map, uses it to make a colour map
    /// </summary>
    /// <param name="solarBody"></param>
    /// <returns></returns>
    Color[] DrawMap(SolarBody solarBody)
    {
        float[,] noiseMap = new float[mapSize, mapSize];

        int satelitteSeed = GetSatelliteSeed(solarBody);

        NoiseGenerator.GenerateNoiseMap(noiseMap, satelitteSeed, noiseScale, octaves, persistance, lacunarity, offset);

        Color[] colourMap = ColourMapGenerator.GetColourMap(noiseMap, solarBody, falloffCurve);
        return colourMap;
    }

    /// <summary>
    /// Creates the texture on the solarBody 
    /// </summary>
    /// <param name="colourMap"></param>
    /// <param name="solarBody"></param>
    void CreateTexture(Color[] colourMap, SolarBody solarBody)
    {
        Texture2D texture = TextureFromColourMap(colourMap, mapSize, mapSize);

        solarBody.DrawTexture(texture);
    }

    /// <summary>
    /// Creates a texture from a colour map
    /// </summary>
    /// <param name="colourMap"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
	Texture2D TextureFromColourMap(Color[] colourMap, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colourMap);
        texture.Apply();
        return texture;
    }
}