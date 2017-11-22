using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof (PlatformCreator))]
public class PlatformEditor : Editor
{
    private List<PlatformType> platformTypes = new List<PlatformType>();

    private string buttonLabel = "A refresh is needed to see the changes";

    // Variables for creating a new type
    private string new_typeName;
    private float new_width;
    private float new_jumpHeight;
    private bool new_fragile;
    private Color new_color;
    private float new_probability;
    private Vector2 new_speed;
    private float new_time;
    private float new_space;
    //stuff

    private bool initialCheck = true;

    private string filename = "Gamedata.txt";
    private string standardTypes = "Standard,7,10,False,0,1,0,1,100,0,0,0,1;Fragile,7,0,True,1,1,0,1,10,0,0,0,0.5;High Jump,7,20, False,1,0,0,1,10,0,0,0,2;Move X,7,10, False,0.9448276,0,1,1,10,1,0,0.5,1;Move Y,7,10, False,0,0,1,1,10,0,1,0.5,1;";

    private void Awake()
    {
        // Set all initial inspector values
        new_typeName = "";
        new_width = 7f;
        new_jumpHeight = 10f;
        new_fragile = false;
        new_color = Color.white;
        new_probability = 10f;
        new_speed = Vector2.zero;
        new_time = 0.5f;
        new_space = 1.0f;

        initialCheck = true;
        //CheckFileExists();
        //ReadFile();
        if (initialCheck)
        {
            CheckFileExists();
            ReadFile();
            initialCheck = false;
        }
    }

    public override void OnInspectorGUI()
    {
        PlatformCreator platformCreator = (PlatformCreator)target;

        base.OnInspectorGUI();

        filename = EditorGUILayout.TextField("Filename:", filename);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        DrawNewType();
        DrawCurrentTypes();

        if (GUILayout.Button("Reset Platform Types"))
        {
            CheckFileExists();
            File.WriteAllText(filename, standardTypes);
        }
    }

    private void CheckFileExists()
    {
        if (!File.Exists(filename))
        {
            Debug.Log("Checking file from Editor");
            File.AppendAllText(filename, standardTypes);
            Debug.Log("Written to file");
        }
    }

    private void ReadFile()
    {
        Debug.Log("Reading file from Editor");
        string contents = string.Empty;
        using (FileStream fs = File.Open(filename, FileMode.Open))
        using (StreamReader reader = new StreamReader(fs))
        {
            contents = reader.ReadToEnd();
            reader.Close();
            fs.Close();
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
                        value[0].Trim(),                    // TypeName (string)
                        float.Parse(value[1].TrimEnd()),    // Width    
                        float.Parse(value[2].TrimEnd()),    // JumpHeight
                        bool.Parse(value[3].TrimEnd()),     // Fragile
                        float.Parse(value[4].TrimEnd()),    // ColorR
                        float.Parse(value[5].TrimEnd()),    // ColorG
                        float.Parse(value[6].TrimEnd()),    // ColorB
                        float.Parse(value[7].TrimEnd()),    // ColorA
                        float.Parse(value[8].TrimEnd()),    // Probability
                        float.Parse(value[9].TrimEnd()),    // MovementX
                        float.Parse(value[10].TrimEnd()),    // MovementY
                        float.Parse(value[11].TrimEnd()) ,  // Time
                        float.Parse(value[12].TrimEnd())    // Space
                        );
                    platformTypes.Add(platformType);
                }
            }
        }
        Debug.Log("File read from Editor");
    }

    private void SaveAllToFile()
    {
        string contents = string.Empty;
        foreach(PlatformType pf in platformTypes)
        {
            contents =
                contents
                + pf.typeName + ","
                + pf.width + ","
                + pf.jumpHeight + ","
                + pf.fragile + ","
                + pf.color.r + ","
                + pf.color.g + ","
                + pf.color.b + ","
                + pf.color.a + ","
                + pf.probability + ","
                + pf.speed.x + ","
                + pf.speed.y + ","
                + pf.time + ","
                + pf.space + ";";
        }

        //Write some text to the test.txt file
        File.WriteAllText(filename, contents);
    }

    private void AddToFile(PlatformType pf)
    {
        string contents = string.Empty;
        using (FileStream fs = File.Open(filename, FileMode.Open))
        using (StreamReader reader = new StreamReader(fs))
        {
            contents = reader.ReadToEnd();
            reader.Close();
            fs.Close();
        }
        contents = 
            contents
            + pf.typeName + ","
            + pf.width + ","
            + pf.jumpHeight + ","
            + pf.fragile + ","
            + pf.color.r + ","
            + pf.color.g + ","
            + pf.color.b + ","
            + pf.color.a + ","
            + pf.probability + ","
            + pf.speed.x + ","
            + pf.speed.y + ","
            + pf.time + ","
            + pf.space + ";";

        //Write some text to the test.txt file
        File.WriteAllText(filename, contents);
    }

    private void DrawNewType()
    {
        EditorGUILayout.LabelField("Add new type: ", EditorStyles.boldLabel);
        new_typeName = EditorGUILayout.TextField(new GUIContent("Type name:", "Name used for debugging. No actual use in game."), new_typeName);
        new_width = EditorGUILayout.FloatField("Width:", new_width);
        new_jumpHeight = EditorGUILayout.FloatField("Jump height:", new_jumpHeight);
        new_fragile = EditorGUILayout.Toggle(new GUIContent("Fragile:", "Will break on impact with player if true"), new_fragile);
        new_color = EditorGUILayout.ColorField("Color:", new_color);
        new_probability = EditorGUILayout.Slider(new GUIContent("Probability (%):", "This will be used to sort the list on runtime and sequentually test each probability of spawning. Smaller values will be tested first."), new_probability, 0f, 100f);
        new_speed = EditorGUILayout.Vector2Field("Speed:", new_speed);
        new_time = EditorGUILayout.FloatField("Time (s):", new_time);
        new_space = EditorGUILayout.Slider(new GUIContent("Space:", "The amount of space between this and the next platform"), new_space, 0.1f, 30.0f);

        EditorGUILayout.Space();

        if (GUILayout.Button("Create new type"))
        {
            if (new_typeName != "" && new_typeName != null && new_probability <= 99f)
            {
                PlatformType newPlatform = new PlatformType(
                    new_typeName,
                    new_width,
                    new_jumpHeight,
                    new_fragile,
                    new_color.r,
                    new_color.g,
                    new_color.b,
                    new_color.a,
                    new_probability,
                    new_speed.x,
                    new_speed.y,
                    new_time,
                    new_space);

                AddToFile(newPlatform);
                buttonLabel = "New type \"" + new_typeName + "\" created. Refresh";
                new_typeName = "";
                new_width = 7f;
                new_jumpHeight = 10f;
                new_color = Color.white;
                new_probability = 10f;
                new_speed = Vector2.zero;
                new_time = 0.5f;
            }
            else if (new_probability > 99f)
            {
                buttonLabel = "Probability should not be higher than 99%";
                new_probability = 99f;
            }
            else
            {
                buttonLabel = "No values can be null";
            }
        }
        EditorGUILayout.LabelField(buttonLabel);
    }

    private void DrawCurrentTypes()
    {
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.LabelField("Current types: ", EditorStyles.boldLabel);
        for (int i = 0; i < platformTypes.Count; i++)
        {
            EditorGUILayout.LabelField("Type ID: " + i);
            platformTypes[i].typeName = EditorGUILayout.TextField("Type Name: ", platformTypes[i].typeName);
            platformTypes[i].width = EditorGUILayout.FloatField("Width:", platformTypes[i].width);
            platformTypes[i].jumpHeight = EditorGUILayout.FloatField("Jump Height:", platformTypes[i].jumpHeight);
            platformTypes[i].fragile = EditorGUILayout.Toggle(new GUIContent("Fragile:", "Will break on impact with player if true"), platformTypes[i].fragile);
            platformTypes[i].color = EditorGUILayout.ColorField("Color: ", platformTypes[i].color);
            platformTypes[i].probability = EditorGUILayout.Slider("Probability: ", platformTypes[i].probability, 0f, 100f);
            platformTypes[i].speed = EditorGUILayout.Vector2Field("Movement: ", platformTypes[i].speed);
            platformTypes[i].time = EditorGUILayout.FloatField("Time: ", platformTypes[i].time);
            platformTypes[i].space = EditorGUILayout.Slider(new GUIContent("Space:", "The amount of space between this and the next platform"), platformTypes[i].space, 0.1f, 30.0f);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Save all values"))
            {
                SaveAllToFile();
            }
            if (GUILayout.Button("Delete type"))
            {
                platformTypes.RemoveAt(i);
                SaveAllToFile();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("");
            EditorGUILayout.Space();
        }
    }
}