using System;
using System.IO;
using UnityEngine;

public class DataLogger : MonoBehaviour
{
    void Start()
    {
        string filePath = "Assets/20231223123028_acceleration.txt"; // ファイルのパスを設定
        ReadData(filePath);
    }

    void ReadData(string file)
    {
        try
        {
            string[] lines = File.ReadAllLines(file);

            // 最初の行（ヘッダー）をスキップ
            for (int i = 1; i < lines.Length; i++)
            {
                string[] splitData = lines[i].Split(',');

                if (splitData.Length >= 5) // データの長さをチェック
                {
                    string timestamp = splitData[0];
                    string time = splitData[1];
                    string x = splitData[2];
                    string y = splitData[3];
                    string z = splitData[4];

                    // ログに表示
                    Debug.Log("Timestamp: " + timestamp + ", Time: " + time + ", X: " + x + ", Y: " + y + ", Z: " + z);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error reading the file:");
            Debug.LogError(e.Message);
        }
    }
}
