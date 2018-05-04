using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine;
using System.IO;
using UnityGoogleDrive;



public class HoursSubmit : MonoBehaviour {

	public UnityEngine.UI.InputField Name;
	public UnityEngine.UI.InputField Hours;
	public UnityEngine.UI.InputField Organization;
	public UnityEngine.UI.InputField Signature;
	public GameObject btn;

	private string username;
	private string hours;
	private string organization;
	private string signature;
	private string link;

	public bool imageReady = false;

	public string path;
	public Texture2D image;

	public GameObject S;
	public GameObject F;

	private string BASE_URL = "https://docs.google.com/forms/u/1/d/e/1FAIpQLSdyHlsmfREBkKcP1lnA4dH_sYj5_Svrcc_YQ-CQ0a384EKixg/formResponse";

    private GoogleDriveFiles.CreateRequest request;
	// Use this for initialization
	void Update () {
		username = Name.text;
		hours = Hours.text.ToString();
		organization = Organization.text;
		signature = Signature.text;

		if(name != "" && hours != "" && organization != "" && signature != "" && imageReady == true)
		{
			btn.GetComponent<Button>().interactable = true;
		}
	}
	IEnumerator Post(string n, string h, string o, string s, string p)
	{
        /*
        WWW localFile = new WWW(path);
        yield return localFile;
        if (localFile.error == null)
            Debug.Log("Loaded file successfully");
        else
        {
            Debug.Log("Open file error: " + localFile.error);
            yield break; // stop the coroutine here
        }

        yield return new WaitForEndOfFrame();
        */


		WWWForm form = new WWWForm();
		form.AddField("entry.1583625957", n);
		form.AddField("entry.655267041", h);
		form.AddField("entry.1831294178", o);
		form.AddField("entry.1664739340", s);
		form.AddField("entry.1333156437", p);
		byte[] rawData = form.data;
		WWW www = new WWW(BASE_URL, rawData);
		yield return www;
        if (www.error == null)
            StartCoroutine("Success");
        else
            StartCoroutine("Fail");
        

       /*
        int width = Screen.width;
        int height = Screen.height;
        Texture2D tex = image.texture as Texture2D;

        // Read screen contents into the texture
        tex.ReadPixels( new Rect(0, 0, width, height), 0, 0 );
        tex.Apply();

        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();
        Destroy( tex );

        form.AddBinaryData("entry.1374423980", bytes, "image.png", "image/png");
        */
        //form.AddBinaryData("file", localFile.bytes, "proof", "text/plain");

        /*
        using (var w = UnityWebRequest.Post(p, form))
        {
            yield return w.SendWebRequest();
            if (w.isNetworkError || w.isHttpError) {
                print(w.error);
            }
            else {
                print("Finished Uploading Screenshot");
            }
        }
		
        byte[] rawData = form.data;
        UnityWebRequest www = UnityWebRequest.Put(BASE_URL, rawData);
        yield return www.SendWebRequest();
        //yield return www;
        
        if (www.error == null)
            Debug.Log("upload done :");
        else
            Debug.Log("Error during upload: " + www.error);
            */
            
    }

    IEnumerator Success()
    {
    	S.SetActive(true);
    	yield return new WaitForSeconds(2f);
    	S.SetActive(false);
    }

    IEnumerator Fail()
    {
    	F.SetActive(true);
    	yield return new WaitForSeconds(2f);
    	F.SetActive(false);
    }
	// Update is called once per frame
	public void Send () {
		username = Name.text;
		hours = Hours.text.ToString();
		organization = Organization.text;
		signature = Signature.text;
		Upload();
	}

	public void Open()
	{
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery( ( path ) =>
        {
            Debug.Log( "Image path: " + path );
            if( path != null )
            {
                // Create Texture from selected image
                image = NativeGallery.LoadImageAtPath( path, -1);
                Upload();
                imageReady = true;
                if( image == null )
                {
                    Debug.Log( "Couldn't load texture from " + path );
                    return;
                }
            }
        }, "Select a PNG image", "image/png", -1); 

	}


    void Upload ()
    {
        Texture2D texCopy = new Texture2D(image.width, image.height, image.format, image.mipmapCount > 1);
        texCopy.LoadRawTextureData(image.GetRawTextureData());
        texCopy.Apply();

        var content = texCopy.EncodeToPNG();
        var file = new UnityGoogleDrive.Data.File() { Name = "proof.png", Content = content, MimeType = "image/png" };
        request = GoogleDriveFiles.Create(file);
        request.Fields = new List<string> { "id", "name", "size", "createdTime"};
        request.Send().OnDone += PrintResult;
    }

    private void PrintResult (UnityGoogleDrive.Data.File file)
    {
        //result = string.Format("Name: {0} Size: {1:0.00}MB Created: {2:dd.MM.yyyy HH:MM:ss}", file.Name, file.Size * .000001f, file.CreatedTime);
        link = "drive.google.com/open?id=" + file.Id;
		StartCoroutine(Post(username, hours, organization, signature, link));
    }
}
