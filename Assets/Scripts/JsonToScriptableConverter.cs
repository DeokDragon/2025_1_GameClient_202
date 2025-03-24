#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class JsonToScriptableConverter : EditorWindow
{
    private string jsonFilePath = "";
    private string outputFolder = "Assets/ScriptableObjects/items";
    private bool createDatabase = true;

    [MenuItem("Tools/JSON to Scriptable Objects")]

    public static void ShowWindow()
    {
        GetWindow<JsonToScriptableConverter>("JSON to Scriptable Objects");
    }
    void OnGUI()
    {
        GUILayout.Label("JSON to Scriptable Object Converter", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        if (GUILayout.Button("Select JSON file"))
        {
            jsonFilePath = EditorUtility.OpenFilePanel("Select JSON File", "", "json");
        }
        EditorGUILayout.LabelField("Selected File : ", jsonFilePath);
        EditorGUILayout.Space();
        outputFolder = EditorGUILayout.TextField("Output Folder : ", outputFolder);
        createDatabase = EditorGUILayout.Toggle("Create Database Asset", createDatabase);
        EditorGUILayout.Space();

        if (GUILayout.Button("Convert to Scriptable Objects"))
        {
            if (string.IsNullOrEmpty(jsonFilePath))
            {
                EditorUtility.DisplayDialog("Error", "Please select a JSON file first!", "OK");
                return;
            }
            ConvertJsonToScriptableObjects();
        }
    }
    private void ConvertJsonToScriptableObjects()
    {
        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }
        string jsonText = File.ReadAllText(jsonFilePath);

        try
        {
            List<ItemData> itemDataList = JsonConvert.DeserializeObject<List<ItemData>>(jsonText);
            foreach (var itemData in itemDataList)
            {
                ItemSO itemSO = ScriptableObject.CreateInstance<ItemSO>();

                itemSO.id = itemData.id;
                itemSO.itemName = itemData.itemName;
                itemSO.nameEng = itemData.nameEng;
                itemSO.description = itemData.description;

                if (System.Enum.TryParse(itemData.itemTypeString, out ItemType parsedType))
                {
                    itemSO.ItemType = parsedType;
                }
                else
                {
                    Debug.LogWarning($"������ '{itemData.itemName}'�� �������� ���� Ÿ�� : {itemData.itemTypeString}");

                    itemSO.price = itemData.price;
                    itemSO.power = itemData.power;
                    itemSO.level = itemData.level;
                    itemSO.isStackable = itemData.isStackable;

                    if (!string.IsNullOrEmpty(itemData.iconPath))
                    {
                        itemSO.icon = AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/Resouces/{itemData.iconPath}.png");
                        if (itemSO.icon != null)
                        {
                            Debug.LogWarning($"������ '{itemData.nameEng}'�� �������� ã�� �� �����ϴ�. : {itemData.iconPath}");
                        }
                    }
                    string assetPath = $"{outputFolder}/Item_{itemData.id.ToString("D4")}_{itemData.nameEng}.asset";
                    AssetDatabase.CreateAsset(itemSO, assetPath);
                    itemSO.name = $"Item_{itemData.id.ToString("D4")}+{itemData.nameEng}";
                    createdItems.Add(itemSO);

                    EditorUtility.SetDirty(itemSO);
                }
            }
        }




        catch (System.Exception e)
        {
            EditorUtility.DisplayDialog("Error", $"Faild to Convert JSON : {e.Message}", "ok");
            Debug.LogError($"JSON ��ȯ ���� : {e}");
        }
    }

}
#endif
