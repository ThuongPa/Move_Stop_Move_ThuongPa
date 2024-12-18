using UnityEngine;
using System.IO;
public class DataUtilities : MonoBehaviour
{
    private const string DEFAULT_PATH = "/GameData";
    private const string DEFAULT_FILE_NAME = "PlayerData.json";
    private static string systemPath = Application.dataPath + DEFAULT_PATH +"/"+DEFAULT_FILE_NAME;

    // Lưu data vào đường dẫn mặc định 
    public static void SaveData<T>(T data)
    {
     
        if (!Directory.Exists(Path.GetDirectoryName(systemPath)))
        {
            // Nếu không tồn tại, tạo thư mục
            Directory.CreateDirectory(Path.GetDirectoryName(systemPath));
        }
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(systemPath, jsonData);
        Debug.Log("Đã lưu thành công Data tại : " + systemPath);
    }

    // Load data từ file Json mặc định 
    public static T LoadData<T>()
    {
        if (File.Exists(systemPath))
        {
            string jsonData = File.ReadAllText(systemPath);
            Debug.Log("Đã lưu thành công Data tại : " + systemPath);
            return JsonUtility.FromJson<T>(jsonData);
        }
        else
        {
            Debug.LogError("Không tìm thấy đường dẫn : " + systemPath);
            return default(T);
        }
    }

    // Cập nhật data theo file Json mặc định 
    public static void UpdateData<T>(T newData)
    {
        if (File.Exists(systemPath))
        {
            string jsonData = JsonUtility.ToJson(newData);
            File.WriteAllText(systemPath, jsonData);
        }
        else
        {
            Debug.LogError("Không tìm thấy đường dẫn : " + systemPath);
        }
    }

    // Xóa data theo file Json mặc định 
    public static void DeleteData(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        else
        {
            Debug.LogError("Không tìm thấy đường dẫn : " + filePath);
        }
    }

    // Lưu data theo đường dẫn 
    public static void SaveData<T>(T data, string filePath)
    {
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, jsonData);
    }

    // Load data từ file Json   
    public static T LoadData<T>(string filePath)
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            return JsonUtility.FromJson<T>(jsonData);
        }
        else
        {
            Debug.LogError("Không tìm thấy đường dẫn : " + filePath);
            return default(T);
        }
    }

    // Cập nhật data theo file Json chỉ định 
    public static void UpdateData<T>(T newData, string filePath)
    {
        if (File.Exists(filePath))
        {
            string jsonData = JsonUtility.ToJson(newData);
            File.WriteAllText(filePath, jsonData);
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }
    }
}
