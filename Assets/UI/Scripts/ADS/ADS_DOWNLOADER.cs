using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;

public class ADS_DOWNLOADER : MonoBehaviour
{



    [System.Serializable]
    public class ImageObject
    {
        public GameObject GO;
      
        public string ImageID;
    }

    public ImageObject[] _ImageObject;


    void Start()
    {

        for (int i = 0; i < _ImageObject.Length; i++)
        {
            _ImageObject[i].GO.SetActive(false);
        }


        //PlayerPrefs.SetString("imgVer", "1");
        // Debug.Log(Application.persistentDataPath);
        // Debug.Log(Path.GetFileName("http://130.185.73.192/img/1/"));



        string url = "http://130.185.73.192/img/";
        List<string> CurrentImages = new List<string>();
        List<string> NeedeImages = new List<string>();



        //Get all images stored.... >=10

        //string[] array1 = Directory.GetFiles(Application.persistentDataPath + "/");
        // CurrentImages.AddRange(array1);


        DirectoryInfo d = new DirectoryInfo(Application.persistentDataPath + "/");//Assuming Test is your Folder
        FileInfo[] Files = d.GetFiles(); //Getting Text files

        foreach (FileInfo file in Files)
        {
            CurrentImages.Add(file.Name);
        }

        Debug.Log("sd");


        //for (int i = 0; i < CurrentImages.Count;i++ )
        //{

        //    Debug.Log(CurrentImages[i]);
        //}



        //Get all Needed images.... =10
        for (int i = 0; i < _ImageObject.Length; i++)
        {
            NeedeImages.Add(CheckFilename(_ImageObject[i].ImageID));
        }


        for (int i = 0; i < CurrentImages.Count; i++)
        {
            //Debug.Log("&& " + CurrentImages[i]);
        }

        Debug.Log("sdd");


        for (int i = 0; i < _ImageObject.Length; i++)
        {

            if (NeedeImages[i] == "[To Parent Directory]")
            {
                PlayerPrefs.SetString("OBJECT" + i.ToString(), "noimage"); //Set Object property to use later
            }
            else
            {

                PlayerPrefs.SetString("OBJECT" + i.ToString(), NeedeImages[i]); //Set Object property to use later

                if (!CurrentImages.Contains(NeedeImages[i])) //Download needeImages if not exists
                {
                    CurrentImages.Add(NeedeImages[i]);
                    DownloadImage(url + _ImageObject[i].ImageID + "/" + NeedeImages[i], i, true);

                }
                else
                {
                    // Load_Texture(i);
                    DownloadImage(url + _ImageObject[i].ImageID + "/" + NeedeImages[i], i, true);
                }
            }

        }
    }


   

    //http://dreamrain.co/header.jpg


    public void DownloadImage(string url, int ObjIndex, bool Download)
    {
        Debug.Log("download url=" + url);
        StartCoroutine(coDownloadImage(url, ObjIndex, Download));
       
    }

    IEnumerator coDownloadImage(string imageUrl, int ObjIndex, bool Download)
    {


        if (Download)
        {
        
            WWW www = new WWW(imageUrl);
            //yield return www;

            while (!www.isDone)
            {
                // Debug.Log("downloaded " + (www.progress * 100).ToString() + "%...");
                yield return null;
            }


            Uri u = new Uri(imageUrl);
            //
            //Debug.Log("Start Download : " + imageUrl);

            //Debug.Log("Start Download :" + Application.persistentDataPath + "/" + System.IO.Path.GetFileName(u.AbsolutePath));


            string fullPath = Application.persistentDataPath + "/" + System.IO.Path.GetFileName(u.AbsolutePath);// "/TestImg.png";
            File.WriteAllBytes(fullPath, www.bytes);

            //Debug.Log("downloaded..." + imageUrl);

        }



        Load_Texture(ObjIndex);




    }



    void Load_Texture(int Index)
    {

        Debug.Log(PlayerPrefs.GetString("OBJECT" + Index.ToString()));


        if (PlayerPrefs.GetString("OBJECT" + Index.ToString()) == "[To Parent Directory]")
        {
            
            //_ImageObject[Index].GO.SetActive(false);
        }
        else
        {
            _ImageObject[Index].GO.SetActive(true);
            //Debug.Log("NEED : "  + Application.persistentDataPath + "/" + PlayerPrefs.GetString("OBJECT" + Index.ToString()));
            Texture2D tex = null;
            byte[] fileData;

            // if (File.Exists(Application.persistentDataPath + "/TestImg.png"))
            //{


            fileData = File.ReadAllBytes(Application.persistentDataPath + "/" + PlayerPrefs.GetString("OBJECT" + Index.ToString()));
            //Debug.Log("****   :" + fileData);

            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.

            //}
            //  _ImageObject[Index].GO.transform.renderer.material.mainTexture = tex;

            // Material Mt = new Material(Shader.Find("Mobile/Unlit (Supports Lightmap)"));
            //  Mt.mainTexture = tex;
            _ImageObject[Index].GO.transform.renderer.material.mainTexture = tex;


        }

        PlayerPrefs.SetString("OBJECT" + Index.ToString(), ""); //Clear Object property

    }


    public static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }
    

    string  CheckFilename(string ImgID)
    {
        string output = "[To Parent Directory]";
        string uri = "http://130.185.73.192/img/" + ImgID + "/";

        Debug.Log("uri=" + uri);

        WebRequest request = WebRequest.Create(uri);
        WebResponse response = request.GetResponse();
        Regex regex = new Regex("<A HREF=\".*\">(?<name>.*)</A>");
       // Debug.Log(regex);

        using (var reader = new StreamReader(response.GetResponseStream()))
        {
            string result = reader.ReadToEnd();

            MatchCollection matches = regex.Matches(result);
            if (matches.Count == 0)
            {
                Debug.Log("parse failed.");
                //return;
            }

            foreach (Match match in matches)
            {
                if (!match.Success) { continue; }
               // Debug.Log(match.Groups["name"].ToString());
                output = match.Groups["name"].ToString();

            }
        }
        Debug.Log(ImgID + " : " + output);
        return output;    

    }



   

 
}



//using UnityEngine;
//using System.Collections;
//using System.IO;
//using System.Net;
//using System.Text.RegularExpressions;
//using System.Collections.Generic;
//using System;

//public class ADS_DOWNLOADER : MonoBehaviour
//{



//    [System.Serializable]
//    public class ImageObject
//    {
//        public GameObject GO;
      
//        public string ImageID;
//    }

//    public ImageObject[] _ImageObject;
    

//        void Start()
//    {

//        //PlayerPrefs.SetString("imgVer", "1");
//       // Debug.Log(Application.persistentDataPath);


//       // Debug.Log(Path.GetFileName("http://130.185.73.192/img/1/"));

        

//            string url = "http://130.185.73.192/img/";
//        List<string> CurrentImages = new List<string>();
//        List<string> NeedeImages = new List<string>();



//        //Get all images stored.... >=10
            
//        //string[] array1 = Directory.GetFiles(Application.persistentDataPath + "/");
//       // CurrentImages.AddRange(array1);


//        DirectoryInfo d = new DirectoryInfo(Application.persistentDataPath + "/");//Assuming Test is your Folder
//        FileInfo[] Files = d.GetFiles(); //Getting Text files
     
//        foreach (FileInfo file in Files)
//        {
//            CurrentImages.Add(file.Name);
           
//        }



//        //for (int i = 0; i < CurrentImages.Count;i++ )
//        //{

//        //    Debug.Log(CurrentImages[i]);
//        //}



//            //Get all Needed images.... =10
//            for (int i = 0; i < _ImageObject.Length; i++)
//            {

//                NeedeImages.Add(CheckFilename(_ImageObject[i].ImageID));

//            }


//            for (int i = 0; i < CurrentImages.Count;i++ )
//            {
//                Debug.Log("&& " + CurrentImages[i]);
//            }


//                for (int i = 0; i < _ImageObject.Length; i++)
//                {
//                    PlayerPrefs.SetString("OBJECT" + i.ToString(), NeedeImages[i]); //Set Object property to use later

//                    if (!CurrentImages.Contains(NeedeImages[i]) ) //Download needeImages if not exists
//                    {
//                        CurrentImages.Add(NeedeImages[i]);
//                        DownloadImage(url + _ImageObject[i].ImageID + "/" + NeedeImages[i], i, true);
                       
//                    }
//                    else
//                    {
//                        // Load_Texture(i);
//                        DownloadImage(url + _ImageObject[i].ImageID + "/" + NeedeImages[i], i, true);
//                    }


//                }


         

//    }


   

//    //http://dreamrain.co/header.jpg


//    public void DownloadImage(string url, int ObjIndex, bool Download)
//    {

//        StartCoroutine(coDownloadImage(url, ObjIndex, Download));
       
//    }

//    IEnumerator coDownloadImage(string imageUrl, int ObjIndex, bool Download)
//    {


//        if (Download)
//        {
        
//            WWW www = new WWW(imageUrl);
//            //yield return www;

//            while (!www.isDone)
//            {
//                // Debug.Log("downloaded " + (www.progress * 100).ToString() + "%...");
//                yield return null;
//            }


//            Uri u = new Uri(imageUrl);
//            //
//            Debug.Log("Start Download : " + imageUrl);

//            //Debug.Log("Start Download :" + Application.persistentDataPath + "/" + System.IO.Path.GetFileName(u.AbsolutePath));


//            string fullPath = Application.persistentDataPath + "/" + System.IO.Path.GetFileName(u.AbsolutePath);// "/TestImg.png";
//            File.WriteAllBytes(fullPath, www.bytes);

//            Debug.Log("downloaded..." + imageUrl);

//        }



//        Load_Texture(ObjIndex);




//    }



//    void Load_Texture(int Index)
//    {
//        Debug.Log("NEED : "  + Application.persistentDataPath + "/" + PlayerPrefs.GetString("OBJECT" + Index.ToString()));
//        Texture2D tex = null;
//        byte[] fileData;

//        // if (File.Exists(Application.persistentDataPath + "/TestImg.png"))
//        //{

        
//        fileData = File.ReadAllBytes(Application.persistentDataPath + "/" + PlayerPrefs.GetString("OBJECT" + Index.ToString()));
//        //Debug.Log("****   :" + fileData);
        
//        tex = new Texture2D(2, 2);
//        tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.

//        //}
//        //  _ImageObject[Index].GO.transform.renderer.material.mainTexture = tex;

//       // Material Mt = new Material(Shader.Find("Mobile/Unlit (Supports Lightmap)"));
//      //  Mt.mainTexture = tex;
//        _ImageObject[Index].GO.transform.renderer.material.mainTexture = tex;




//        PlayerPrefs.SetString("OBJECT" + Index.ToString(), ""); //Clear Object property

//    }


//    public static string Base64Encode(string plainText)
//    {
//        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
//        return System.Convert.ToBase64String(plainTextBytes);
//    }
    

//    string  CheckFilename(string ImgID)
//    {

//        string output = "";
//        string uri = "http://130.185.73.192/img/" + ImgID + "/";
//        WebRequest request = WebRequest.Create(uri);
//        WebResponse response = request.GetResponse();
//        Regex regex = new Regex("<A HREF=\".*\">(?<name>.*)</A>");
//       // Debug.Log(regex);

//        using (var reader = new StreamReader(response.GetResponseStream()))
//        {
//            string result = reader.ReadToEnd();

//            MatchCollection matches = regex.Matches(result);
//            if (matches.Count == 0)
//            {
//                Debug.Log("parse failed.");
//                //return;
//            }

//            foreach (Match match in matches)
//            {
//                if (!match.Success) { continue; }
//               // Debug.Log(match.Groups["name"].ToString());
//                output = match.Groups["name"].ToString();

//            }
//        }

//        return output;    

//    }



   

 
//}

