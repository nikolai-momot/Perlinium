using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshGenerator))]
public class MapGenerator : MonoBehaviour {	
    public int mapSize = 241;
	public float noiseScale;

	[Range(1,5)]
	public int octaves;

	[Range(0,1)]
	public float persistance;
	public float lacunarity;
	
	public AnimationCurve offsetCurve;

	public int seed;

	[Range(20, 100)]
	public int offsetSpeed;
	public Vector2 offset;
	
	public bool autoUpdate;
	public bool useFalloff;
	public bool movingMap;
    
	private float[,] falloffMap;

    SolarManager solarManager;

	void Awake() {
        solarManager = FindObjectOfType<SolarManager>();
        falloffMap = FalloffGenerator.GenerateFalloffMap (mapSize, offsetCurve);

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
        switch (solarBody.bodyType)
        {
            case BodyType.Moon:
                if (useFalloff)
                    solarBody.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(mapSize, offsetCurve)));
                else
                    solarBody.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
                break;
            case BodyType.GasGiant:
            case BodyType.Earth:
            case BodyType.Barren:
            case BodyType.Sun:
            default:
                solarBody.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapSize, mapSize));
                break;
        }
    }

    public void GenerateMap() {
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

    public int getSatelliteSeed(SolarBody solarBody) {
        return solarBody.seedOffset ^ 3 + seed;
    }

    public void GenerateMap(SolarBody solarBody)
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapSize, mapSize, getSatelliteSeed(solarBody), noiseScale, octaves, persistance, lacunarity, offset);
        
        if(solarBody.bodyType == BodyType.GasGiant)
            GasGiantGenerator.GenerateGasGiant(noiseMap);

        List<TerrainColour> regionsInUse = FindObjectOfType<TerrainManager>().getPalettes(solarBody.bodyType);
        
        Color[] colourMap = new Color[mapSize * mapSize];
        
        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {

                if (useFalloff)
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
                
                float currentHeight = noiseMap[x, y];

                for (int i = 0; i < regionsInUse.Count; i++)
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

        falloffMap = FalloffGenerator.GenerateFalloffMap (mapSize, offsetCurve);
    }
}