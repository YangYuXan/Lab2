using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    public bool shouldRotate=true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldRotate == true)
        {
            transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
        }

    }
}
