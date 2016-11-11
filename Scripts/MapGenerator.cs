using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(TerrainManager))]
public class MapGenerator : MonoBehaviour {
	public enum DrawMode {NoiseMap, ColourMap, SunMap, MoonMap, FalloffMap};
	public DrawMode drawMode;
	
    public int mapSize = 241;
	public float noiseScale;

	[Range(1,5)]
	public int octaves;

	[Range(0,1)]
	public float persistance;
	public float lacunarity;
	
	public AnimationCurve animationCurve;

	public int seed;

	[Range(20, 100)]
	public int offsetSpeed;
	public Vector2 offset;
	
	public bool autoUpdate;
	public bool useFalloff;
	public bool movingMap;

	public TerrainType[] regions;
	private float[,] falloffMap;

    SolarManager solarManager;

	void Awake() {
        solarManager = FindObjectOfType<SolarManager>();
        falloffMap = FalloffGenerator.GenerateFalloffMap (mapSize, animationCurve);
        GenerateMap();
	}

	void Update(){
		if (movingMap) {
			offset.x += 0.01f;
			offset.y += 0.01f;
			GenerateMap();
		}
	}

    public void DrawMapInEditor(float[,] noiseMap, Color[] colourMap, SolarBody solarBody)
    {
        //MapDisplay display = FindObjectOfType<MapDisplay>();

        switch (solarBody.bodyType) {
            case SolarBody.BodyType.Barren:
                if(drawMode == DrawMode.FalloffMap)
                    solarBody.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(mapSize, animationCurve)));
                else
                    solarBody.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
                break;
            case SolarBody.BodyType.Earth:
            case SolarBody.BodyType.Moon:
            case SolarBody.BodyType.Sun:
            default:
                solarBody.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapSize, mapSize));
                break;
                    
        }
    }

    public void GenerateMap() {
        // manager = FindObjectOfType<SolarManager>();
        if (solarManager == null)
            solarManager = FindObjectOfType<SolarManager>();

        SolarBody solarBody = solarManager.sun.GetComponentInChildren<SolarBody>();

        if (solarBody == null)
            solarBody = FindObjectOfType<SolarBody>();

        GenerateMap(solarBody);
        GenerateSatelliteMaps(solarManager.solarBodies);
	}

    void GenerateSatelliteMaps(List<SolarBody> solarBodies) {
        foreach(SolarBody solarBody in solarBodies) {
            GenerateMap(solarBody);
            GenerateSatelliteMaps(solarBody.satellites);
        }
    }

    public void GenerateMap(SolarBody solarBody)
    {
        int newSeed = solarBody.seedOffset^3 + seed;
        float[,] noiseMap = Noise.GenerateNoiseMap(mapSize, mapSize, newSeed, noiseScale, octaves, persistance, lacunarity, offset);

        TerrainManager terrainManager = FindObjectOfType<TerrainManager>();
        TerrainType[] regionsInUse;

        switch (solarBody.bodyType)
        {
            case SolarBody.BodyType.Sun:
                regionsInUse = (terrainManager.getPalettes("Sun").Length == 0) ? regions : terrainManager.getPalettes("Sun");
                break;
            case SolarBody.BodyType.Moon:
                regionsInUse = (terrainManager.getPalettes("Moon").Length == 0) ? regions : terrainManager.getPalettes("Moon");
                break;
            case SolarBody.BodyType.Earth:
                regionsInUse = (terrainManager.getPalettes("Earth").Length == 0) ? regions : terrainManager.getPalettes("Earth");
                break;
            default:
                Debug.Log("using regions");
                regionsInUse = regions;
                break;
        }

        Color[] colourMap = new Color[mapSize * mapSize];

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {

                if (useFalloff)
                {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
                }

                float currentHeight = noiseMap[x, y];

                for (int i = 0; i < regionsInUse.Length; i++)
                {
                    if (currentHeight <= regionsInUse[i].height)
                    {
                        colourMap[y * mapSize + x] = regionsInUse[i].colour;
                        break;
                    }
                }
            }
        }

        DrawMapInEditor(noiseMap, colourMap, solarBody);
    }

    void OnValidate() {
		if (lacunarity < 1) {
			lacunarity = 1;
		}

		if (octaves < 0) {
			octaves = 0;
		}

        falloffMap = FalloffGenerator.GenerateFalloffMap (mapSize, animationCurve);
    }
}