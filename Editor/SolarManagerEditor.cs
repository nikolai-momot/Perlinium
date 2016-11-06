using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(SolarManager))]
public class SolarManagerEditor : Editor
{
    SolarManager solarManager;

    void OnEnable()
    {
        //solarManager = (SolarManager)target;
    }

    /*public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }*/
}
