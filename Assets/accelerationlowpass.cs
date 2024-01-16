using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;

public class accelerationlowpass : MonoBehaviour
{
    float accelerometerUpdateInterval = 1.0f / 26.0f;
    //float accelerometerUpdateInterval = 1.0f / 10.0f;
    //float accelerometerUpdateInterval = 1.0f / 60.0f;
    private List<Vector3> accelerationData;
    [SerializeField]
    private int currentIndex = 0;
    private Vector3 currentVelocity = Vector3.zero;
    private Vector3 currentPosition = Vector3.zero;

    private float timeAccumulator = 0f; // 時間の蓄積
    private List<Vector3> buffer = new List<Vector3>(); // 0.5秒間のデータを保存するバッファ

    [SerializeField]
    string filePath = "Assets/20231223123028_acceleration.txt";

    void Start()
    {
        accelerationData = ReadAccelerationData(filePath);
    }

    void Update()
    {
        float deltaTime = Time.deltaTime; // フレーム間の経過時間
        timeAccumulator += deltaTime;

        while (currentIndex < accelerationData.Count && timeAccumulator >= accelerometerUpdateInterval)
        {
            buffer.Add(accelerationData[currentIndex]);
            timeAccumulator -= accelerometerUpdateInterval;
            currentIndex++;
        }

        if (buffer.Count >= (0.5f / accelerometerUpdateInterval))
        {
            Vector3 averageAcceleration = CalculateAverage(buffer);
            UpdatePosition(buffer[buffer.Count-1],deltaTime);

            Debug.Log($"Time: {Time.time}s, Position: {transform.position}");

            buffer.Clear();
        }
    }

    Vector3 CalculateAverage(List<Vector3> data)
    {
        Vector3 sum = Vector3.zero;
        foreach (Vector3 v in data)
        {
            sum += v;
        }
        return sum / data.Count;
    }

    void UpdatePosition(Vector3 acceleration, float deltaTime)
    {
        currentVelocity += acceleration * deltaTime;
        currentPosition += currentVelocity * deltaTime;
        transform.position = currentPosition;
    }

    List<Vector3> ReadAccelerationData(string filePath)
    {
        List<Vector3> data = new List<Vector3>();
        string[] lines = File.ReadAllLines(filePath);

        for (int i = 1; i < lines.Length; i++) // 最初の行をスキップ
        {
            string[] values = lines[i].Trim().Split(',');
            if (values.Length >= 6)
            {
                try
                {
                    // 加速度データはvalues[2], values[3], values[4]にあると想定
                    float x = float.Parse(values[2], CultureInfo.InvariantCulture);
                    float y = float.Parse(values[3], CultureInfo.InvariantCulture);
                    float z = float.Parse(values[4], CultureInfo.InvariantCulture);
                    data.Add(new Vector3(x, y, z));
                }
                catch (FormatException)
                {
                    Debug.LogError("Format error in line: " + lines[i]);
                }
            }
        }

        return data;
    }

}







//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using System.IO;
//using System.Globalization; // CultureInfoを使用するため


//public class accelerationlowpass : MonoBehaviour
//{
//    float accelerometerUpdateInterval = 1.0f / 60.0f;
//    float lowPassKernelWidthInSeconds = 1.0f;

//    private float lowPassFilterFactor;
//    private Vector3 lowPassValue = Vector3.zero;
//    private List<Vector3> accelerationData;
//    private int currentIndex = 0;
//    private Vector3 lastAcceleration = Vector3.zero;
//    private Vector3 lastVelocity = Vector3.zero;
//    private float lastUpdateTime = 0f;
//    private Vector3 currentVelocity = Vector3.zero;
//    private Vector3 currentPosition = Vector3.zero;


//    // ファイルパス
//    [SerializeField]
//    string filePath = "Assets/20231223123028_acceleration.txt";

//    void Start()
//    {
//        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
//        accelerationData = ReadAccelerationData(filePath);
//        if (accelerationData.Count > 0)
//        {
//            lowPassValue = accelerationData[0];
//        }

//        if (accelerationData.Count > 0)
//        {
//            lastAcceleration = accelerationData[0];
//        }

//        lastUpdateTime = Time.time;

//    }

//    void Update()
//    {
//        if (currentIndex < accelerationData.Count)
//        {
//            Vector3 currentAcceleration = accelerationData[currentIndex];
//            lowPassValue = LowPassFilterAccelerometer(lowPassValue, currentAcceleration);

//            float deltaTime = Time.time - lastUpdateTime;

//            // 台形法を使った速度の積分
//            currentVelocity += 0.5f * (lastAcceleration + currentAcceleration) * deltaTime;

//            // 台形法を使った位置の積分
//            currentPosition += 0.5f * (lastVelocity + currentVelocity) * deltaTime;

//            transform.position = currentPosition; // オブジェクトの位置を更新

//            lastAcceleration = currentAcceleration;
//            lastUpdateTime = Time.time;

//            currentIndex++;
//        }

//    }

//    Vector3 LowPassFilterAccelerometer(Vector3 prevValue, Vector3 currentValue)
//    {
//        return Vector3.Lerp(prevValue, currentValue, lowPassFilterFactor);
//    }

//    List<Vector3> ReadAccelerationData(string filePath)
//    {
//        List<Vector3> data = new List<Vector3>();
//        string[] lines = File.ReadAllLines(filePath);

//        for (int i = 1; i < lines.Length; i++) // 最初の行をスキップ
//        {
//            string[] values = lines[i].Trim().Split(',');
//            if (values.Length >= 5)
//            {
//                try
//                {
//                    float x = float.Parse(values[2], CultureInfo.InvariantCulture);
//                    float y = float.Parse(values[3], CultureInfo.InvariantCulture);
//                    float z = float.Parse(values[4], CultureInfo.InvariantCulture);
//                    data.Add(new Vector3(x, y, z));
//                }
//                catch (FormatException)
//                {
//                    Debug.LogError("Format error in line: " + lines[i]);
//                }
//            }
//        }

//        return data;



//    }

//}


