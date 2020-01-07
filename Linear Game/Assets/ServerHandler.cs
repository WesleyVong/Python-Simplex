using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class ServerHandler : MonoBehaviour
{
    public const string SendURL = "http://192.168.1.34:8000/upload/upload";
    public const string GetURL = "http://192.168.1.34:8000";
    public Matrix resultMat = new Matrix();
    public bool uploadFinished = false;
    public bool error = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Compute(float[][] data)
    {
        uploadFinished = false;
        Matrix mat = new Matrix();
        mat.data = data;
        //Debug.Log(mat.toJSON());
        StartCoroutine(Upload(SendURL, mat.toJSON()));
    }
    
    IEnumerator Upload(string uri, string data)
    {
        using (UnityWebRequest www = UnityWebRequest.Put(uri, data))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                uploadFinished = true;
                error = true;
            }
            else
            {
                string result = www.downloadHandler.text;
                //Debug.Log(result);
                resultMat.fromJSON(result);
                uploadFinished = true;
                error = false;
            }
        }
    }

    // Not needed right now
    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
            }
        }
    }
}

public class Matrix
{
    public float[][] data;
    public string toJSON()
    {
        string[] jsonString = new string[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            string[] rowString = new string[data[i].Length];
            for (int j = 0; j < data[i].Length; j++)
            {
                rowString[j] = data[i][j].ToString();
            }
            jsonString[i] = "[" + string.Join(",", rowString) + "]";
        }
        return ("[" + string.Join(",",jsonString) + "]");
    }
    public void fromJSON(string JSON)
    {
        string tmp = JSON.Substring(1, JSON.Length - 2);
        string[] tmpData = tmp.Split(',');
        float[] tmpFloat = new float[tmpData.Length];
        data = new float[1][];
        for (int i = 0; i < tmpData.Length; i++)
        {
            tmpFloat[i] = float.Parse(tmpData[i]);
        }
        data[0] = tmpFloat;
    }
}
