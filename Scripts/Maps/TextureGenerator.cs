using UnityEngine;

public static class TextureGenerator {
	
    /// <summary>
    /// Creates a texture froma colour map
    /// </summary>
    /// <param name="colourMap"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
	public static Texture2D TextureFromColourMap(Color[] colourMap, int width, int height) {
		Texture2D texture = new Texture2D (width, height);
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.SetPixels (colourMap);
		texture.Apply ();
		return texture;
	}
}