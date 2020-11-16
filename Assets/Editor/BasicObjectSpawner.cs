using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BasicObjectSpawner : EditorWindow
{
    string objectBaseName = "";
    int objectID = 1;
    GameObject objectToSpawn;
    int spawnAmount = 1;
    float objectScale = 1f;
    float spawnRadius = 5f;

    Texture2D headerSectionTexture;

    Color headerSectionColor = new Color(89f / 255f, 134f / 255f, 158f / 255f);

    Rect headerSection;
    Rect bodySection;

    GUISkin skin;

    [MenuItem("My Tools/Basic Object Spawner")]
    static void OpenWindow()
    {
        BasicObjectSpawner window = (BasicObjectSpawner)GetWindow(typeof(BasicObjectSpawner));
        window.minSize = new Vector2(300, 188);
        window.Show();
    }

    void OnEnable()
    {
        InitTextures();
        skin = Resources.Load<GUISkin>("GUI Styles/EnemyDesignerSkin");
    }

    void InitTextures()
    {
        headerSectionTexture = new Texture2D(1, 1);
        headerSectionTexture.SetPixel(0, 0, headerSectionColor);
        headerSectionTexture.Apply();
    }

    void OnGUI()
    {
        DrawLayouts();
        DrawHeader();
        DrawBody();
    }

    void DrawLayouts()
    {
        headerSection.x = 0;
        headerSection.y = 0;
        headerSection.width = Screen.width;
        headerSection.height = 50;

        bodySection.x = 0;
        bodySection.y = 50;
        bodySection.width = Screen.width;
        bodySection.height = Screen.width - 50f;

        GUI.DrawTexture(headerSection, headerSectionTexture);
       // GUI.DrawTexture(bodySection, bodySectionTexture);
    }

    void DrawHeader()
    {
        GUILayout.BeginArea(headerSection); //Every begin needs an end just like using "{}"

        GUILayout.Label("Basic Object Spawner", skin.GetStyle("ObjectSpawnerHeader"));

        GUILayout.EndArea();
    }

    void DrawBody()
    {
        GUILayout.BeginArea(bodySection);
        GUILayout.Space(10);

        objectBaseName = EditorGUILayout.TextField("Base Name", objectBaseName);
        objectID = EditorGUILayout.IntField("Object ID", objectID);
        objectScale = EditorGUILayout.Slider("Object Scale", objectScale, 0.1f, 10f);
        spawnRadius = EditorGUILayout.FloatField("Spawn Radius", spawnRadius);
        spawnAmount = EditorGUILayout.IntSlider("Spawn Amount", spawnAmount, 1, 100);
        objectToSpawn = (GameObject)EditorGUILayout.ObjectField("Prefab to Spawn", objectToSpawn, typeof(GameObject), false);


        GUILayout.Space(10);
        if (GUILayout.Button("Spawn Object!", GUILayout.Height(30)))
        {
            SpawnObject();
        }

        GUILayout.EndArea();
    }

    void SpawnObject()
    {
        if (objectToSpawn == null)
        {
            EditorGUILayout.HelpBox("This requires a [Prefab] before it can be created", MessageType.Warning);
            return;
        }
        if (objectBaseName == null || objectBaseName.Length < 1)
        {
            EditorGUILayout.HelpBox("This requires a [Base Name] before it can be created", MessageType.Warning);
            return;
        }

        for (int i = 0; i < spawnAmount; i++)
        {
            Vector2 spawnCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPos = new Vector3(spawnCircle.x, 0, spawnCircle.y);

            GameObject newObject = Instantiate(objectToSpawn, spawnPos, Quaternion.identity);
          //  newObject.transform.parent = GameObject.FindGameObjectWithTag("Group").transform;

            newObject.name = objectBaseName + "_" + objectID;
            newObject.transform.localScale = Vector3.one * objectScale;

            objectID++;
        }   
    }
}
