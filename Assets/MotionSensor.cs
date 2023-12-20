using System.Collections;
using System.Globalization;
using System.IO;
using UnityEngine;

public class MotionSensor : MonoBehaviour
{
    private Vector3 currentVelocity = Vector3.zero;
    private Vector3 currentPosition = Vector3.zero;
    private float scale = 0.001f; // スケール係数をさらに小さく
    private float maxVelocity = 10f; // 最大速度の制限
    private float maxPosition = 100f; // 最大位置の制限

    void Start()
    {
        StartCoroutine(ReadDataAndMoveObject());
    }

    IEnumerator ReadDataAndMoveObject()
    {
        string path = "Assets/20231217114359_acceleration.txt";
        string[] lines = File.ReadAllLines(path);
        float lastTime = 0.0f;

        foreach (string line in lines)
        {
            string[] values = line.Split(',');
            if (values.Length >= 5)
            {
                if (float.TryParse(values[1], NumberStyles.Any, CultureInfo.InvariantCulture, out float currentTime) &&
                    float.TryParse(values[2], NumberStyles.Any, CultureInfo.InvariantCulture, out float x) &&
                    float.TryParse(values[3], NumberStyles.Any, CultureInfo.InvariantCulture, out float y) &&
                    float.TryParse(values[4], NumberStyles.Any, CultureInfo.InvariantCulture, out float z))
                {
                    float deltaTime = currentTime - lastTime;
                    Vector3 acceleration = new Vector3(x, y, z) * scale;

                    currentVelocity += acceleration * deltaTime;
                    currentVelocity = Vector3.ClampMagnitude(currentVelocity, maxVelocity);

                    currentPosition += currentVelocity * deltaTime;
                    currentPosition = Vector3.ClampMagnitude(currentPosition, maxPosition);

                    transform.position = currentPosition;

                    yield return new WaitForSeconds(0.1f); // 0.1秒待つ
                    lastTime = currentTime;
                }
                else
                {
                    Debug.LogError("NG");
                }
            }
        }
    }
}


/*
using System.Collections;
using System.Globalization;
using System.IO;
using UnityEngine;

public class MotionSensor : MonoBehaviour
{
    private Vector3 currentVelocity = Vector3.zero;
    private Vector3 currentPosition = Vector3.zero;

    void Start()
    {
        StartCoroutine(ReadDataAndMoveObject());
    }

    IEnumerator ReadDataAndMoveObject()
    {
        string path = "Assets/20231217114359_acceleration.txt";
        string[] lines = File.ReadAllLines(path);
        float lastTime = 0.0f;

        foreach (string line in lines)
        {
            string[] values = line.Split(',');
            if (values.Length >= 5)
            {
                if (float.TryParse(values[1], NumberStyles.Any, CultureInfo.InvariantCulture, out float currentTime) &&
                    float.TryParse(values[2], NumberStyles.Any, CultureInfo.InvariantCulture, out float x) &&
                    float.TryParse(values[3], NumberStyles.Any, CultureInfo.InvariantCulture, out float y) &&
                    float.TryParse(values[4], NumberStyles.Any, CultureInfo.InvariantCulture, out float z))
                {
                    float deltaTime = currentTime - lastTime;
                    Vector3 acceleration = new Vector3(x, y, z);

                    // 加速度から速度を計算
                    currentVelocity += acceleration * deltaTime;

                    // 速度から位置を計算
                    currentPosition += currentVelocity * deltaTime;

                    transform.position = currentPosition;

                    yield return new WaitForSeconds(0.1f); // 0.1秒待つ
                    lastTime = currentTime;
                }
                else
                {
                    Debug.LogError("NG");
                }
            }
        }
    }
}




















using System.Collections;
using System.Globalization;
using System.IO;
using UnityEngine;

public class MotionSensor : MonoBehaviour
{
    private Vector3 currentPosition = Vector3.zero;

    void Start()
    {
        StartCoroutine(ReadDataAndMoveObject());
    }

    IEnumerator ReadDataAndMoveObject()
    {
        string path = "Assets/20231217114359_acceleration.txt";
        string[] lines = File.ReadAllLines(path);
        float lastTime = 0.0f;

        foreach (string line in lines)
        {
            string[] values = line.Split(',');
            if (values.Length >= 5)
            {
                if (float.TryParse(values[1], NumberStyles.Any, CultureInfo.InvariantCulture, out float currentTime) &&
                    float.TryParse(values[2], NumberStyles.Any, CultureInfo.InvariantCulture, out float x) &&
                    float.TryParse(values[3], NumberStyles.Any, CultureInfo.InvariantCulture, out float y) &&
                    float.TryParse(values[4], NumberStyles.Any, CultureInfo.InvariantCulture, out float z))
                {
                    float deltaTime = currentTime - lastTime;
                    Vector3 acceleration = new Vector3(x, y, z);
                    currentPosition += IntegrateAcceleration(acceleration, deltaTime);
                    transform.position = currentPosition;
                    yield return new WaitForSeconds(0.1f); // 0.1秒待つ
                    lastTime = currentTime;
                }
                else
                {
                    Debug.LogError("NG");
                }
            }
        }
    }

    private Vector3 IntegrateAcceleration(Vector3 acceleration, float deltaTime)
    {
        // 台形計算法を使用して加速度から位置を計算
        // この例では、加速度を直接位置に変換する簡易的な方法を示しています
        // 実際には加速度から速度を積分し、さらに速度から位置を積分する必要があります
        return 0.5f * acceleration * deltaTime * deltaTime;
    }
}




/*


using System.Collections;
using System.Globalization;
using System.IO;
using UnityEngine;

public class DataReader : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ReadDataAndMoveObject());
    }

    IEnumerator ReadDataAndMoveObject()
    {
        string path = "Assets/20231210235904_magnetic.txt";
        string[] lines = File.ReadAllLines(path);

        foreach (string line in lines)
        {
            string[] values = line.Split(',');
            if (values.Length >= 5)
            {
                if (float.TryParse(values[2], NumberStyles.Any, CultureInfo.InvariantCulture, out float x) &&
                    float.TryParse(values[3], NumberStyles.Any, CultureInfo.InvariantCulture, out float y) &&
                    float.TryParse(values[4], NumberStyles.Any, CultureInfo.InvariantCulture, out float z))
                {
                    Vector3 newPosition = new Vector3(x, y, z);
                    transform.position = newPosition;
                    yield return new WaitForSeconds(0.1f); // 0.1秒待つ
                }
                else
                {
                    Debug.LogError("NG");
                }
            }
        }
    }
}




*/


