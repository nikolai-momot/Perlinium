using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {
	
	public enum DrawMode {NoiseMap, ColourMap, FalloffMap};
	public DrawMode drawMode;
	
	public const int mapChunkSize = 241;
	public float noiseScale;
	
	public int octaves;
	[Range(0,1)]
	public float persistance;
	public float lacunarity;
	
	public int seed;
	public Vector2 offset;
	
	public bool autoUpdate;
	public bool useFalloff;

	public TerrainType[] regions;
	private float[,] falloffMap;

	void Awake() {
		falloffMap = FalloffGenerator.GenerateFalloffMap (mapChunkSize);
	}

	public void DrawMapInEditor(float[,] noiseMap, Color[] colourMap) {		
		MapDisplay display = FindObjectOfType<MapDisplay> ();

		if (drawMode == DrawMode.NoiseMap) {
			display.DrawTexture (TextureGenerator.TextureFromHeightMap(noiseMap));
		} else if (drawMode == DrawMode.ColourMap) {
			display.DrawTexture (TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
		}else if (drawMode == DrawMode.FalloffMap) {
			display.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(mapChunkSize)));
		}
	}
	
	public void GenerateMap() {
		float[,] noiseMap = Noise.GenerateNoiseMap (mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);
		
		Color[] colourMap = new Color[mapChunkSize * mapChunkSize];
		for (int y = 0; y < mapChunkSize; y++) {
			for (int x = 0; x < mapChunkSize; x++) {

				if (useFalloff) {
					noiseMap [x, y] = Mathf.Clamp01(noiseMap [x, y] - falloffMap [x, y]);
				}

				float currentHeight = noiseMap [x, y];

				for (int i = 0; i < regions.Length; i++) {
					if (currentHeight <= regions [i].height) {
						colourMap [y * mapChunkSize + x] = regions [i].colour;
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
		falloffMap = FalloffGenerator.GenerateFalloffMap (mapChunkSize);
	}
}

[System.Serializable]
public struct TerrainType {
	public string name;
	public float height;
	public Color colour;
}