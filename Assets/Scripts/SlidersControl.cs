using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlidersControl : MonoBehaviour
{

    public Slider Horizontal_S,Vertical_S; 


    public Camera RenderTextureCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HorizontalSlider ()
    {
       
        RenderTextureCamera.GetComponent<Camera>().orthographicSize = Horizontal_S.value ;

    }


    public void VerticalSlider()
    {
        RenderTextureCamera.GetComponent<Transform>().position = new Vector3(RenderTextureCamera.GetComponent<Transform>().position.x , Vertical_S.value, RenderTextureCamera.GetComponent<Transform>().position.z);
    }
}
