using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine;
using UnityEditor;


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

	public bool imageReady = false;

	public string path;
	public RawImage image;

	private string BASE_URL = "https://volunteerhourslghs.formstack.com/forms/index.php";
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
		form.AddField("field64110253", n);
		form.AddField("field64110251", h);
		form.AddField("field64110250", o);
		form.AddField("field64110249", s);

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
		*/
        byte[] rawData = form.data;
		WWW www  = new WWW(BASE_URL, rawData);
		yield return www;
        /*
        if (www.error == null)
            Debug.Log("upload done :" + www.text);
        else
            Debug.Log("Error during upload: " + www.error);
            */
    }
	// Update is called once per frame
	public void Send () {
		username = Name.text;
		hours = Hours.text.ToString();
		organization = Organization.text;
		signature = Signature.text;

		StartCoroutine(Post(username, hours, organization, signature, path));
	}

	public void Open()
	{
		path = "file:///" + EditorUtility.OpenFilePanel("Overwrite with png", "", "png");
		GetImage();
		imageReady = true;
	}

	void GetImage()
	{
		if(path != null)
		{
			UpdateImage();
		}
	}

	void UpdateImage()
	{
		WWW wwwi = new WWW(path);
		image.texture = wwwi.texture;
	}
}
