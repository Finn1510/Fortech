using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorInventoryHack : EditorWindow
{
    [SerializeField] Item item;
    
    [MenuItem("Window/InventoryHack")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<EditorInventoryHack>("InventoryHack");
    }

    private void OnGUI()
    {
        GUILayout.Label("ONLY WORKS IN PLAYMODE", EditorStyles.boldLabel);
        GUILayout.Space(10);
        GUILayout.Label("General");

        if (GUILayout.Button("Add selected Item to inventory"))
        {

        }
    }
}
