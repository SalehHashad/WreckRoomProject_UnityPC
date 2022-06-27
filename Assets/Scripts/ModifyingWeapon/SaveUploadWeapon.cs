using Steamworks;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SaveUploadWeapon : MonoBehaviour
{



    public GameObject Weapon;
    public Transform WeaponTransformLoaderHolder;

    public Text TitleTextField;
    public Text Describtion;

    public RenderTexture rendertexture;

    public TMP_Text UploadFeedBack_text; 

    string fileName;

    private SteamAPICall_t createItemCall;
    private CallResult<CreateItemResult_t> createCallRes;
    private UGCUpdateHandle_t updateHandle;
    string appDataPath;
    string previewPath;

    // screenshoot values
    private string directoryName = "Screenshots";

    string weaponName;

    private void Start()
    {
        weaponName = Weapon.name;
    }

    public void SaveWeaponModification()
    {
        TakeScreenShoot(rendertexture);

        CubPart[] cubeArray = FindObjectsOfType<CubPart>();

        fileName = "WeaponModifiedFile" + (cubeArray.Length-2 )+ ".es3";
        //TitleTextField.text = "Crow Bar  " + (cubeArray.Length - 2);

        ES3.Save(weaponName, Weapon, fileName);

        UploadLevelToSteamWorkshop();
    }

    public void LoadModifiedWeapon(List<string> fileNames)
    {
        foreach (var item in fileNames)
        {
            GameObject loadedObject = ES3.Load(weaponName, item) as GameObject;
            //GameObject a = GameObject.Find(name);
            loadedObject.transform.parent = WeaponTransformLoaderHolder;
            loadedObject.transform.localPosition = Vector3.zero;
            loadedObject.transform.localEulerAngles = Vector3.zero;
        }

        

    }

    private void UploadLevelToSteamWorkshop()
    {
        appDataPath = ES3Settings.defaultSettings.FullPath;
        previewPath = Application.persistentDataPath + "/Images/01.jpg";
        appDataPath = appDataPath.Replace("SaveFile.es3", fileName);

        if (SteamManager.Initialized)
        {
            createCallRes = CallResult<CreateItemResult_t>.Create(OnCreateItem);
            SteamAPICall_t handle = SteamUGC.CreateItem(new AppId_t(1263930), EWorkshopFileType.k_EWorkshopFileTypeCommunity);
            createCallRes.Set(handle);
        }

        UploadFeedBack_text.text = "Weapon Uploaded Successfuly...";


    }

    void OnCreateItem(CreateItemResult_t pCallback, bool bIOFailure)
    {
        if (!pCallback.m_bUserNeedsToAcceptWorkshopLegalAgreement)
        {
            updateHandle = SteamUGC.StartItemUpdate(new AppId_t(1263930), pCallback.m_nPublishedFileId); //may need to do a create and onFunction to set the handle of UGCUpdateHandle_t
        SteamUGC.SetItemTitle(updateHandle, TitleTextField.text);
        SteamUGC.SetItemDescription(updateHandle, Describtion.text);
        SteamUGC.SetItemContent(updateHandle, appDataPath);

        SteamUGC.SetItemPreview(updateHandle, previewPath);

            SteamUGC.SetItemVisibility(updateHandle, 0); //k_ERemoteStoragePublishedFileVisibilityPublic = 0, so it should be set to public with this line

            SteamUGC.SubmitItemUpdate(updateHandle, "New workshop item");
            SteamFriends.ActivateGameOverlayToWebPage("steam://url/CommunityFilePage/" + pCallback.m_nPublishedFileId);

        }
        else
        {
            redirectToLegal();
        }
    }

    public void redirectToLegal()
    {
        SteamFriends.ActivateGameOverlayToWebPage("https://steamcommunity.com/sharedfiles/workshoplegalagreement");
    }


    public void TakeScreenShoot(RenderTexture renderTexture)
    {
        SaveRenderTexture(renderTexture, Path.Combine(Application.persistentDataPath, "Images/01.jpg"));
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

    //public void TakeScreenshot()
    //{
    //    ScreenCapture.CaptureScreenshot("TestImage.png");
    //    DirectoryInfo screenshotDirectory = Directory.CreateDirectory("/ Images");
    //    string fullPath = Path.Combine(screenshotDirectory.FullName, "01.jpg");

    //    ScreenCapture.CaptureScreenshot(fullPath);
    //}


}
