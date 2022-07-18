using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
public class ShowDetails : MonoBehaviour
{
    public GameObject discriptionObj;
    int discriptionN;
    public RawImage rawImage;

    [System.Serializable]
    public class Article
    {
        public Source source;
        public string author;
        public string title;
        public string description;
        public string url;
        public string urlToImage;
        public string publishedAt;
        public string content;
    }

    [System.Serializable]
    public class Root
    {
        public string status;
        public int totalResults;
        public List<Article> articles;
    }

    [System.Serializable]
    public class Source
    {
        public string id;
        public string name;
    }

    //private void Start()
    //{
    //}

    public void SetValue(int value)
    {
        discriptionN = value;
        Debug.Log(value);
        StartCoroutine(GetRawImage(discriptionN, discriptionObj.transform));
        StartCoroutine(Read(discriptionN));
    }
    public Root _root;
    IEnumerator Read(int Number)         //get api data
    {

        UnityWebRequest url = UnityWebRequest.Get("https://newsapi.org/v2/everything?q=tesla&from=2022-06-18&sortBy=publishedAt&apiKey=9703fb82cefa4109b799ad12658d0bea");
        yield return url.SendWebRequest();

        if (url.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("--failed--");
        }
        else
        {
            string result = url.downloadHandler.text;
            _root = JsonUtility.FromJson<Root>(result);
            Debug.Log(_root.articles[1].author);

            SetGroup1Headlines(Number);
        }
        url.Dispose();
    }

    IEnumerator GetRawImage(int imageIndex, Transform verticalGroup)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(_root.articles[imageIndex].urlToImage);
        yield return request.SendWebRequest();
        verticalGroup.transform.GetChild(1).GetComponent<RawImage>().texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        request.Dispose();
    }

    public void SetGroup1Headlines(int TextNumber)
    {

        TextMeshProUGUI currentTextBox = discriptionObj.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        if (currentTextBox != null)
        {
            currentTextBox.text = _root.articles[TextNumber].description;
            //int childIndex = i;
            StartCoroutine(GetRawImage(discriptionN, discriptionObj.transform));

        }
        else
        {
            Debug.Log("can't reached");
        }
        //if (currentTextBox != 0)
        //{
        //    currentTextBox.text = root.articles[TextNumber].description;
        //    //int childIndex = 1;
        //    StartCoroutine(GetRawImage(discriptionN, discriptionObj.transform));

        //}
        //else
        //{
        //    Debug.Log("can't reached");
        //}


    }


    

}
