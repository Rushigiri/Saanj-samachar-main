using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class NewsApi : MonoBehaviour
{
    public GameObject VerticalGroup;

    public RawImage rawImage;

    private void Start()
    {
        StartCoroutine(Read());
    }
    public Root root;
    IEnumerator Read()
    {
        

        UnityWebRequest url = UnityWebRequest.Get("https://newsapi.org/v2/everything?q=tesla&from=2022-06-18&sortBy=publishedAt&apiKey=9703fb82cefa4109b799ad12658d0bea");
        yield return url.SendWebRequest();

        if (url.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("----try again----");
        }

        else
        {
            string result = url.downloadHandler.text;
            root = JsonUtility.FromJson<Root>(result);
            Debug.Log(root.articles[1].author);

            SetHeadlines();


        }

        url.Dispose();
    }

    [System.Serializable]
    public class Article
    {
        public Source source;
        public string author;
        public string title;
        public string description;
        public string url;
        public string urlToImage;
        //changed below line
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


    public void SetHeadlines()
    {
        for (int i = 0; i < 5   ; i++)
        {
            
                TextMeshProUGUI currentTextBox = VerticalGroup.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>();
                if (currentTextBox != null)
                {
                    currentTextBox.text = root.articles[i].title;
                    int childIndex = i;
                    StartCoroutine(GetRawImage(i, childIndex, VerticalGroup.transform));

                }
                else
                {
                    Debug.Log("try again");
                }
            
           
        }

    }

    IEnumerator GetRawImage(int imageIndex, int childIndex, Transform verticalGroup)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(root.articles[imageIndex].urlToImage);
        yield return request.SendWebRequest();
        verticalGroup.transform.GetChild(childIndex).GetChild(1).GetComponent<RawImage>().texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        request.Dispose();
    }

}
