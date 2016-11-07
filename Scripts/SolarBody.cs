using UnityEngine;

[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(MeshRenderer))]
[System.Serializable]
public class SolarBody : MonoBehaviour
{
    public enum BodyType { Sun, Moon, Earth, Barren };
    public BodyType bodyType;

    public int distanceFromSun;
    public int mass;

    public bool inOrbit;
    public int orbitSpeed;

    public bool inRotation;
    public float rotationSpeed;

    public Transform axisOfOrbit;
    public Renderer textureRenderer;
    public Material material;
	public SolarBody(Transform center, int minimumDistance) { 
        name = "";

        axisOfOrbit = center;
        textureRenderer = transform.gameObject.GetComponent<Renderer>();
        material = new Material(Shader.Find("Transparent/Diffuse"));
        textureRenderer.material = material;
        textureRenderer.sharedMaterial = material;
        bodyType = BodyType.Earth;

        distanceFromSun = minimumDistance;
        mass = 350;

        inOrbit = true;
        orbitSpeed = 20;

        inRotation = true;
        rotationSpeed = 1f;
    }

    public void DrawTexture(Texture2D texture)
    {
        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(texture.width, texture.width, texture.height);
    }

    void Awake() {
        material = new Material(Shader.Find("Unlit/Texture"));
        textureRenderer.material = material;
        textureRenderer.sharedMaterial = material;
    }

    void Start()
    {
        transform.position = modifyPosition();
    }

    void Update()
    {
        orbit();

        rotate();
    }

    public void orbit() {
        if (inOrbit)
        {
            transform.RotateAround(axisOfOrbit.transform.position, Vector3.up, orbitSpeed * Time.deltaTime);
            Vector3 desiredPosition = modifyPosition();
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * orbitSpeed);
        }
    }

    public void rotate() {
        if (inRotation)
            transform.Rotate(new Vector3(0, -1 * rotationSpeed, 0));
    }

    void OnValidate()
    {
        transform.localPosition = new Vector3(0,0,distanceFromSun);
        transform.localScale = new Vector3(mass,mass,mass);
    }

    Vector3 modifyPosition()
    {
        return (transform.position - axisOfOrbit.transform.position).normalized * distanceFromSun + axisOfOrbit.transform.position;
    }
}
