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

    public List<SolarBody> satellites;

    public int distanceFromAxis;
    public int mass;
    public int seedOffset;
    
    public int orbitSpeed;
    public float rotationSpeed;

    public Transform axisOfOrbit;
    public Renderer textureRenderer;
    public Material material;


    /// <summary>
    /// Assigns key variables and sets up orbit
    /// </summary>
    /// <param name="center"></param>
    /// <param name="newDistance"></param>
    /// <param name="type"></param>
	public void setup(Transform center, int newDistance, BodyType type, int orbitSpeed) {
        bodyType = type;
        axisOfOrbit = center;
        
        seedOffset = generateSeedOffset();

        distanceFromAxis = newDistance;

        textureRenderer = transform.gameObject.GetComponent<Renderer>();
        material = new Material(Shader.Find("Unlit/Texture"));
        textureRenderer.material = material;

        this.orbitSpeed = orbitSpeed;
        rotationSpeed = 1f;
        
        Orbit orbit = GetComponent<Orbit>();
        orbit.Setup(center, orbitSpeed, newDistance);
    }

    /// <summary>
    /// Satellite version of the Setup method which calculates a shorter distance from their axisOfOrbit and faster orbitSpeed
    /// </summary>
    /// <param name="center"></param>
    public void setupSatellite(Transform center, int speed) {
        int newDistance = Mathf.RoundToInt(center.localPosition.z + center.localScale.x)/2 + 50;

        setup(center, newDistance, BodyType.Moon, speed);
        
        mass /= 15;
    }

    private int generateSeedOffset()
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
    
    void OnMouseDown() {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<CameraController>().target = hit.collider.gameObject.transform;
            }
        }
    }
    
    void OnValidate()
    {
        if(axisOfOrbit != null)
            transform.localPosition = new Vector3(0,0, adjustedDistance());
    }

    public int adjustedDistance() {
        return Mathf.RoundToInt( axisOfOrbit.position.z ) + distanceFromAxis;
    }
}
