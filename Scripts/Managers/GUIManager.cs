using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    SolarManager solarManager;
    public CameraManager cameraManager;
    List<GameObject> solarBodies;

    void Start() {
        solarManager = GetComponent<SolarManager>();
        FillSolarBodies();
    }

    void FillSolarBodies() {
        solarBodies = new List<GameObject>();

        solarBodies.Add( solarManager.getSun() );

        foreach (SolarBody solarBody in solarManager.solarBodies)
        {
            solarBodies.Add(solarBody.gameObject);
            AddSatellites(solarBody);
        }
    }

    void AddSatellites(SolarBody solarBody)
    {
        foreach (SolarBody satellite in solarBody.satellites)
            solarBodies.Add(satellite.gameObject);
    }

    public void ExitButtonPress()
    {
        Application.Quit();
    }

    public void NextButtonPress() {
        int index = solarBodies.IndexOf(cameraManager.GetTarget());

        cameraManager.SetTarget( (index+1 < 0) ? solarBodies[index] : solarBodies[index+1] );
    }

    public void HomeButtonPress()
    {
        cameraManager.SetTarget(solarBodies[0]);
    }
    
    public void PrevButtonPress() {
        int index = solarBodies.IndexOf(cameraManager.GetTarget());

        cameraManager.SetTarget((index - 1 < 0) ? solarBodies[0] : solarBodies[index-1]);
    }
}
