using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Spawner : MonoBehaviour {

    private List<PlatformType> platformTypes = new List<PlatformType>();

    [SerializeField]
    private Transform cameraObject;

    [SerializeField]
    private GameObject platform;

    [SerializeField]
    private PlayerController playerController;

    private float rng;
    private float platformY;
    private PlatformType prevPlatform;

    [Header("Enemy Spawning")]

    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    [Range(0f, 100f)]
    private float enemyProbability = 2f;
    [SerializeField]
    private float enemyOffset = 5f;

    [Header("PowerUp Spawning")]

    [SerializeField]
    private GameObject powerupPrefab;
    [SerializeField]
    [Range(0f,100f)]
    private float powerupProbability = 5f;
    [SerializeField]
    private float powerupOffset = 0.3f;

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

                    // If it is a standard platform
                    if (platformTypes[i].probability == 100f)
                    {
                        //Debug.Log("Trying to spawn powerup");
                        if (rng < powerupProbability)
                        {
                            //Debug.Log("Spawned powerup");
                            Vector2 spawnPos = new Vector2(currentPlatform.transform.position.x, currentPlatform.transform.position.y + powerupOffset);
                            Instantiate(powerupPrefab, spawnPos, Quaternion.identity);
                        }
                    }
                }
                else
                    continue;
            }
        }
        // Spawn Enemies
        if (rng < enemyProbability && !playerController.powerUp)
        {
            Instantiate(enemyPrefab, new Vector2(Random.Range(-1.8f, 1.8f), platformY + enemyOffset), Quaternion.identity);
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
                        float.Parse(value[2].TrimEnd()),    // Width
                        bool.Parse(value[3].TrimEnd()),     // Fragile
                        float.Parse(value[4].TrimEnd()),    // ColorR
                        float.Parse(value[5].TrimEnd()),    // ColorG
                        float.Parse(value[6].TrimEnd()),    // ColorB
                        float.Parse(value[7].TrimEnd()),    // ColorA
                        float.Parse(value[8].TrimEnd()),    // Probability
                        float.Parse(value[9].TrimEnd()),    // MovementX
                        float.Parse(value[10].TrimEnd()),   // MovementY
                        float.Parse(value[11].TrimEnd()),   // Time
                        float.Parse(value[12].TrimEnd())    // Space
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
            //Debug.Log(pf.typeName + ", " + pf.probability);
        }
    }
}
