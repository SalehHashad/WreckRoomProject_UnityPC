using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AddPartToWeapon : MonoBehaviour
{
    public GameObject Part;

    Vector3 partPosition;

    public TMP_Text UploadFeedBack_text;

    List<GameObject> partsList = new List<GameObject>();
    void Start()
    {
        Part = FindObjectOfType<CubPart>().gameObject;
        partPosition = Part.transform.position;
    }

   public void AddPartButton_Click()
    {
        UploadFeedBack_text.text = "";

        if (partsList.Count > 4)
        {
            partPosition = Part.transform.position;
            foreach (var item in partsList)
            {
                Destroy(item);
            }
            partsList.Clear();
        }

        Vector3 newPosition = new Vector3(partPosition.x,partPosition.y- 0.07f, partPosition.z);
        GameObject newPart = Instantiate(Part, newPosition,Part.transform.rotation, Part.transform.parent);
        //newPart.transform.parent = Part.transform.parent;
        if(newPart.GetComponent<CubPart>()==null)
            newPart.AddComponent<CubPart>();
        partPosition = newPart.transform.position;
        partsList.Add(newPart);
    }


    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

   


}
