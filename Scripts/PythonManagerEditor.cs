using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Scripting.Python;

[CustomEditor(typeof(PythonManager))]
public class PythonManagerEditor : Editor
{
    PythonManager target;

    private void OnEnable() {
        target = (PythonManager) target;
    }

    public override void OnInspectorGUI() {
        if (GUILayout.Button("Run Python Scripts", GUILayout.Height(50))) {
            foreach (string path in GameObject.Find("Python Manager").GetComponent<PythonScripts>().paths) {
                PythonRunner.RunFile(Application.dataPath + "/Python/" + path);
            }
        }
    }
}
