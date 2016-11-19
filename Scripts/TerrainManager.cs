using UnityEngine;
using System.Collections.Generic;

public class TerrainManager : MonoBehaviour{
	public List<TerrainTypes> palettes;
   
    public List<TerrainColour> getPalettes(BodyType type)
    {
        foreach (TerrainTypes palette in palettes)
            if (palette.bodyType == type)
                return palette.palette;

        return new List<TerrainColour>();
    }
}

[System.Serializable]
public class TerrainTypes {
	public string name;
    public BodyType bodyType;
    public List<TerrainColour> palette;

    public TerrainTypes() {
        name = "";
        palette = new List<TerrainColour>();
    }
}

[System.Serializable]
public class TerrainColour
{
	public string name;
	public float height;
	public Color colour;

    public TerrainColour()
    {
        name = "";
        height = 0f;
        colour = Color.white;
    }
}
