using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Net;
using System.IO;
using System;
using System.Text;
using System.Threading;

public class Web_socket : MonoBehaviour
{

    WebSocket ws;
    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
    byte[] receive;
    private SynchronizationContext context;
    Texture2D texture2D;

    private bool isReceivedMessage;

    public static Texture2D GetTexture2DFromPngFile(string path)
    {
        byte[] img = File.ReadAllBytes(path);

        int width = 1024;
        int height = 512;

        Texture2D texture = new Texture2D(width, height);
        texture.LoadImage(img);
        TextureScale.Point(texture, width / 2, height / 2);
        return texture;
    }

    void Start()
    {
        texture2D = new Texture2D(256, 128);
        isReceivedMessage = false;

        ws = new WebSocket("ws://127.0.0.1:5000/");

        ws.OnOpen += (sender, e) => {
            Debug.Log("WebSocket Open");
        };

        ws.OnMessage += (sender, e) => {
            Debug.Log(e.Data);
            receive = Convert.FromBase64String(e.Data);
            isReceivedMessage = true;
        };

        ws.OnError += (sender, e) => {
            Debug.Log("WebSocket Error Message: " + e.Message);
        };

        ws.OnClose += (sender, e) => {

            Debug.Log("WebSocket Close" + e);
        };
        ws.Connect();

    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            string send_message = "SpaceKey_pressed";
            byte[] img = GetTexture2DFromPngFile(Application.dataPath + "./aaa.png").EncodeToJPG();
            string base64string = Convert.ToBase64String(img);

            Debug.Log(base64string.Length);

            sw.Start();
            Debug.Log("WebSocket Send Message Data: " + send_message);

            if (isReceivedMessage)
            {
                isReceivedMessage = false;
                texture2D.LoadImage(receive);
                receive = texture2D.EncodeToPNG();
                File.WriteAllBytes("./Assets/rec2.png", receive);
            }
            ws.Send(base64string);
        }

    }

    void OnDestroy()
    {
        ws.Close();
        ws = null;
    }
}
