using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof (PlatformCreator))]
public class PlatformEditor : Editor
{
    [SerializeField]
    public List<Type> Types;

    private void Awake()
    {
        Types = new List<Type>();

        int i = 0;

        string contents = string.Empty;
        using (FileStream fs = File.Open("Gamedata.txt", FileMode.Open))
        using (StreamReader reader = new StreamReader(fs))
        {
            contents = reader.ReadToEnd();
        }
        if (contents.Length > 0)
        {
            string[] lines = contents.Split(new char[] { ';' });
            foreach (string line in lines)
            {
                if (!string.IsNullOrEmpty(line.Trim()) && !line.StartsWith("//"))
                {
                    string[] value = line.Split(new char[] { ',' });

                    Type type = new Type(
                        i,                                  // ID (int)
                        value[0].Trim(),                    // TypeName (string)
                        float.Parse(value[1].TrimEnd()),    // JumpHeight
                        float.Parse(value[2].TrimEnd()),    // ColorR
                        float.Parse(value[3].TrimEnd()),    // ColorG
                        float.Parse(value[4].TrimEnd()),    // ColorB
                        float.Parse(value[5].TrimEnd()),    // Probability
                        float.Parse(value[6].TrimEnd()),    // MovementX
                        float.Parse(value[7].TrimEnd()),    // MovementY
                        float.Parse(value[8].TrimEnd())     // Time
                        );
                    Types.Add(type);
                    i++;
                }
            }
        }
    }

    public override void OnInspectorGUI()
    {
        PlatformCreator platformCreator = (PlatformCreator)target;

        base.OnInspectorGUI();

        EditorGUILayout.LabelField("Current types: ", EditorStyles.boldLabel);
        for (int i = 0; i < Types.Count; i++)
        {
            EditorGUILayout.LabelField("Type ID: " + Types[i].GetID());
            EditorGUILayout.LabelField("Type Name: " + Types[i].GetTypeName());
            EditorGUILayout.ColorField("Color: ", Types[i].GetColor());
            EditorGUILayout.LabelField("Probability: " + Types[i].GetProbability() + "%");
            EditorGUILayout.Vector2Field("Movement: ", Types[i].GetMovement());
            EditorGUILayout.LabelField("Time: " + Types[i].GetTime() + "s");
            EditorGUILayout.Space();
        }
        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("Add new type: ", EditorStyles.boldLabel);
        EditorGUILayout.TextField("Type name:", "");
        EditorGUILayout.ColorField("Color: ", Color.white);
        EditorGUILayout.FloatField("Jump height: ", 10f);
        EditorGUILayout.Slider("Probability (%):", 10f, 0f, 100f);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Movement:");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Vector2Field("Speed: ", new Vector2());
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.FloatField("Time (s): ", 0.5f);

        EditorGUILayout.Space();

        if (GUILayout.Button("Create new type"))
        {
            Debug.Log("Created a new type");
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("");

        EditorGUILayout.LabelField("Remove a type:", EditorStyles.boldLabel);
        EditorGUILayout.TextField("Type name: ", "");
        if (GUILayout.Button("Remove type"))
        {
            Debug.Log("Removed a type");
        }
    }
}

public class Type : MonoBehaviour
{
    int id;
    string typeName;
    float jumpHeight;
    float colorR;
    float colorG;
    float colorB;
    float probability;
    float movementX;
    float movementY;
    float time;
    
    public Type(int id, string typeName, float jumpHeight, float colorR, float colorG, float colorB, float probability, float movementX, float movementY, float time)
    {
        this.id = id;
        this.typeName = typeName;
        this.jumpHeight = jumpHeight;
        this.colorR = colorR;
        this.colorG = colorG;
        this.colorB = colorB;
        this.probability = probability;
        this.movementX = movementX;
        this.movementY = movementY;
        this.time = time;
    }

    public int GetID() { return id; }
    public string GetTypeName() { return typeName; }
    public float GetJumpHeight() { return jumpHeight; }
    public Color GetColor() { return new Color(colorR, colorG, colorB); }
    public float GetProbability() { return probability; }
    public Vector2 GetMovement() { return new Vector2(movementX, movementY); }
    public float GetTime() { return time; }
}