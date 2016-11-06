using UnityEngine;
using System.Collections;

public class MapDisplay : MonoBehaviour {
	
	public Renderer planeRenderer;
	public Renderer sphereRenderer;
    public Renderer sunRenderer;

	public bool useSphere;
    public bool useSun;
	
	public void DrawTexture(Texture2D texture) {
		Renderer textureRenderer = (useSphere) ? sphereRenderer : planeRenderer;

		textureRenderer.enabled = true;
		( (useSphere) ? planeRenderer : sphereRenderer ).enabled = false;

        if(useSun) sunRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.sharedMaterial.mainTexture = texture;
		textureRenderer.transform.localScale = new Vector3 (texture.width, texture.width, texture.height);
	}
	
}
