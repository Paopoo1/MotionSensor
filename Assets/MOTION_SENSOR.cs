using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MOTION_SENSOR: MonoBehaviour
{
    private List<Vector3> accelerationData;
    private Vector3 currentVelocity = Vector3.zero;
    private Vector3 currentPosition = Vector3.zero;
    private int dataIndex = 0;
    private float lastUpdateTime = 0f;
    private float updateInterval = 0.02f; // 50 Hz のサンプリングレート

    void Start()
    {
        accelerationData = LoadAccelerationData("20231217114359_acceleration.txt");
    }

    void Update()
    {
        if (Time.time - lastUpdateTime > updateInterval && dataIndex < accelerationData.Count)
        {
            // 加速度データを使用して速度と位置を更新
            Vector3 acceleration = accelerationData[dataIndex];
            currentVelocity += acceleration * updateInterval;
            currentPosition += currentVelocity * updateInterval;

            // オブジェクトの位置を更新
            transform.position = currentPosition;

            lastUpdateTime = Time.time;
            dataIndex++;
        }
    }

    List<Vector3> LoadAccelerationData(string filePath)
    {
        List<Vector3> data = new List<Vector3>();
        string[] lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            string[] values = line.Split(',');
            // 加速度データは X, Y, Z の順で CSV ファイルに記載されていると仮定
            Vector3 acceleration = new Vector3(
                float.Parse(values[1]), // X
                float.Parse(values[2]), // Y
                float.Parse(values[3])  // Z
            );
            data.Add(acceleration);
        }

        return data;
    }
}
