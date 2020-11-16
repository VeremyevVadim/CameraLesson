using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Security;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequest : MonoBehaviour
{
    private static string archiveName = "Archive.zip";
    private static string extractFolderName = "Extract";

    [MenuItem("Tools/Archive task")]
    public static void SetArchiveData()
    {
        LoadZipFile();
    }

    private static void LoadZipFile()
    {
        var outPath = Path.Combine(Application.persistentDataPath, archiveName);
        if (File.Exists(outPath))
        {
            UnpackZipFile(outPath);
            return;
        }
        
        // Download zip file
        var uri = "https://dminsky.com/settings.zip";
        var uwr = UnityWebRequest.Get(uri);

        var asyncOperation =  uwr.SendWebRequest();
        asyncOperation.completed += (ao) =>
        {
            if (uwr.isHttpError || uwr.isNetworkError)
            {
                Debug.Log(uwr.error);
                return;
            }
            
            outPath = Path.Combine(Application.persistentDataPath, archiveName);

            try
            {
                File.WriteAllBytes(outPath, uwr.downloadHandler.data);
            }
            catch (ArgumentNullException e)
            {
                EditorUtility.DisplayDialog("Exception", 
                    "No data to write. DownloadHandler.data is empty\n" + e.Message, "Ok");
                return;
            }
            catch (IOException e)
            {
                EditorUtility.DisplayDialog("Exception", 
                    "An I/O error occurred while opening the file.\n" + e.Message, "Ok");
                return;
            }

            UnpackZipFile(outPath);
        };
    }

    private static void UnpackZipFile(string outPath)
    {
        // Work with archive
        var extractPath = Path.Combine(Application.persistentDataPath, extractFolderName);
        
        var info = new DirectoryInfo(extractPath);
        if (!info.Exists)
        {
            ZipFile.ExtractToDirectory(outPath, extractPath);
        }

        // Find settings.json file
        var fileInfos = info.GetFiles();
        if (fileInfos.Length == 0)
        {
            return;
        }
            
        // Deserialize json file
        string settingsJson;
        try
        {
            settingsJson = File.ReadAllText(fileInfos[0].FullName);
        }
        catch (SecurityException e)
        {
            EditorUtility.DisplayDialog("Exception", 
                "The caller does not have the required permission.\n" + e.Message, "Ok");
            return;
        }        
        catch (IOException e)
        {
            EditorUtility.DisplayDialog("Exception", 
                "An I/O error occurred while opening the file.\n" + e.Message, "Ok");
            return;
        }
        
        var playerData = JsonUtility.FromJson<PlayerData>(settingsJson);

        // Set character speed
        var character = GameObject.Find("CharacterFirst");
        if (character is null)
        {
            Debug.Log("No character on scene");
            return;
        }

        var characterController = character.GetComponent<CharacterControl>();
        if (characterController is null)
        {
            Debug.Log("No controller on character");
            return;
        }
        
        characterController.movementSpeed = playerData.speed;
        
        Texture2D myTexture = new Texture2D(2, 2);
        var textureData = Convert.FromBase64String(playerData.base64Texture);
        myTexture.LoadImage(textureData);

        var myFloor = GameObject.Find("Floor");
        if (myFloor == null)
        {
            return;
        }
        
        var myMaterial =  myFloor.GetComponent<MeshRenderer>().sharedMaterial;
        if (myMaterial == null)
        {
            return;
        }
        myMaterial.SetTexture("_MainTex", myTexture);
    }
    
    
}

internal class PlayerData
{
    public float speed;
    public int health;
    public string fullName;
    public string base64Texture;
}


