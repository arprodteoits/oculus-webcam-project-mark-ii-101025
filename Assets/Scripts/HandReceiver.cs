using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json.Linq;

public class HandReceiver : MonoBehaviour
{
    UdpClient udp;
    Thread receiveThread;

    public GameObject arObjectPrefab;   // Prefab untuk AR Object (misal Sphere)
    public Camera arCamera;             // Kamera untuk konversi posisi
    private GameObject spawnedObject;   // Object yang ditampilkan

    Vector3 latestHandPos = Vector3.zero;
    bool handDetected = false;

    void Start()
    {
        udp = new UdpClient(5052);
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    void Update()
    {
        if (handDetected && spawnedObject == null)
        {
            spawnedObject = Instantiate(arObjectPrefab, latestHandPos, Quaternion.identity);
        }
        else if (handDetected && spawnedObject != null)
        {
            spawnedObject.transform.position = latestHandPos;
        }
    }

    void ReceiveData()
    {
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
        while (true)
        {
            try
            {
                byte[] data = udp.Receive(ref remoteEndPoint);
                string json = Encoding.UTF8.GetString(data);
                JObject obj = JObject.Parse(json);

                float x = (float)obj["x"];
                float y = (float)obj["y"];
                float z = (float)obj["z"];
                bool detected = (bool)obj["detected"];

                // konversi posisi layar ke world space
                Vector3 screenPos = new Vector3(x, y, z);
                latestHandPos = arCamera.ScreenToWorldPoint(screenPos);
                handDetected = detected;
            }
            catch (System.Exception e)
            {
                Debug.Log("Receive error: " + e.Message);
            }
        }
    }

    void OnApplicationQuit()
    {
        if (receiveThread != null) receiveThread.Abort();
        if (udp != null) udp.Close();
    }
}
