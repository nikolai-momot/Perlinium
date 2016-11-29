using UnityEngine;
using System.Collections.Generic;

public class PaletteManager : MonoBehaviour{
	public List<Palette> palettes;
   
    public List<PaletteColour> getPalette(BodyType type)
    {
        foreach (Palette palette in palettes)
            if (palette.bodyType == type)
                return palette.palette;

        return new List<PaletteColour>();
    }
}

[System.Serializable]
public class Palette {
	public string name;
    public BodyType bodyType;
    public List<PaletteColour> palette;

    public Palette() {
        name = "";
        palette = new List<PaletteColour>();
    }
}

[System.Serializable]
public class PaletteColour
{
	public string name;
	public float height;
	public Color colour;

    public PaletteColour()
    {
        name = "";
        height = 0f;
        colour = Color.white;
    }
}
