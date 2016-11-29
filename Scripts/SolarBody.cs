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
        
        textureRenderer = transform.gameObject.GetComponent<Renderer>();
        
        material = new Material(Shader.Find("Unlit/Texture"));

        textureRenderer.material = material;
        
        orbit = GetComponent<Orbit>();
        orbit.Setup(axisOfOrbit, orbitSpeed, radius);
    }

    public void SetOrbitRadius()
    {
        if(orbit == null)
            orbit = GetComponent<Orbit>();

        transform.localPosition = new Vector3(0, 0, radius);

        orbit.radius = radius;
    }

    /*public void SetSatelliteOrbitSpeed(float parentSpeed) {
        if (orbit == null)
            orbit = GetComponent<Orbit>();

        orbit.orbitSpeed = parentSpeed * 2;
    }*/

    public float GetOrbitSpeed()
    {
        if(orbit == null)
            orbit = GetComponent<Orbit>();

        return orbit.orbitSpeed;
    }

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

    public void ScaleBodySize() {
        transform.localScale = new Vector3(mass, mass, mass);
    }

    int GenerateSeedOffset()
    {
        return Mathf.RoundToInt( Random.Range(2,2000) );
    }

    public void DrawTexture(Texture2D texture)
    {
        textureRenderer.sharedMaterial.mainTexture = texture;
    }

    void Awake() {
        material = new Material(Shader.Find("Unlit/Texture"));
        textureRenderer.material = material;
    }
        
    void OnValidate()
    {
        if(axisOfOrbit != null)
            transform.localPosition = new Vector3(0,0, radius);
    }

    /*void OnMouseDown() {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<CameraManager>().target = hit.collider.gameObject.transform;
            }
        }
    }*/
}
