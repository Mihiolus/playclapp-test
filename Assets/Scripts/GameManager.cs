using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GameManager : MonoBehaviour
{
    private ObjectPool<Transform> _cubePool;

    [SerializeField]
    private Transform _cubePrefab;

    [SerializeField]
    private int _initCubes, _maxCubes;

    [SerializeField]
    private float _spawnInterval = 1f, _speed = 1f, _distance = 10f;
    private float _timer;

    private void InitPool()
    {
        _cubePool = new ObjectPool<Transform>(
            CreateCube,
            GetCube,
            ReleaseCube,
            t => Destroy(t.gameObject),
            true, _initCubes, _maxCubes);
    }

    private void GetCube(Transform cube)
    {
        cube.SetParent(null);
        cube.gameObject.SetActive(true);
    }

    private Transform CreateCube()
    {
        var instance = Instantiate(_cubePrefab);
        instance.SetParent(transform);
        instance.gameObject.SetActive(false);
        return instance;
    }

    private void ReleaseCube(Transform cube)
    {
        cube.SetParent(transform);
        cube.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        InitPool();
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer <= 0)
        {
            _cubePool.Get();
            _timer = _spawnInterval;
        }
        _timer -= Time.deltaTime;
    }
}
