using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(SphereCollider))]
[System.Serializable]
public class SolarBody : MonoBehaviour
{
    public enum BodyType { Sun, Moon, Earth, Barren };
    public BodyType bodyType;

    public List<SolarBody> satellites;

    public int distanceFromSun;
    public int mass;
    public int seedOffset = 0;

    public bool inOrbit = true;
    public int orbitSpeed = 20;

    public bool inRotation = true;
    public float rotationSpeed = 1f;

    public Transform axisOfOrbit;
    public Renderer textureRenderer;
    public Material material;

	public SolarBody(Transform center, int newDistance) {
        bodyType = BodyType.Earth;
        axisOfOrbit = center;

        distanceFromSun = newDistance;

        textureRenderer = transform.gameObject.GetComponent<Renderer>();
        material = new Material(Shader.Find("Unlit/Texture"));
        textureRenderer.material = material;
    }

    public void DrawTexture(Texture2D texture)
    {
        textureRenderer.sharedMaterial.mainTexture = texture;
        //textureRenderer.transform.localScale = new Vector3(texture.width, texture.width, texture.height);
    }

    void Awake() {
        material = new Material(Shader.Find("Unlit/Texture"));
        textureRenderer.material = material;
    }

    void Start()
    {
        //transform.position = modifyPosition();
    }

    void Update()
    {
        //orbit();

        rotate();
        
    }

    void orbit() {
        if (inOrbit)
        {
            transform.RotateAround(axisOfOrbit.transform.position, Vector3.up, orbitSpeed * Time.deltaTime);
            Vector3 desiredPosition = modifyPosition();
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * orbitSpeed);
        }
    }

    void rotate() {
        if (inRotation)
        {
            transform.Rotate(new Vector3(0, -1 * rotationSpeed, 0));
        }
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
        transform.localPosition = new Vector3(0,0,distanceFromSun);
    }

    public Vector3 modifyPosition()
    {
        return (transform.position - axisOfOrbit.transform.position).normalized * distanceFromSun + axisOfOrbit.transform.position;
    }
}
