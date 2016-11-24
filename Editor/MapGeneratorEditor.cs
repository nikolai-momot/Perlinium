using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (MapGenerator))]
public class MapGeneratorEditor : Editor {

    /// <summary>
    /// If an item is removed then subsequent inspector calls will get a NullReference Error
    /// This is avoided by returning true ending of drawing the GUI early and redrawing it with the new items
    /// </summary>
    const bool REFRESH_EDITOR = true;

    /// <summary>
    /// The editor target
    /// </summary>
    MapGenerator mapGenerator;

    /// <summary>
    /// Sets the editor target on editor enable
    /// </summary>
    void OnEnable() {
        mapGenerator = (MapGenerator)target;
    }

    /// <summary>
    /// Draws the default inspector with a generate map button
    /// </summary>
	public override void OnInspectorGUI(){

        GUILayout.BeginVertical("box");

            DrawDefaultInspector();

            DrawGenerateButton();

        GUILayout.EndVertical();
	}

    /// <summary>
    /// Draws the generate map button
    /// </summary>
    void DrawGenerateButton()
    {
        if (GUILayout.Button("Generate"))
        {
            mapGenerator.GenerateMap();
        }
    }
}
