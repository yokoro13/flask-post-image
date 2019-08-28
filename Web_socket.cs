using UnityEngine;
using WebSocketSharp;
using System.IO;
using System;
using System.Threading;
using System.Collections.Generic;

public class Web_socket : MonoBehaviour
{
    WebSocket ws;
    private SynchronizationContext context;
    private Texture2D texture2D;

    private bool isReceivedMessage;
    byte[] receive;

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
            Debug.Log(BitConverter.ToString(e.RawData));
            receive = e.RawData;
            Debug.Log("re: " + BitConverter.ToString(receive));
            isReceivedMessage = true;
        };

        ws.OnError += (sender, e) => {
            Debug.Log("WebSocket Error Message: " + e.Message);
        };

        ws.OnClose += (sender, e) => {

            Debug.Log("WebSocket Close" + e);
            print(ws);

        };
        ws.Connect();

    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {

            byte[] img = GetTexture2DFromPngFile(Application.dataPath + "./aaa.png").EncodeToJPG();
            Debug.Log(img.Length);
            Debug.Log(BitConverter.ToString(img));
            
            string base64string = Convert.ToBase64String(img);
            print(base64string.Length);

            print(isReceivedMessage);
            if (isReceivedMessage)
            {
                isReceivedMessage = false;
                texture2D.LoadImage(receive);
                receive = texture2D.EncodeToPNG();
                File.WriteAllBytes("./Assets/zzzz.png", receive);
            }
            ws.Send(img);
        }

    }


    void OnDestroy()
    {
        ws.Close();
        ws = null;
    }
}
