using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Net;
using System.IO;
using System;
using System.Text;

public class Web_socket : MonoBehaviour
{

    WebSocket ws;
    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            

    void Start()
    {
        ws = new WebSocket("ws://127.0.0.1:5000/");
        


        ws.OnOpen += (sender, e) => {
            Debug.Log("WebSocket Open");
        };
        ws.OnMessage += (sender, e) => {
            Debug.Log(e.Data);
            byte[] receive = Convert.FromBase64String(e.Data);

            File.WriteAllBytes("./Assets/soc.png", receive);
            print("aaaa");
            sw.Stop();
            Debug.Log($"{sw.ElapsedMilliseconds} sec");
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

        if (Input.GetKeyUp(KeyCode.Space)) {
            string send_message = "SpaceKey_pressed";
            byte[] img = File.ReadAllBytes(Application.dataPath + "./3.jpg");
            string base64string = Convert.ToBase64String(img);

            Debug.Log(base64string.Length);

            sw.Start();
            Debug.Log("WebSocket Send Message Data: " + send_message);
            ws.Send(base64string);

        }

    }

    void OnDestroy()
    {
        ws.Close();
        ws = null;
    }
}
