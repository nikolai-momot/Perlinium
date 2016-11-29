using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (MapManager))]
public class MapGeneratorEditor : Editor {
    
    /// <summary>
    /// If an item is removed then subsequent inspector calls will get a NullReference Error
    /// This is avoided by returning true ending of drawing the GUI early and redrawing it with the new items
    /// </summary>
    const bool REFRESH_EDITOR = true;

    /// <summary>
    /// The editor target
    /// </summary>
    MapManager mapGenerator;

    /// <summary>
    /// Sets the editor target on editor enable
    /// </summary>
    void OnEnable() {
        mapGenerator = (MapManager)target;
    }

    /// <summary>
    /// Draws the default inspector with a generate map button
    /// </summary>
	public override void OnInspectorGUI(){

       GUILayout.BeginVertical("box");

            DrawDefaultInspector();

            if (DrawGenerateButton())
                return;

        GUILayout.EndVertical();
	}

    /// <summary>
    /// Draws the generate map button
    /// </summary>
    bool DrawGenerateButton()
    {
        if (GUILayout.Button("Generate"))
        {
            mapGenerator.GenerateMap();
            return REFRESH_EDITOR;
        }

        return !REFRESH_EDITOR;
    }
}
