using UnityEngine;
using System.Collections;


public class TerrainManager : MonoBehaviour{
	public TerrainTypes[] palettes;

    public bool empty() {
        return palettes.Length == 0;
    }

	public TerrainType[] getPalettes(string name){
        foreach(TerrainTypes palette in palettes)
            if (palette.name.Equals(name))
                return palette.palette;

        return new TerrainType[0] ;
    }
}

[System.Serializable]
public struct TerrainTypes {
	public string name;
	public TerrainType[] palette;
}

[System.Serializable]
public struct TerrainType {
	public string name;
	public float height;
	public Color colour;
}
