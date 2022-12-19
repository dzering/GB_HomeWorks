using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Material material = GetComponent<MeshRenderer>().material;
        string passName = material.GetPassName(0);

        Debug.Log(passName);
    }


}
