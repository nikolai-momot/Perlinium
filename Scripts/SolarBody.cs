using UnityEngine;
using System.Collections.Generic;

public enum BodyType { Sun, Moon, Earth, Barren, GasGiant }

[RequireComponent(typeof(Orbit))]
[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(SphereCollider))]
[System.Serializable]
public class SolarBody : MonoBehaviour
{
    public BodyType bodyType;
    Orbit orbit;

    public List<SolarBody> satellites;
    
    public int mass;
    public int seedOffset;
    
    Renderer textureRenderer;
    Material material;

    void Start() {
        orbit = transform.GetComponent<Orbit>();
    }

    void Awake()
    {
        textureRenderer = transform.gameObject.GetComponent<Renderer>();
        material = new Material(Shader.Find("Unlit/Texture"));
        textureRenderer.material = material;
    }
    
    /// <summary>
    /// Assigns key variables and sets up orbit
    /// </summary>
    /// <param name="center"></param>
    /// <param name="newDistance"></param>
    /// <param name="type"></param>
	public void Setup(float distanceFromSun, Transform axisOfOrbit, BodyType bodyType, bool isSatelitte) {

        this.bodyType = bodyType;
        
        this.seedOffset = GenerateSeedOffset();
        
        mass = (isSatelitte) ? Mathf.RoundToInt( mass / 15f ) : mass;
        
        material = new Material(Shader.Find("Unlit/Texture"));

        textureRenderer = transform.gameObject.GetComponent<Renderer>();
        textureRenderer.material = material;
        
        orbit = GetComponent<Orbit>();
        orbit.Setup(distanceFromSun, axisOfOrbit, isSatelitte);
    }
    
    /// <summary>
    /// Fetches the orbit if it hasn't been already and adjusts the solarbody position according to it's radius
    /// </summary>
    public void SetRadius(float radius)
    {
        if(orbit == null)
            orbit = GetComponent<Orbit>();
        
        orbit.radius = radius;
    }

    /// <summary>
    /// Returns the radius value from the Orbit object
    /// </summary>
    /// <returns></returns>
    public float GetRadius() {
        if (orbit == null)
            orbit = GetComponent<Orbit>(); 

        return orbit.radius;
    }

    /// <summary>
    /// Fetches the Orbit object if it hadn't been already and returns the speed of its orbit
    /// </summary>
    /// <returns></returns>
    public float GetOrbitSpeed()
    {
        if(orbit == null)
            orbit = GetComponent<Orbit>();

        return orbit.orbitSpeed;
    }

    /// <summary>
    /// Adds a solarbody to the list of sattellite solarbodies
    /// </summary>
    /// <param name="solarBody"></param>
    public void AddSatellite(SolarBody solarBody) {
        satellites.Add(solarBody);
    }
    
    /// <summary>
    /// Sets the the solarboy's size according to it's mass
    /// </summary>
    public void ScaleBodyMass() {
        transform.localScale = new Vector3(mass, mass, mass);
    }
    
    /// <summary>
    /// Returns a number to offset the seed generating the texture
    /// </summary>
    /// <returns></returns>
    int GenerateSeedOffset()
    {
        return Mathf.RoundToInt( Random.Range(2,2000)*mass );
    }

    /// <summary>
    /// Sets the given texture to wrap aroung the sphere
    /// </summary>
    /// <param name="texture"></param>
    public void DrawTexture(Texture2D texture)
    {
        if (textureRenderer == null)
            textureRenderer = transform.gameObject.GetComponent<Renderer>();

        textureRenderer.sharedMaterial.mainTexture = texture;
    }
}
    