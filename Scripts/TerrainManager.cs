using UnityEngine;
using System.Collections.Generic;

public class TerrainManager : MonoBehaviour{
	public List<TerrainTypes> palettes;

    void OnValidate()
    {
       /* MapGenerator mapGenerator = FindObjectOfType<MapGenerator>();
        mapGenerator.GenerateMap();*/
    }

    public TerrainType[] getPalettes(string name){
        foreach(TerrainTypes palette in palettes)
            if (palette.name.Equals(name))
                return palette.palette.ToArray();

        return new TerrainType[0];
    }
}

[System.Serializable]
public class TerrainTypes {
	public string name;
	public List<TerrainType> palette;

    public TerrainTypes() {
        name = "";
        palette = new List<TerrainType>();
    }
}

[System.Serializable]
public class TerrainType {
	public string name;
	public float height;
	public Color colour;

    public TerrainType()
    {
        name = "";
        height = 0f;
        colour = Color.white;
    }
}
