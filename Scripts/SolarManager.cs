using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class SolarManager : MonoBehaviour
{
    public GameObject sun;

    //is the solar system in Motion
    public bool solarOrbit;

    //planets, moons, etc
    public List<SolarBody> solarBodies;

	void Start(){
        transform.GetComponentInChildren<CameraController>().target = sun.transform;

        foreach (SolarBody solarBody in solarBodies) {
            solarBody.transform.position = solarBody.modifyPosition();

        }
	}

    void OnValidate()
    {
        updateVariables();
    }

    public void updateVariables()
    {
        foreach (SolarBody solarBody in solarBodies)
        {
            solarBody.inOrbit = solarOrbit;
            solarBody.transform.localScale = new Vector3(solarBody.mass, solarBody.mass, solarBody.mass);
            solarBody.transform.position = new Vector3(0,0, solarBody.adjustedDistance());
        }
    }

    public Vector3 getLastPlanetPosition() {
        return (solarBodies.Count == 0) ? sun.transform.localPosition : solarBodies[solarBodies.Count - 1].transform.localPosition;
    }
    
    void RemoveSatellites(SolarBody solarBody)
    {
        foreach (SolarBody body in solarBody.satellites)
            RemoveSolarBody(body);
    }

    public void RemoveSolarBody(SolarBody solarBody)
    {
        if (solarBody == null)
            return;

        solarBodies.Remove(solarBody);
        RemoveSatellites(solarBody);
        DestroyImmediate(solarBody.gameObject);
    }

    public void AddSolarBody(SolarBody parent)
    {
        GameObject newBody = Instantiate(Resources.Load("Prefab/SolarBody", typeof(GameObject))) as GameObject;
        newBody.transform.SetParent(sun.transform);

        SolarBody newSolar = newBody.GetComponent<SolarBody>();
        newSolar.setupSatellite(parent.transform);
        parent.satellites.Add(newSolar);

        string newName = "Satellite " + solarBodies.Count + " of " + parent.name;
        newBody.name = newName;
        newSolar.name = newName;

        //newSolar.axisOfOrbit = parent.transform;

        //newSolar.mass /= 15;
        //newSolar.distanceFromAxis /= 20;
        newBody.transform.localPosition = new Vector3(0, 0, newSolar.distanceFromAxis);
    }

    public void AddSolarBody()
    {
        GameObject newBody = Instantiate(Resources.Load("Prefab/SolarBody", typeof(GameObject))) as GameObject;
        newBody.transform.SetParent(sun.transform, false);

        SolarBody newSolar = newBody.GetComponent<SolarBody>();
        newSolar.setup(sun.transform, Mathf.RoundToInt(getLastPlanetPosition().z)*2, SolarBody.BodyType.Earth);
        solarBodies.Add(newSolar);

        string newName = "Planet " + solarBodies.Count;
        newBody.name = newName;
        newSolar.name = newName;

        newSolar.distanceFromAxis += Mathf.RoundToInt(getLastPlanetPosition().z);

        newBody.transform.localPosition = new Vector3(0, 0, newSolar.distanceFromAxis);

    }
}
