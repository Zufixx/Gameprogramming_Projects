using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlatformManager : MonoBehaviour {

    private List<PlatformType> platformTypes = new List<PlatformType>();

    [SerializeField]
    private Transform cameraObject;

    [SerializeField]
    private GameObject platform;

    private float rng;
    private float platformY;
    private PlatformType prevPlatform;

    private void Start()
    {
        ReadFile();
        SortTypes();

        platformY = 0.0f;
        while (platformY < cameraObject.position.y + 6.0f)
        {
            SpawnPlatform();
        }
    }

    private void Update()
    {
        if (platformY < cameraObject.position.y + 6.0f)
            SpawnPlatform();
    }

    public void SpawnPlatform()
    {
        for (int i = 0; i < platformTypes.Count; i++)
        {
            rng = Random.Range(0f, 100f);
            if (rng <= platformTypes[i].probability)
            {
                //Debug.Log("Trying to spawn a " + platformTypes[i].typeName);
                if (prevPlatform == null || platformTypes[i].typeName != prevPlatform.typeName || platformTypes[i].probability == 100f)
                {
                    prevPlatform = platformTypes[i];

                    GameObject currentPlatform;
                    currentPlatform = Instantiate(platform, new Vector2(Random.Range(-1.8f, 1.8f), platformY), Quaternion.identity);
                    Platform platformScript = currentPlatform.GetComponent<Platform>();
                    platformScript.Initialize(platformTypes[i]);
                    platformY += platformTypes[i].space;
                }
                else
                    continue;
            }
        }
    }

    private void ReadFile()
    {
        string contents = string.Empty;
        using (FileStream fs = File.Open("Gamedata.txt", FileMode.Open))
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
                        float.Parse(value[1].TrimEnd()),    // JumpHeight
                        bool.Parse(value[2].TrimEnd()),     // Fragile
                        float.Parse(value[3].TrimEnd()),    // ColorR
                        float.Parse(value[4].TrimEnd()),    // ColorG
                        float.Parse(value[5].TrimEnd()),    // ColorB
                        float.Parse(value[6].TrimEnd()),    // ColorA
                        float.Parse(value[7].TrimEnd()),    // Probability
                        float.Parse(value[8].TrimEnd()),    // MovementX
                        float.Parse(value[9].TrimEnd()),    // MovementY
                        float.Parse(value[10].TrimEnd()),   // Time
                        float.Parse(value[11].TrimEnd())    // Space
                        );
                    platformTypes.Add(platformType);
                }
            }
        }
    }

    private void SortTypes()
    {
        // Sort list by probability
        for (int i = 0; i < platformTypes.Count; i++)
        {
            for (int j = 1; j <= i; j++)
            {
                if (platformTypes[j - 1].probability > platformTypes[j].probability)
                {
                    PlatformType temp = platformTypes[j - 1];
                    platformTypes[j - 1] = platformTypes[j];
                    platformTypes[j] = temp;
                }
            }
        }
        foreach(PlatformType pf in platformTypes)
        {
            Debug.Log(pf.typeName + ", " + pf.probability);
        }
    }
}
