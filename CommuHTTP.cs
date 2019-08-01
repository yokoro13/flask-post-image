using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class CommuHTTP : MonoBehaviour
{


    // Start is called before the first frame update
    void Start(){
        StartCoroutine(Commu());
        
    }

    IEnumerator Commu()
    {
        byte[] img = File.ReadAllBytes(Application.dataPath + "./3.jpg");
        WWWForm form = new WWWForm();
        form.AddBinaryData("post_data", img, "3.jpg", "image/png");
        UnityWebRequest resuest = UnityWebRequest.Post("http://localhost:5000", form);

        yield return resuest.SendWebRequest();

        Debug.Log(resuest.responseCode);
        File.WriteAllBytes("./Assets/awa.png", resuest.downloadHandler.data);
        Debug.Log(resuest.downloadHandler.text);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
