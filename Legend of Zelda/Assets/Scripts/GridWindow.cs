using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GridWindow : EditorWindow
{

    Grid grid;

    public void init()
    {
        grid = (Grid)FindObjectOfType(typeof(Grid));
    }

    private void OnGUI()
    {
        grid.color = EditorGUILayout.ColorField(grid.color, GUILayout.Width(200));
    }
}
