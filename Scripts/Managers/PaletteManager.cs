using UnityEngine;
using System.Collections.Generic;

public class PaletteManager : MonoBehaviour{
	public List<Palette> palettes;
   
    /// <summary>
    /// Gets a palette of a specified body type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Palette getPalette(BodyType type)
    {
        foreach (Palette palette in palettes)
            if (palette.bodyType == type)
                return palette;

        return new Palette();
    }
}

/// <summary>
/// A palette of a body type
/// </summary>
[System.Serializable]
public class Palette {
	public string name;
    public BodyType bodyType;
    public List<PaletteColour> colours;

    public Palette() {
        name = "";
        colours = new List<PaletteColour>();
    }
}

/// <summary>
/// A colour in a body type palette
/// </summary>
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
