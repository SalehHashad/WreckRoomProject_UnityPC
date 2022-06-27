using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using System.IO;
using UnityEngine.UI;
using TMPro;

//The name download is disengenuous as this has more to do with relocating installed files
public class SteamWorkshopDownload : MonoBehaviour
{
    bool installedStuff;
    uint folderSize;
    //public Button FirstButton;
    private List<string> listOfCustomLevels = new List<string>();
    private string path;

    void Start()
    {
        if (SteamManager.Initialized)
        {
            //Debug.Log("Number of subscribed items: " + SteamUGC.GetNumSubscribedItems());
            var nItems = SteamUGC.GetNumSubscribedItems();
            if (nItems > 0)
            {


                PublishedFileId_t[]  PublishedFileID = new PublishedFileId_t[nItems];
                uint ret = SteamUGC.GetSubscribedItems(PublishedFileID, nItems);

                //We want to iterate through all the file IDs in order to get their install info (folder out: Returns the absolute path to the folder containing the content by copying it) HOWEVER it only has the path if k_EItemStateInstalled is set
                foreach (PublishedFileId_t i in PublishedFileID)
                {
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
                    listOfCustomLevels.Add(tempFile);  //Make a string that concats all the files so that we can assign it to the scrollView
                }
               
            }

            InstantiateProperWeapon(listOfCustomLevels);

            //scrollBoiText.text = listOfCustomLevels;
            //for (int i = 0; i < listOfCustomLevels.Count; i++)
            //{
            //    if (i == 0)
            //    {
            //        FirstButton.GetComponentInChildren<TMP_Text>().text = listOfCustomLevels[0];
            //        FirstButton.onClick.AddListener(() => InstantiateProperWeapon(listOfCustomLevels[0]));
            //    }
            //    else
            //    {

            //        Button button = Instantiate(FirstButton, FirstButton.transform.parent);
            //    button.GetComponentInChildren<TMP_Text>().text = listOfCustomLevels[i];
            //        button.onClick.RemoveAllListeners();
            //    button.onClick.AddListener(() => InstantiateProperWeapon(listOfCustomLevels[i]));

            //    }

            //    int x = 0;
            //}
        }
    }


    void InstantiateProperWeapon(List<string> fileNames)
    {
        //GetComponent<SaveUploadWeapon>().LoadModifiedWeapon(fileNames);
    }

    

    
}