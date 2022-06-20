using Steamworks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveUploadWeapon : MonoBehaviour
{



    public GameObject Weapon;
    public Transform WeaponTransform;

    public TMP_Text TitleTextField;
    public Text Describtion;


    string name;
    string fileName;

    private SteamAPICall_t createItemCall;
    private CallResult<CreateItemResult_t> createCallRes;
    private UGCUpdateHandle_t updateHandle;
    string appDataPath;
    string previewPath;

    private void Start()
    {
        name = Weapon.name;
    }

    public void SaveWeaponModification()
    {
        CubPart[] cubeArray = FindObjectsOfType<CubPart>();

        fileName = "WeaponModifiedFile" + (cubeArray.Length-2 )+ ".es3";
        TitleTextField.text = "Crow Bar  " + (cubeArray.Length - 2);

        ES3.Save(name, Weapon, fileName);

        UploadLevelToSteamWorkshop();
    }

    public void LoadModifiedWeapon(string fileName)
    {
        ES3.Load(name, fileName);
        GameObject a = GameObject.Find(name);
        a.transform.parent = WeaponTransform;
        a.transform.localPosition = Vector3.zero;
        a.transform.localEulerAngles = Vector3.zero;

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

       
    }

    void OnCreateItem(CreateItemResult_t pCallback, bool bIOFailure)
    {
        if (!pCallback.m_bUserNeedsToAcceptWorkshopLegalAgreement)
        {
            updateHandle = SteamUGC.StartItemUpdate(new AppId_t(1263930), pCallback.m_nPublishedFileId); //may need to do a create and onFunction to set the handle of UGCUpdateHandle_t
        SteamUGC.SetItemTitle(updateHandle, "Andrew Testing Uploading Weapon");
        SteamUGC.SetItemDescription(updateHandle, "Andrew Testing Uploading Weapon Description");
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





}
