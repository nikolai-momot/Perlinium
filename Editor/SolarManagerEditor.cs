using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SolarManager))]
public class SolarManagerEditor : Editor
{
    /// <summary>
    /// The editor target
    /// </summary>
    SolarManager solarManager;
    /// <summary>
    /// If an item is removed then subsequent inspector calls will get a NullReference Error
    /// This is avoided by returning true ending of drawing the GUI early and redrawing it with the new items
    /// </summary>
    const bool REFRESH_EDITOR = true;

    /// <summary>
    /// Sets the eitor's target on editor enable
    /// </summary>
    void OnEnable()
    {
        solarManager = (SolarManager)target;
    }

    /// <summary>
    /// Ovverride draws the two main components of the custom editor
    /// </summary>
    public override void OnInspectorGUI()
    {
        if (DrawMainControlBox())
            return;

        GUILayout.Space(5);

        if (DrawSolarBodies())
        {
            solarManager.updateSolarBodies();
            return;
        }
    }

    /// <summary>
    /// Draws the box that contains the planet count and the the button that adds more
    /// </summary>
    /// <returns></returns>
    bool DrawMainControlBox()
    {
        GUILayout.BeginVertical("box");
        
            //DrawSunObjectField();

            if (DrawAddSolarBodyButton())
                return REFRESH_EDITOR;

        GUILayout.EndVertical();

        return !REFRESH_EDITOR;
    }

    /// <summary>
    /// Loops through each solar body and draws their respective interfaces
    /// </summary>
    /// <returns></returns>
    bool DrawSolarBodies()
    {
        if(solarManager.solarBodies.Count == 0)
            return !REFRESH_EDITOR;

        GUILayout.BeginVertical();

            foreach (SolarBody solarBody in solarManager.solarBodies)
                if (DrawSolarBody(solarBody))
                    return REFRESH_EDITOR;
            
        GUILayout.EndVertical();

        return !REFRESH_EDITOR;
    }

    /// <summary>
    /// Draws a planet's interface
    /// </summary>
    /// <param name="solarBody"></param>
    /// <returns></returns>
    bool DrawSolarBody(SolarBody solarBody)
    {
        if (solarBody == null)
            return REFRESH_EDITOR;

        
        GUILayout.BeginHorizontal("box");

            GUILayout.BeginVertical();

                if (DrawRemoveSolarBodyButton(solarBody))
                    return REFRESH_EDITOR;

                if (DrawSolarBodyMain(solarBody))
                    return REFRESH_EDITOR;

                if (DrawAddSatelliteButton(solarBody))
                    return REFRESH_EDITOR;

                if (solarBody.satellites.Count != 0)
                {
                    if (DrawSatellites(solarBody))
                        return REFRESH_EDITOR;
                }
        

            GUILayout.EndVertical();

        GUILayout.EndHorizontal();
        

        return !REFRESH_EDITOR;
    }

    /// <summary>
    /// Loops through all the satellites of a planet and draws their respective interfaces
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    bool DrawSatellites(SolarBody parent)
    {
        if (parent == null)
            return REFRESH_EDITOR;

        GUILayout.BeginVertical();
        
            foreach (SolarBody satellite in parent.satellites)
                if (DrawSatellite(satellite))
                    return REFRESH_EDITOR;
        
        GUILayout.EndVertical();

        return !REFRESH_EDITOR;
    }

    /// <summary>
    /// Draws a satellite's interface
    /// </summary>
    /// <param name="satellite"></param>
    /// <returns></returns>
    bool DrawSatellite(SolarBody satellite)
    {
        if (satellite == null)
            return !REFRESH_EDITOR;

        GUILayout.BeginHorizontal("box");

            GUILayout.BeginVertical();

                if (DrawRemoveSatelliteButton(satellite))
                        return REFRESH_EDITOR;

                if (DrawSolarBodyMain(satellite))
                    return REFRESH_EDITOR;

            GUILayout.EndVertical();

        GUILayout.EndHorizontal();

        return !REFRESH_EDITOR;
    }

    /// <summary>
    /// Draws the main information of a solar body
    /// </summary>
    /// <param name="solarBody"></param>
    /// <returns></returns>
    bool DrawSolarBodyMain(SolarBody solarBody)
    {
        if (solarBody == null)
            return REFRESH_EDITOR;

        GUILayout.BeginVertical();

            DrawNameField(solarBody);
            DrawBodyTypeField(solarBody);
            DrawMassField(solarBody);
            DrawDistanceFromAxisField(solarBody);
            DrawSeedOffsetField(solarBody);

        GUILayout.EndVertical();

        return !REFRESH_EDITOR;
    }

    /// <summary>
    /// Draws the button that adds planets and triggers the editor refresh
    /// </summary>
    /// <returns></returns>
    bool DrawAddSolarBodyButton()
    {
        GUILayout.BeginHorizontal();

            if (GUILayout.Button("Add Solar Body"))
            {
                solarManager.AddSolarBody();
                return REFRESH_EDITOR;
            }

        GUILayout.EndHorizontal();

        return !REFRESH_EDITOR;
    }

    /// <summary>
    /// Draws the button that removes the satellites and triggers the editor refresh
    /// </summary>
    /// <param name="solarBody"></param>
    /// <returns></returns>
    bool DrawRemoveSolarBodyButton(SolarBody solarBody)
    {
        GUILayout.BeginHorizontal();

            if (GUILayout.Button("Remove Solar Body"))
            {
                solarManager.RemoveSolarBody(solarBody);
                return REFRESH_EDITOR;
            }

        GUILayout.EndHorizontal();

        return !REFRESH_EDITOR;
    }

    /// <summary>
    /// Draws the button that adds a satellite and triggers the editor refresh
    /// </summary>
    /// <param name="solarBody"></param>
    /// <returns></returns>
    bool DrawAddSatelliteButton(SolarBody solarBody)
    {
        if (solarBody.bodyType != BodyType.Moon)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Satellite"))
            {
                solarManager.AddSatellite(solarBody);
                return REFRESH_EDITOR;
            }
            GUILayout.EndHorizontal();
        }

        return !REFRESH_EDITOR;
    }

    /// <summary>
    /// Draws the button that removes a satellite and triggers the editor refresh
    /// </summary>
    /// <param name="satellite"></param>
    /// <returns></returns>
    bool DrawRemoveSatelliteButton(SolarBody satellite)
    {

        GUILayout.BeginHorizontal();

            if (GUILayout.Button("Remove Satellite"))
            {
                solarManager.RemoveSolarBody(satellite);
                return REFRESH_EDITOR;
            }

        GUILayout.EndHorizontal();

        return !REFRESH_EDITOR;
    }
    
    /// <summary>
    /// Draws an object field for the sun's gameobject
    /// </summary>
    /*void DrawSunObjectField()
    {
        GUILayout.BeginHorizontal();

            string label = "Center of the solar system ";
            solarManager.solarAxis = (Transform)EditorGUILayout.ObjectField(label, solarManager.solarAxis, typeof(Transform), true);
            solarManager.updateSolarBodies();

        GUILayout.EndHorizontal();
    }*/

    /// <summary>
    /// Draws the offset field for a solar body
    /// </summary>
    /// <param name="solarBody"></param>
    void DrawSeedOffsetField(SolarBody solarBody)
    {
        GUILayout.BeginHorizontal();

            GUILayout.Space(5);

            GUILayout.Label("Seed Offset");

            solarBody.seedOffset = EditorGUILayout.IntField(solarBody.seedOffset);

            GUILayout.Space(5);

        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// Draws the distance from axis field for a solar body
    /// </summary>
    /// <param name="solarBody"></param>
    void DrawDistanceFromAxisField(SolarBody solarBody)
    {
        GUILayout.BeginHorizontal();

            GUILayout.Space(5);

            GUILayout.Label("Distance from Sun");

            solarBody.radius = EditorGUILayout.IntField(solarBody.radius);
        
            solarBody.SetOrbitRadius();

            GUILayout.Space(5);

        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// Draws the mass field for a solar body
    /// </summary>
    /// <param name="solarBody"></param>
    void DrawMassField(SolarBody solarBody)
    {
        GUILayout.BeginHorizontal();

            GUILayout.Space(5);

            GUILayout.Label("Mass");

            solarBody.mass = EditorGUILayout.IntField(solarBody.mass);

            solarBody.ScaleBodyMass();

            GUILayout.Space(5);

        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// Draws the body type field for a solar body
    /// </summary>
    /// <param name="solarBody"></param>
    void DrawBodyTypeField(SolarBody solarBody)
    {
        GUILayout.BeginHorizontal();

            solarBody.bodyType = (BodyType)EditorGUILayout.EnumPopup("Solar Body Type:", solarBody.bodyType);

        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// Draws the name field for a solar body
    /// </summary>
    /// <param name="solarBody"></param>
    void DrawNameField(SolarBody solarBody)
    {
        GUILayout.BeginHorizontal();

            GUILayout.Label("Name");

            solarBody.name = GUILayout.TextField(solarBody.name, GUILayout.Width(135));

        GUILayout.EndHorizontal();
    }
}
