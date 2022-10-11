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

    }
}
