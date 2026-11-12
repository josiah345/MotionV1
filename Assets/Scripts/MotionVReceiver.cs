using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System.Collections.Concurrent;

public class MotionVSerialReceiver : MonoBehaviour
{
    public event System.Action<int> OnFootState; // 1 or 0

    SerialPort serial;
    Thread readThread;
    bool running = false;
    ConcurrentQueue<string> messages = new ConcurrentQueue<string>();

    void Start()
    {
        serial = new SerialPort("COM5", 115200);
        serial.ReadTimeout = 100;

        try
        {
            serial.Open();
            running = true;

            readThread = new Thread(ReadData);
            readThread.IsBackground = true;
            readThread.Start();

            Debug.Log("MOTIONV: ESP32 connected on COM5");
        }
        catch (System.Exception e)
        {
            Debug.LogError("SERIAL ERROR: " + e.Message);
        }
    }

    void ReadData()
    {
        while (running)
        {
            try
            {
                messages.Enqueue(serial.ReadLine().Trim());
            }
            catch (System.TimeoutException) { }
            catch { }
        }
    }

    void Update()
    {
        while (messages.TryDequeue(out string msg))
        {
            Debug.Log("MOTIONV RECEIVED: " + msg);

            if (msg == "MOTIONV,FOOT,1") OnFootState?.Invoke(1);
            else if (msg == "MOTIONV,FOOT,0") OnFootState?.Invoke(0);
        }
    }

    void OnDestroy()
    {
        running = false;

        if (serial != null && serial.IsOpen)
            serial.Close();

        if (readThread != null && readThread.IsAlive)
            readThread.Join(200);
    }
}