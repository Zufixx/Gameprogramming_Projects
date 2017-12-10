﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(Grid))]
public class GridEditor : Editor {

    Grid grid;

    private int oldIndex;

    private void OnEnable()
    {
        oldIndex = 0;
        grid = (Grid)target;
    }

    [MenuItem("Assets/Create/TileSet")]
    static void CreateTileSet()
    {
        TileSet asset = ScriptableObject.CreateInstance<TileSet>();
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);

        if(string.IsNullOrEmpty(path))
        {
            path = "Assets";
        }
        else if(Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(path), "");
        }
        else
        {
            path += "/";
        }

        var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "TileSet.asset");
        AssetDatabase.CreateAsset(asset, assetPathAndName);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
        asset.hideFlags = HideFlags.DontSave;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();


        grid.spriteWidth = createSlider("Width:", grid.spriteWidth);
        grid.spriteHeight = createSlider("Height:", grid.spriteHeight);

        if(GUILayout.Button("Open Grid Window"))
        {
            GridWindow window = (GridWindow)EditorWindow.GetWindow(typeof(GridWindow));
            window.init();
        }

        // Tile Prefab
        EditorGUI.BeginChangeCheck();
        var newTilePrefab = (Transform)EditorGUILayout.ObjectField("Tile Prefab", grid.tilePrefab, typeof(Transform), false);
        if(EditorGUI.EndChangeCheck())
        {
            grid.tilePrefab = newTilePrefab;
            Undo.RecordObject(target, "Grid Changed");
        }

        // Tile Map
        EditorGUI.BeginChangeCheck();
        var newTileSet = (TileSet)EditorGUILayout.ObjectField("Tileset", grid.tileSet, typeof(TileSet), false);
        if(EditorGUI.EndChangeCheck())
        {
            grid.tileSet = newTileSet;
            Undo.RecordObject(target, "Grid Changed");
        }

        if(grid.tileSet != null)
        {
            EditorGUI.BeginChangeCheck();
            string[] names = new string[grid.tileSet.prefabs.Length];
            int[] values = new int[names.Length];

            for(int i = 0; i < names.Length; i++)
            {
                names[i] = grid.tileSet.prefabs[i] != null ? grid.tileSet.prefabs[i].name : "";
                values[i] = i;
            }

            int index = EditorGUILayout.IntPopup("Select Tile", oldIndex,names,values);

            if(EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Grid Changed");
                if(oldIndex != index)
                {
                    oldIndex = index;
                    grid.tilePrefab = grid.tileSet.prefabs[index];

                    float width = grid.tilePrefab.GetComponent<Renderer>().bounds.size.x;
                    float height = grid.tilePrefab.GetComponent<Renderer>().bounds.size.y;

                    grid.spriteWidth = width;
                    grid.spriteHeight = height;
                }
            }
        }
    }

    private float createSlider(string labelName, float sliderPosition)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(labelName);
        sliderPosition = EditorGUILayout.Slider(sliderPosition, 8f, 64f);
        GUILayout.EndHorizontal();

        return sliderPosition;
    }

    private void OnSceneGUI()
    {
        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        Event e = Event.current;
        Ray ray = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));
        Vector3 mousePos = ray.origin;

        if (e.isMouse && e.type == EventType.MouseDown)
        {
            GUIUtility.hotControl = controlID;
            e.Use();

            GameObject spawnGO;
            Transform prefab = grid.tilePrefab;

            if (prefab)
            {
                Undo.IncrementCurrentGroup();
                spawnGO = (GameObject)PrefabUtility.InstantiatePrefab(prefab.gameObject);
                Vector3 aligned = new Vector3(
                    Mathf.Floor(mousePos.x / grid.width) * grid.width + grid.width / 2.0f,
                    Mathf.Floor(mousePos.y / grid.height) * grid.height + grid.height / 2.0f);
                spawnGO.transform.position = aligned;
                spawnGO.transform.parent = grid.transform;
                Undo.RegisterCreatedObjectUndo(spawnGO, "Create " + spawnGO.name);
            }
        }

        if (e.isMouse && e.type == EventType.MouseUp)
        {
            GUIUtility.hotControl = 0;
        }
    }

}