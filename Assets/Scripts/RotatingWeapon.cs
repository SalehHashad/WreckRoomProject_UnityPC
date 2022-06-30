using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingWeapon : MonoBehaviour
{
    public float rotate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0.0f, rotate, 0.0f, Space.World);
    }
}
