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
        foreach (SolarBody solarBody in solarBodies) {
            solarBody.transform.position = modifyPosition(solarBody);
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
            solarBody.transform.position = new Vector3(0,0, solarBody.distanceFromSun);
            //solarBody.transform.position = modifyPosition(solarBody);
        }
    }

    Vector3 modifyPosition(SolarBody solarBody){
		return (solarBody.transform.position - sun.transform.position).normalized * solarBody.distanceFromSun + sun.transform.position;
	}

    public Vector3 getLastPlanet() {
        return solarBodies[solarBodies.Count - 1].transform.localPosition;
    }
}
