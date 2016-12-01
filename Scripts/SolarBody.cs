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
    public Orbit orbit;

    public List<SolarBody> satellites;

    public int radius;
    public int mass;
    public int seedOffset;

    public Transform axisOfOrbit;

    public Renderer textureRenderer;
    public Material material;

    void Start() {
        orbit = transform.GetComponent<Orbit>();
    }

    void Awake()
    {
        material = new Material(Shader.Find("Unlit/Texture"));
        textureRenderer.material = material;
    }

    void OnValidate()
    {
        if (axisOfOrbit != null)
            transform.localPosition = new Vector3(0, 0, radius);
    }
   
    /// <summary>
    /// Assigns key variables and sets up orbit
    /// </summary>
    /// <param name="center"></param>
    /// <param name="newDistance"></param>
    /// <param name="type"></param>
	public void Setup(Transform axisOfOrbit, int radius, BodyType bodyType, float orbitSpeed) {
        this.bodyType = bodyType;

        this.axisOfOrbit = axisOfOrbit;
        
        seedOffset = GenerateSeedOffset();

        this.radius = radius;
        SetOrbitRadius();

        textureRenderer = transform.gameObject.GetComponent<Renderer>();
        
        material = new Material(Shader.Find("Unlit/Texture"));

        textureRenderer.material = material;
        
        orbit = GetComponent<Orbit>();
        orbit.Setup(axisOfOrbit, orbitSpeed, radius);
    }

    /// <summary>
    /// Fetches the orbit if it hasn't been already and adjusts the solarbody position according to it's radius
    /// </summary>
    public void SetOrbitRadius()
    {
        if(orbit == null)
            orbit = GetComponent<Orbit>();

        transform.localPosition = new Vector3(0, 0, radius);

        orbit.radius = radius;
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
    /// Satellite Setup method which calculates a shorter distance from their axisOfOrbit and faster orbitSpeed
    /// </summary>
    /// <param name="center"></param>
    public void SetupSatellite(Transform center, float speed) {
        int newDistance = Mathf.RoundToInt(center.localPosition.z + center.localScale.x)/2 + 50;

        Setup(center, newDistance, BodyType.Moon, speed*2);

        mass /= 15;
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
        return Mathf.RoundToInt( Random.Range(2,2000) );
    }

    /// <summary>
    /// Sets the given texture to wrap aroung the sphere
    /// </summary>
    /// <param name="texture"></param>
    public void DrawTexture(Texture2D texture)
    {
        textureRenderer.sharedMaterial.mainTexture = texture;
    }
}
