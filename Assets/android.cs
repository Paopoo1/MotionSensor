using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using System.Collections.Generic;


public class android : MonoBehaviour
{
    private TcpListener tcpListener;
    private Thread tcpListenerThread;
    private TcpClient connectedTcpClient;

    private Vector3 currentAcceleration = Vector3.zero;
    private Vector3 currentVelocity = Vector3.zero;
    private Vector3 currentPosition = Vector3.zero;

    private float lowPassKernelWidthInSeconds = 1/26f; // ローパスフィルタの幅
    private Vector3 lowPassValue = Vector3.zero;

    void Start()
    {
        tcpListenerThread = new Thread(new ThreadStart(ListenForIncomingRequests));
        tcpListenerThread.IsBackground = true;
        tcpListenerThread.Start();
    }

    private void ListenForIncomingRequests()
    {
        try
        {
            tcpListener = new TcpListener(IPAddress.Parse("192.168.128.177"), 5000);
            tcpListener.Start();
            Byte[] bytes = new Byte[1024];
            while (true)
            {
                using (connectedTcpClient = tcpListener.AcceptTcpClient())
                {
                    using (NetworkStream stream = connectedTcpClient.GetStream())
                    {
                        int length;
                        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            var incomingData = new byte[length];
                            Array.Copy(bytes, 0, incomingData, 0, length);
                            string clientMessage = Encoding.ASCII.GetString(incomingData);
                            ProcessData(clientMessage);
                        }
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("SocketException " + socketException.ToString());
        }
    }

    private Queue<Vector3> accelerationQueue = new Queue<Vector3>();

    private void ProcessData(string data)
    {
        Debug.Log("Received data: " + data);
        Debug.Log("Received data:" + data);
        try
        {
            string[] splitData = data.Split(',');
            if (splitData.Length == 3)
            {
                float x = float.Parse(splitData[0].Split(':')[1]);
                float y = float.Parse(splitData[1].Split(':')[1]);
                float z = float.Parse(splitData[2].Split(':')[1]);

                Vector3 newAcceleration = new Vector3(x, y, z);
                lock (accelerationQueue)
                {
                    accelerationQueue.Enqueue(newAcceleration);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in ProcessData: " + ex.Message);
        }
    }

    void Update()
    {
        if (accelerationQueue.Count > 0)
        {
            Vector3 newAcceleration;
            lock (accelerationQueue)
            {
                newAcceleration = accelerationQueue.Dequeue();
            }

            ApplyLowPassFilter(newAcceleration);
            UpdatePositionAndVelocity(Time.deltaTime);
        }
    }
    private void ApplyLowPassFilter(Vector3 newAcceleration)
    {
        // ローパスフィルタの適用
        lowPassValue = Vector3.Lerp(lowPassValue, newAcceleration, Time.deltaTime / lowPassKernelWidthInSeconds);
        currentAcceleration = lowPassValue;
    }

    private void UpdatePositionAndVelocity(float deltaTime)
    {
        // 速度を更新 (v = u + at)
        currentVelocity += currentAcceleration * deltaTime;

        // 位置を更新 (s = s + vt)
        currentPosition += currentVelocity * deltaTime;

        // オブジェクトの位置を更新
        transform.position = currentPosition;
    }


    void OnApplicationQuit()
    {
        if (tcpListenerThread != null)
        {
            tcpListenerThread.Abort();
        }

        if (tcpListener != null)
        {
            tcpListener.Stop();
        }

    }
}
