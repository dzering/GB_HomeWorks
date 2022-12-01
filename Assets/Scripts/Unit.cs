using System.Collections;
using UnityEngine;


public class Unit : MonoBehaviour
{
    [SerializeField] private int _health;
    private int _maxHealth;
    private bool _isReceiveHealing;

    public int Health
    {
        get { return _health; }
        set { _health = value; }
    }

    void Start()
    {
        _maxHealth = _health;
        Debug.Log($"Health: {_health}");
    }

    IEnumerator ReceiveHealing()
    {
        _isReceiveHealing = true;

        while(_isReceiveHealing)
        {
            _health += 5;
            if (_health > _maxHealth)
            {
                _health = _maxHealth;

                _isReceiveHealing = false;
            }
            Debug.Log($"Health: {_health}");

            yield return new WaitForSecondsRealtime(0.5f);
        }

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            _health = _health/Random.Range(1, Health);

        if (_health < _maxHealth && !_isReceiveHealing)
            StartCoroutine(ReceiveHealing());
    }
}
