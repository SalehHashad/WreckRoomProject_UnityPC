using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SaveCameraViewFromRenderTexture : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		
	}

    // Update is called once per frame
    void Update()
    {
        
    }
	public void TakeScreenShoot(RenderTexture renderTexture)
    {
		SaveRenderTexture(renderTexture,Path.Combine(Application.persistentDataPath,"Images/01.jpg"));
	}

    static void SaveRenderTexture(RenderTexture rt, string path)
{
	RenderTexture.active = rt;
	Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
	tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
	RenderTexture.active = null;
	var bytes = tex.EncodeToPNG();
	System.IO.File.WriteAllBytes(path, bytes);
	AssetDatabase.ImportAsset(path);
	Debug.Log($"Saved texture: {rt.width}x{rt.height} - " + path);
}
[MenuItem("Assets/Take Screenshot", true)]
public static bool TakeScreenshotValidation() =>
	Selection.activeGameObject && Selection.activeGameObject.GetComponent<Camera>();
[MenuItem("Assets/Take Screenshot")]
public static void TakeScreenshot()
{
	var camera = Selection.activeGameObject.GetComponent<Camera>();
	var prev = camera.targetTexture;
	var rt = new RenderTexture(Screen.width, Screen.height, 16);
	camera.targetTexture = rt;
	camera.Render();
	SaveRenderTexture(rt, "Assets/screenshot.png");
	camera.targetTexture = prev;
	Object.DestroyImmediate(rt);
}
}
