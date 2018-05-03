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

	private string BASE_URL = "https://docs.google.com/forms/u/1/d/e/1FAIpQLSdyHlsmfREBkKcP1lnA4dH_sYj5_Svrcc_YQ-CQ0a384EKixg/formResponse";
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
		yield return new WaitForEndOfFrame();
		WWWForm form = new WWWForm();
		form.AddField("entry.1583625957", n);
		form.AddField("entry.655267041", h);
		form.AddField("entry.1831294178", o);
		form.AddField("entry.1664739340", s);

       // Create a texture the size of the screen, RGB24 format
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
