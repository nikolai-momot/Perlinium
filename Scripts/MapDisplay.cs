using UnityEngine;
using System.Collections;

public class MapDisplay : MonoBehaviour {
	
	public Renderer planeRenderer;
	public Renderer sphereRenderer;

	public bool useSphere;
	
	public void DrawTexture(Texture2D texture) {
		Renderer textureRenderer = (useSphere) ? sphereRenderer : planeRenderer;

		textureRenderer.enabled = true;
		( (useSphere) ? planeRenderer : sphereRenderer ).enabled = false;

		textureRenderer.sharedMaterial.mainTexture = texture;
		textureRenderer.transform.localScale = new Vector3 (texture.width, texture.width, texture.height);
	}
	
}
