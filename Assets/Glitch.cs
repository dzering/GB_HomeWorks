using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glitch : MonoBehaviour
{
    private Renderer _renderer;
    [SerializeField] private float _glitchTime = 0.1f;
    [SerializeField] private float _glitchLoopTime = 0.1f;
    [SerializeField] private float _glitchChance = 0.1f;

    [SerializeField] private float _rotationSpeed = 20f;


    private void Awake()
    {
        _renderer = GetComponent<Renderer>();

    }
    IEnumerator Start()
    {
        while (true)
        {
            if(Random.Range(0f,1f) <= _glitchChance)
            {
                StartCoroutine(Glitching());
            }
            yield return new WaitForSeconds(_glitchLoopTime);

        }
    }

    private void Update()
    {
        
        transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);
    }

        IEnumerator Glitching()
        {
            _renderer.material.SetFloat("_Amount", 0.249f);
            _renderer.material.SetFloat("_CutoutTresh", 0.250f);
            _renderer.material.SetFloat("_Speed", Random.Range(1, 10));
            _renderer.material.SetFloat("_Amplitude", Random.Range(100, 250));
            yield return new WaitForSeconds(_glitchTime);
            _renderer.material.SetFloat("_Amount", 0);
            _renderer.material.SetFloat("_CutoutTresh", 0);
        }
}
