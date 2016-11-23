using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (MapGenerator))]
public class MapGeneratorEditor : Editor {
	public override void OnInspectorGUI(){
		MapGenerator mapGen = (MapGenerator)target;

        GUILayout.BeginVertical("box");

            DrawDefaultInspector();

		    if (GUILayout.Button ("Generate")) {
			    mapGen.GenerateMap();
		    }

        GUILayout.EndVertical();
	}
}
