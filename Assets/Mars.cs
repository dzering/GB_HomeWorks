using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mars : MonoBehaviour
{
    [SerializeField] float _speedRotation = 10;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * _speedRotation);
        transform.Rotate(Vector3.forward, Time.deltaTime * _speedRotation/0.3f);
    }
}
