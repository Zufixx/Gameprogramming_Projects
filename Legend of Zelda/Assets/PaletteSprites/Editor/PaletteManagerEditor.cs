using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(PaletteManager.Palette))]
public class PalettePropertyDrawer : PropertyDrawer
{
    const float colorElementHeight = 18;

    TextAsset paletteNames;

    TextAsset colorNames;

    public PalettePropertyDrawer() : base()
    {
        paletteNames = (TextAsset) AssetDatabase.LoadAssetAtPath("Assets/PaletteSprites/Customization/Palette_Names.txt", typeof(TextAsset));
        colorNames = (TextAsset)AssetDatabase.LoadAssetAtPath("Assets/PaletteSprites/Customization/Color_Names.txt", typeof(TextAsset));
    }
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = 16;

        if (property.isExpanded)
        {
            height += colorElementHeight * PaletteManager.colorCount;
        }

        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int index = int.Parse(label.text.Substring(label.text.IndexOf(' ')));

        string[] names = paletteNames.text.Split('\n');

        if (names.Length == 1 && String.IsNullOrEmpty(names[0]))
        {
            names = new string[0];
        }

        if (index < names.Length)
        {
            label.text = names[index];
        }
        else
        {
            label.text = label.text.Replace("Element", "Palette");
        }
        
        position.height = 16;

        property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label, false);
        
        if (property.isExpanded)
        {
            bool changed = false;
            
            SerializedProperty colorsArray = property.FindPropertyRelative("colors");
            
            float lastY = position.y;
            
            for (int i = 0; i < colorsArray.arraySize; i++)
            {
                Rect colorPos = new Rect(position);

                lastY += colorElementHeight;

                colorPos.y = lastY;
                colorPos.height = 16;
                
                SerializedProperty colorProp = colorsArray.GetArrayElementAtIndex(i);

                string[] color_names = colorNames.text.Split('\n');

                if (color_names.Length == 1 && String.IsNullOrEmpty(color_names[0]))
                {
                    color_names = new string[0];
                }

                string colorName;

                if (i < color_names.Length)
                {
                    colorName = color_names[i];
                }
                else
                {
                    colorName = "Color " + i;
                }

                GUIContent content = new GUIContent(colorName);

                EditorGUI.BeginChangeCheck();

                EditorGUI.PropertyField(colorPos, colorProp, content);

                changed |= EditorGUI.EndChangeCheck();
            }

            if (property.serializedObject.ApplyModifiedProperties() || (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "UndoRedoPerformed"))
            {
                changed = true;
            }

            if (changed)
            {
                PaletteManager manager = property.serializedObject.targetObject as PaletteManager;

                if (manager != null)
                {
                    manager.UpdateColors();
                }
            }
        }
    }
}

[CanEditMultipleObjects]
[CustomEditor(typeof(PaletteManager))]
public class PaletteManagerEditor : Editor
{
    SerializedProperty LiveEdit;

    SerializedProperty PaletteMaterial;

    SerializedProperty Palettes;

    SerializedProperty Texture;
    
    private void OnEnable()
    {
        LiveEdit = serializedObject.FindProperty("liveEditMode");
        PaletteMaterial = serializedObject.FindProperty("paletteMaterials");
        Palettes = serializedObject.FindProperty("palettes");
        Texture = serializedObject.FindProperty("tex");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SerializedProperty prop = serializedObject.FindProperty("m_Script");
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);
        EditorGUI.EndDisabledGroup();
        serializedObject.ApplyModifiedProperties();

        serializedObject.Update();

        EditorGUILayout.PropertyField(LiveEdit);

        EditorGUI.BeginDisabledGroup(LiveEdit.boolValue);

        EditorGUILayout.PropertyField(PaletteMaterial, true);

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(Texture);

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();

            PaletteManager manager = (PaletteManager) target;

            manager.UpdatePalettes();

            if (manager.tex != null)
            {
                manager.UpdateColors();
            }
            
            serializedObject.Update();
        }

        EditorGUI.EndDisabledGroup();

        EditorGUI.BeginDisabledGroup(!LiveEdit.boolValue);
        
        EditorGUILayout.PropertyField(Palettes, true);

        EditorGUI.EndDisabledGroup();

        serializedObject.ApplyModifiedProperties();
        
        if (GUILayout.Button("Generate Palette Asset"))
        {
            ((PaletteManager)target).UpdateColors();

            int count = AssetDatabase.FindAssets("", new[] { "Assets/PaletteSprites/Palettes" }).Length;

            if (!AssetDatabase.Contains(Texture.objectReferenceValue))
            {
                AssetDatabase.CreateAsset(Texture.objectReferenceValue as Texture2D, string.Format("Assets/PaletteSprites/Palettes/Palette{0}.asset", count + 1));
            }

            EditorGUIUtility.PingObject(Texture.objectReferenceValue);
        }
    }
}
