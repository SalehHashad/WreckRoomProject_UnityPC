using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using System.IO;
using UnityEngine.UI;

//The name download is disengenuous as this has more to do with relocating installed files
public class SteamWorkshopDownload : MonoBehaviour
{
    bool installedStuff;
    uint folderSize;
    public ScrollRect scrollBoi;
    public Text scrollBoiText;
    private string listOfCustomLevels;
    private string path;

    PublishedFileId_t[] PublishedFileID;
    void Start()
    {
        if (SteamManager.Initialized)
        {
            //Debug.Log("Number of subscribed items: " + SteamUGC.GetNumSubscribedItems());
            var nItems = SteamUGC.GetNumSubscribedItems();
            if (nItems > 0)
            {

                
               PublishedFileID = new PublishedFileId_t[nItems];
                uint ret = SteamUGC.GetSubscribedItems(PublishedFileID, nItems);

                //We want to iterate through all the file IDs in order to get their install info (folder out: Returns the absolute path to the folder containing the content by copying it) HOWEVER it only has the path if k_EItemStateInstalled is set
                foreach (PublishedFileId_t i in PublishedFileID)
                {
                    InstantiateProperWeapon(i);
                    //SteamAPICall_t handle = SteamUGC.RequestUGCDetails(i, 5);
                    installedStuff = SteamUGC.GetItemInstallInfo(i, out ulong SizeOnDisk, out string Folder, 1024, out uint punTimeStamp); //Must name the outs exactly the same as in docs, it returned null with folder, but making it Folder works
                    //print(Folder);


                    string[] path = Directory.GetFiles(Folder);
                    string filename = Path.GetFileName(path[0]);
                    string fullPath = path[0];
                    string destPath = Application.persistentDataPath + "/" + filename;
                    //print(destPath);
                    System.IO.File.Copy(fullPath, destPath, true);

                    Debug.Log(PublishedFileID);
                }
            }
            path = Application.persistentDataPath;

            foreach (string file in System.IO.Directory.GetFiles(path))
            {
                string tempFile = (System.IO.Path.GetFileName(file)); //we only want file names, not the whole directory to the file + name
                if (string.Equals(tempFile, "Player.log") == false && string.Equals(tempFile, "Player-prev.log") == false)
                {
                    listOfCustomLevels = listOfCustomLevels + tempFile + "\n";  //Make a string that concats all the files so that we can assign it to the scrollView
                }
            }
            scrollBoiText.text = listOfCustomLevels;
        }
    }


    public void InstantiateProperWeapon(PublishedFileId_t WeaponIndexID)
    {
        bool check = SteamUGC.DownloadItem(WeaponIndexID, true);
    }
}