using UnityEngine;
using System.Collections.Generic;


[RequireComponent(typeof(PaletteManager))]
public class MapManager : MonoBehaviour {	
    public int mapSize = 241;
	public float noiseScale;

	[Range(1,5)]
	public int octaves;

	[Range(0,1)]
	public float persistance;

    [Range(0,1)]
	public float lacunarity;
	
	public AnimationCurve falloffCurve;

	public int seed;

	[Range(20, 100)]
	public int offsetSpeed;
	public Vector2 offset;
	
	public bool useFalloff;
	public bool movingMap;
    
	float[,] falloffMap;

    SolarManager solarManager;

	void Awake() {
        solarManager = FindObjectOfType<SolarManager>();

        GenerateMaps();
	}

	void Update(){
		if (movingMap) {
			offset.x += 0.01f;
			offset.y += 0.01f;
			GenerateMaps();
		}
    }

    void OnValidate()
    {
        GenerateMaps();
    }

    /// <summary>
    /// Draws the texture on the solarBody 
    /// </summary>
    /// <param name="colourMap"></param>
    /// <param name="solarBody"></param>
    public void DrawMapInEditor(Color[] colourMap, SolarBody solarBody)
    {
        solarBody.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapSize, mapSize));
    }

    /// <summary>
    /// Generates a texture map for every solarbody in the system
    /// </summary>
    public void GenerateMaps() {
        if (solarManager == null)
            solarManager = FindObjectOfType<SolarManager>();

        SolarBody solarBody = solarManager.GetComponentInChildren<SolarBody>();

        if (solarBody == null)
            solarBody = FindObjectOfType<SolarBody>();

        GenerateMap(solarBody);
        GenerateSatelliteMaps(solarManager.solarBodies);
	}

    /// <summary>
    /// Generates a texture map for every sattelite of the given planet
    /// </summary>
    /// <param name="solarBodies"></param>
    void GenerateSatelliteMaps(List<SolarBody> solarBodies) {
        foreach(SolarBody solarBody in solarBodies) {

            if (solarBody == null)
                continue;

            GenerateMap(solarBody);
            GenerateSatelliteMaps(solarBody.satellites);
        }
    }

    /// <summary>
    /// Modifies the seedOffest of the given solarbody
    /// </summary>
    /// <param name="solarBody"></param>
    /// <returns></returns>
    public int getSatelliteSeed(SolarBody solarBody) {
        return solarBody.seedOffset ^ 3 + seed;
    }

    /// <summary>
    /// Generates a texture map for the given solarbody
    /// </summary>
    /// <param name="solarBody"></param>
    public void GenerateMap(SolarBody solarBody)
    {
        float[,] noiseMap = NoiseGenerator.GenerateNoiseMap(mapSize, mapSize, getSatelliteSeed(solarBody), noiseScale, octaves, persistance, lacunarity, offset);

        List<PaletteColour> regionsInUse = FindObjectOfType<PaletteManager>().getPalette(solarBody.bodyType);

        Color[] colourMap = MapGenerator.GenerateColourMap(noiseMap, solarBody, regionsInUse, falloffCurve);
       
        DrawMapInEditor(colourMap, solarBody);
    }
}