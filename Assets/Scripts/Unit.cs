using System.Collections;
using UnityEngine;


public class Unit : MonoBehaviour
{
    [SerializeField] private bool _isActive;

    [Header("Health setting")]
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _receiveHealth;

    [Header("Receive setting")]
    [SerializeField] private float _waitingTime;
    [SerializeField] private float _receiveTime;

    private int _health;
    private bool _isReceiving;


    private void Awake()
    {
        if(!_isActive)
            this.enabled = false;  
    }

    private void Start()
    {
        _health = _maxHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int damage = Random.Range(1, _health);
            int newHealth = _health - damage;
            Debug.Log($"{_health} - {damage} = {newHealth}");
            _health = newHealth;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && !_isReceiving)
        {
            StartCoroutine(ReceiveHealth());
        }
    }

    IEnumerator ReceiveHealth()
        {
        float startTime = Time.time;
        float counter = 0;

        _isReceiving = true;

        while (_isReceiving)
            {
            counter += _waitingTime;
            _health += _receiveHealth;
            Debug.Log($"Count: {counter}");


            if(counter >= _receiveTime)
            {
                _isReceiving = false;
            }
            Debug.Log($"Health: {_health}");

            yield return new WaitForSecondsRealtime(0.5f);
        }

            if (_health >= _maxHealth)
            {
                _health = _maxHealth;
                _isReceiving = false;
    }

            Debug.Log($"Health: {_health}");

            yield return new WaitForSecondsRealtime(_waitingTime);
        }
        Debug.Log(Time.time - startTime);
    }
}
