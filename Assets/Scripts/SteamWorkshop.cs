using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using UnityEngine.UI;
using System.IO;

//Mainly using https://partner.steamgames.com/doc/features/workshop/implementation#creating_a_workshop_item as a guide and https://partner.steamgames.com/doc/api/ISteamUGC for details

public class SteamWorkshop : MonoBehaviour
{
    public InputField title;
    public InputField description;
    private SteamAPICall_t createItemCall;
    private CallResult<CreateItemResult_t> createCallRes;
    private UGCUpdateHandle_t updateHandle;
    private string path;
    public AudioSource click;
    public Animator animationBoi;
    public AudioSource negativeTone;
    public string previewImagePath;
    public Button findImageButton;
    private bool klikd;
    private bool properImageSelected;
    
    protected string m_textPath;

    //protected FileBrowser m_fileBrowser;

    [SerializeField]
    protected Texture2D m_directoryImage,
                        m_fileImage;

    private void Start()
    {
        //klikd = false;
        //findImageButton.onClick.AddListener(TaskOnClick);
        //properImageSelected = true;
    }

    public void TaskOnClick() {
        klikd = true;
        click.Play();
    }

    //protected void OnGUI()
    //{
    //    if (m_fileBrowser != null)
    //    {
    //        m_fileBrowser.OnGUI();
    //    }
    //    else
    //    {
    //        OnGUIMain();
    //    }
    //}

    //protected void OnGUIMain()
    //{
    //    GUILayout.BeginHorizontal();
    //    GUILayout.FlexibleSpace();     

    //    if(klikd)
    //    {
    //        GUILayout.ExpandWidth(false);
    //        m_fileBrowser = new FileBrowser(
    //            new Rect(0, 100, Screen.width /3, Screen.height - 150),
    //            "Choose .JPG File",
    //            FileSelectedCallback
    //        );
    //        m_fileBrowser.SelectionPattern = "*.jpg"; //They must select a JPG
    //        m_fileBrowser.DirectoryImage = m_directoryImage;
    //        m_fileBrowser.FileImage = m_fileImage;
    //    }
    //    klikd = false;
    //    GUILayout.EndHorizontal();
    //}

    protected void FileSelectedCallback(string path)
    {
        //m_fileBrowser = null;
        m_textPath = path;
        var fileInfo = new System.IO.FileInfo(path);
        if (fileInfo.Length > 1000000) {           
            findImageButton.GetComponentInChildren<Text>().text = "OVER 1MB\nCHOOSE DIFFERENT FILE";
            properImageSelected = false;
            negativeTone.Play();
        }
        else {
            findImageButton.GetComponentInChildren<Text>().text = path;
            properImageSelected = true;
        }      
    }
    
    // Start is called before the first frame update
    public void SubmitToWorkshop()
    {
        //path = Application.persistentDataPath + "/" + title.text;

        //if (File.Exists(path) && properImageSelected)
        //{
        //    if (SteamManager.Initialized)
        //    {
        //        click.Play();
        //        //1. All workshop items begin their existence with a call to ISteamUGC::CreateItem
        //        createCallRes = CallResult<CreateItemResult_t>.Create(OnCreateItem);

        //        //2. Next Register a call result handler for CreateItemResult_t
        //        SteamAPICall_t handle = SteamUGC.CreateItem(new AppId_t(1313640), EWorkshopFileType.k_EWorkshopFileTypeCommunity);
        //        createCallRes.Set(handle);
        //    }
        //}
        //else {
        //    animationBoi.SetTrigger("fnf");
        //    negativeTone.Play();
        //}

        if (SteamManager.Initialized)
        {
            createCallRes = CallResult<CreateItemResult_t>.Create(OnCreateItem);
            SteamAPICall_t handle = SteamUGC.CreateItem(new AppId_t(1263930), EWorkshopFileType.k_EWorkshopFileTypeCommunity);
            createCallRes.Set(handle);
        }

    }

    void OnCreateItem(CreateItemResult_t pCallback, bool bIOFailure)
    {
        //3. First check the m_eResult to ensure that the item was created successfully.
        //4. When the call result handler is executed, read the m_nPublishedFileId value and store for future updates to the workshop item (e.g. in a project file associated with the creation tool).
        //Debug.Log("m_eResult: " + pCallback.m_eResult + "   Published File ID: " + pCallback.m_nPublishedFileId + "   User needs to accept legal agreement: " + pCallback.m_bUserNeedsToAcceptWorkshopLegalAgreement);
        //5. The m_bUserNeedsToAcceptWorkshopLegalAgreement variable should also be checked and if it's true, the user should be redirected to accept the legal agreement. See the Workshop Legal Agreement section for more details.
        //https://partner.steamgames.com/doc/features/workshop/implementation#Legal

        //if (!pCallback.m_bUserNeedsToAcceptWorkshopLegalAgreement)
        //{          
        //    //Once a workshop item has been created and a PublishedFileId_t value has been returned, the content of the workshop item can be populated and uploaded to the Steam Workshop.
        //    //An item update begins with a call to SteamUGC.StartItemUpdate

        //    updateHandle = SteamUGC.StartItemUpdate(new AppId_t(1263930), pCallback.m_nPublishedFileId); //may need to do a create and onFunction to set the handle of UGCUpdateHandle_t
        //    SteamUGC.SetItemTitle(updateHandle, title.text);
        //    SteamUGC.SetItemDescription(updateHandle, description.text);
        //    SteamUGC.SetItemContent(updateHandle, path);

        //    string newImagePath = "";
        //    if (m_textPath != null)
        //    {
        //        newImagePath = m_textPath.Replace("\\", "/");
        //    }
        //    if (File.Exists(newImagePath)) {                        
        //        SteamUGC.SetItemPreview(updateHandle, newImagePath);
        //        //print("Setting " + newImagePath + " as preview image");
        //    }

        //    SteamUGC.SetItemVisibility(updateHandle, 0); //k_ERemoteStoragePublishedFileVisibilityPublic = 0, so it should be set to public with this line

        //    //Once the update calls have been completed, calling ISteamUGC::SubmitItemUpdate will initiate the upload process to the Steam Workshop.
        //    SteamUGC.SubmitItemUpdate(updateHandle, "New workshop item");
        //    SteamFriends.ActivateGameOverlayToWebPage("steam://url/CommunityFilePage/" + pCallback.m_nPublishedFileId);

        //    animationBoi.SetTrigger("submittedToWorkshop");
        //}
        //else {
        //    redirectToLegal();
        //}

        if (!pCallback.m_bUserNeedsToAcceptWorkshopLegalAgreement)
        {
            //Once a workshop item has been created and a PublishedFileId_t value has been returned, the content of the workshop item can be populated and uploaded to the Steam Workshop.
            //An item update begins with a call to SteamUGC.StartItemUpdate

            updateHandle = SteamUGC.StartItemUpdate(new AppId_t(1263930), pCallback.m_nPublishedFileId); //may need to do a create and onFunction to set the handle of UGCUpdateHandle_t
            SteamUGC.SetItemTitle(updateHandle, "Andrew Adding Mely Weapon");
            SteamUGC.SetItemDescription(updateHandle, "Adrew Adding Mely Weapon Description");

            string path1 = Application.persistentDataPath + "/Images/01.jpg";
            SteamUGC.SetItemPreview(updateHandle, path1);

            SteamUGC.SetItemVisibility(updateHandle, 0); //k_ERemoteStoragePublishedFileVisibilityPublic = 0, so it should be set to public with this line

            SteamUGC.SubmitItemUpdate(updateHandle, "New workshop item");
            SteamFriends.ActivateGameOverlayToWebPage("steam://url/CommunityFilePage/" + pCallback.m_nPublishedFileId);

        }
        else
        {
            redirectToLegal();
        }
    }

    public void redirectToLegal() {
        SteamFriends.ActivateGameOverlayToWebPage("https://steamcommunity.com/sharedfiles/workshoplegalagreement");
    }
}
