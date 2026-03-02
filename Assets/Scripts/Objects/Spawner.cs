using System;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private TimeManager _timeManager;
    [SerializeField] private ObjectMovement[] _fallingLines;
    [SerializeField] private GameObject _objectToSpawn;
    [SerializeField] private GameObject _objectToSpawn2;
    [SerializeField] private GameObject _objectToSpawn3;
    [SerializeField] private GameObject _BonusToSpawn;
    [SerializeField] private int _spawnTimer = 0;
    [SerializeField] private int _spawnInterval = 4;
    private int _RefNumber;
    public event Action OnSpawn;

    private void OnEnable()
    {
        _timeManager.OnTimePassed += TimeGestion;
    }

    private void OnDisable()
    {
        _timeManager.OnTimePassed -= TimeGestion;
    }

    private int random()
    {

        return _RefNumber = UnityEngine.Random.Range(0, _fallingLines.Length);
        
    }

    private int randomBonus()
    {

        return  UnityEngine.Random.Range(0,5);

    }
    private void TimeGestion()
    {
        _spawnTimer++;
        if (_spawnTimer >= _spawnInterval)
        {
            OnSpawn?.Invoke();
            _spawnTimer = 0;
           random();
            var LastRefNumb = _RefNumber;
            
            if (_RefNumber == 0)
            {
                _fallingLines[_RefNumber].Init(Instantiate(_objectToSpawn));
            }
            if (_RefNumber == 1)
            {
                _fallingLines[_RefNumber].Init(Instantiate(_objectToSpawn2));
            }
            if (_RefNumber == 2)
            {
                _fallingLines[_RefNumber].Init(Instantiate(_objectToSpawn3));
            }
            if (randomBonus() == 0)
            {

                int newRef;
                do
                {
                    newRef = UnityEngine.Random.Range(0, _fallingLines.Length);
                } while (newRef == LastRefNumb);

                _fallingLines[newRef].Init(Instantiate(_BonusToSpawn));
            }
            
        }
    }
}

