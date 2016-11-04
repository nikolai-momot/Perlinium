using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {
	public enum DrawMode {NoiseMap, ColourMap, SunMap, MoonMap, FalloffMap};
	public DrawMode drawMode;
	
	public int mapChunkSize = 241;
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

	
	public TerrainTypes[] palettes;
	void Awake() {
		falloffMap = FalloffGenerator.GenerateFalloffMap (mapChunkSize, animationCurve);
		GenerateMap ();
	}

	void Update(){
		if (movingMap) {
			offset.x += 0.01f;
			offset.y += 0.01f;
			GenerateMap();
		}
	}
		
	public void DrawMapInEditor(float[,] noiseMap, Color[] colourMap) {		
		MapDisplay display = FindObjectOfType<MapDisplay> ();

		if (drawMode == DrawMode.NoiseMap) {
			display.DrawTexture (TextureGenerator.TextureFromHeightMap (noiseMap));
		} else if (drawMode == DrawMode.ColourMap || drawMode == DrawMode.SunMap || drawMode == DrawMode.MoonMap)
        {
			display.DrawTexture (TextureGenerator.TextureFromColourMap (colourMap, mapChunkSize, mapChunkSize));
		} else if (drawMode == DrawMode.FalloffMap) {
			display.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(mapChunkSize, animationCurve)));
		}
	}
	
	public void GenerateMap() {
		float[,] noiseMap = Noise.GenerateNoiseMap (mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);
		TerrainManager terrainManager = FindObjectOfType<TerrainManager>();
        TerrainType[] regionsInUse;

        //regionsInUse = (drawMode == DrawMode.SunMap) ? palettes[0].palette : regions;
        //regionsInUse = (drawMode == DrawMode.SunMap) ? terrainManager.getPalettes("Sun") : regions;

        switch (drawMode){
            case DrawMode.SunMap:
                regionsInUse = (terrainManager.empty()) ? regions : terrainManager.getPalettes("Sun");
                break;
            case DrawMode.MoonMap:
                regionsInUse = (terrainManager.empty()) ? regions : terrainManager.getPalettes("Moon");
                break;
            default:
                regionsInUse = regions;
                break;
        }

        Color[] colourMap = new Color[mapChunkSize * mapChunkSize];

		for (int y = 0; y < mapChunkSize; y++) {
			for (int x = 0; x < mapChunkSize; x++) {

				if (useFalloff) {
					noiseMap [x, y] = Mathf.Clamp01(noiseMap [x, y] - falloffMap [x, y]);
				}

				float currentHeight = noiseMap [x, y];

				for (int i = 0; i < regionsInUse.Length; i++) {
					if (currentHeight <= regionsInUse [i].height) {
						colourMap [y * mapChunkSize + x] = regionsInUse [i].colour;
						break;
					}
				}
			}
		}
		
		DrawMapInEditor (noiseMap, colourMap);
	}

	void OnValidate() {
		if (lacunarity < 1) {
			lacunarity = 1;
		}

		if (octaves < 0) {
			octaves = 0;
		}

		falloffMap = FalloffGenerator.GenerateFalloffMap (mapChunkSize, animationCurve);
	}
}