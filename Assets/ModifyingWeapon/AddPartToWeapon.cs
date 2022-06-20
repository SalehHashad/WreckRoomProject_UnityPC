using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AddPartToWeapon : MonoBehaviour
{
    GameObject Part;

    Vector3 partPosition;

    List<GameObject> partsList = new List<GameObject>();
    void Start()
    {
        Part = FindObjectOfType<CubPart>().gameObject;
        partPosition = Part.transform.position;
    }

   public void AddPartButton_Click()
    {
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
        GameObject newPart = Instantiate(Part, newPosition,Part.transform.rotation);
        newPart.transform.parent = Part.transform.parent;
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
