using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _spawnObject;
    [SerializeField] private int _spawnCount;

    [Header("Spawn Point")]
    [SerializeField] private float _xMinimumRange;
    [SerializeField] private float _xMaximumRange;
    [SerializeField] private float _yMinimumRange;
    [SerializeField] private float _yMaximumRange;

    public void SpawnObjects()
    {
        RangeValidation();

        for (int i = 0; i < _spawnCount; i++)
        {
            var spawnPoint = new Vector3(Random.Range(_xMinimumRange, _xMaximumRange), Random.Range(_yMinimumRange, _yMaximumRange));
        }
    }

    private void RangeValidation()
    {
        if(_xMinimumRange >= _xMaximumRange)
            _xMinimumRange = _xMaximumRange - 1f;

        if (_yMinimumRange >= _yMaximumRange)
            _yMinimumRange = _yMaximumRange - 1f;
    }
}