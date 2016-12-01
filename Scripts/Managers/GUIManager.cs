using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public SolarManager solarManager;
    CameraManager cameraManager;
    public Dropdown dropdown;

    List<GameObject> solarBodies;

    void Start() {
        cameraManager = GetComponent<CameraManager>();
        FillSolarBodies();
        dropdown.options.Clear();
        foreach (GameObject solarbody in solarBodies)
        {
            dropdown.options.Add(new Dropdown.OptionData(solarbody.name));
        }
    }

    void FillSolarBodies() {
        solarBodies = new List<GameObject>();
        foreach (SolarBody solarBody in solarManager.solarBodies)
        {
            solarBodies.Add(solarBody.transform.gameObject);
            AddSatellite(solarBody);
        }
    }

    void AddSatellite(SolarBody solarBody)
    {
        foreach (SolarBody satellite in solarBody.satellites)
            solarBodies.Add(solarBody.transform.gameObject);
    }

    /*public void NextButtonPress() {
        cameraManager.target = GetNextSolarBody();
    }

    GameObject GetNextSolarBody()
    {
        return ()
    }

    int GetNextIndex() {
        return (currentSolarBodyIndex > solarBodies.Count-1): 
    }

    public void PrevButtonPress() {

    }*/
}
