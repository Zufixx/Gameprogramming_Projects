using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof (PlatformCreator))]
public class PlatformEditor : Editor
{
    [SerializeField]
    public List<PlatformType> Types;

    // Variables for creating a new type
    private int new_id;
    private string new_typeName;
    private float new_jumpHeight;
    private Color new_color;
    private float new_probability;
    private Vector2 new_speed;
    private float new_time;

    private void Awake()
    {
        Types = new List<PlatformType>();

        // Set all initial inspector values
        new_id = Types.Count + 1;
        new_typeName = "Temp";
        new_jumpHeight = 10f;
        new_color = Color.white;
        new_probability = 10f;
        new_speed = Vector2.zero;
        new_time = 0.5f;

        ReadFile();
    }

    public override void OnInspectorGUI()
    {
        PlatformCreator platformCreator = (PlatformCreator)target;

        base.OnInspectorGUI();

        DrawNewType();
        DrawCurrentTypes();
        //DrawRemoveType();
    }

    private void ReadFile()
    {
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

                    PlatformType platformType = new PlatformType(
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
                    Types.Add(platformType);
                    i++;
                }
            }
        }
    }

    private void DrawNewType()
    {
        EditorGUILayout.LabelField("Add new type: ", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Type ID: " + new_id);
        new_typeName = EditorGUILayout.TextField("Type name:", new_typeName);
        new_color = EditorGUILayout.ColorField("Color: ", new_color);
        new_jumpHeight = EditorGUILayout.FloatField("Jump height: ", new_jumpHeight);
        new_probability = EditorGUILayout.Slider("Probability (%):", new_probability, 0f, 100f);

        EditorGUILayout.LabelField("");
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Movement:");
        new_speed = EditorGUILayout.Vector2Field("Speed: ", new_speed);
        new_time = EditorGUILayout.FloatField("Time (s): ", new_time);

        EditorGUILayout.Space();

        if (GUILayout.Button("Create new type"))
        {
            Debug.Log(new_typeName);
        }
    }

    private void DrawCurrentTypes()
    {
        EditorGUILayout.LabelField("");
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Current types: ", EditorStyles.boldLabel);
        for (int i = 0; i < Types.Count; i++)
        {
            Types[i].id = EditorGUILayout.IntField("Type ID: ", Types[i].id);
            Types[i].typeName = EditorGUILayout.TextField("Type Name: ", Types[i].typeName);
            Types[i].color = EditorGUILayout.ColorField("Color: ", Types[i].color);
            Types[i].probability = EditorGUILayout.FloatField("Probability: ", Types[i].probability);
            Types[i].speed = EditorGUILayout.Vector2Field("Movement: ", Types[i].speed);
            Types[i].time = EditorGUILayout.FloatField("Time: ", Types[i].time);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Save values"))
            {
                Debug.Log("Save values");
            }
            if (GUILayout.Button("Delete type"))
            {
                Debug.Log("Deleted type");
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.LabelField("");
            EditorGUILayout.Space();
        }
    }

    private void DrawRemoveType()
    {
        EditorGUILayout.LabelField("");
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Remove a type:", EditorStyles.boldLabel);
        EditorGUILayout.TextField("Type name: ", "");
        if (GUILayout.Button("Remove type"))
        {
            Debug.Log("Removed a type");
        }
    }
}

public class PlatformType : MonoBehaviour
{
    public int id;
    public string typeName;
    public float jumpHeight;
    public float colorR;
    public float colorG;
    public float colorB;
    public float probability;
    public float movementX;
    public float movementY;
    public float time;

    public Color color;
    public Vector2 speed;
    
    public PlatformType(int id, string typeName, float jumpHeight, float colorR, float colorG, float colorB, float probability, float movementX, float movementY, float time)
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

        color = new Color(colorR, colorG, colorB);
        speed = new Vector2(movementX, movementY);

    }
}