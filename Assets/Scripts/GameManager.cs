using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private ObjectPool _cubePool;

    [SerializeField]
    private NumberField _spawnIntervalField, _speedField, _distanceField;

    private float _timer;

    private void Start()
    {
        _cubePool.Init(transform);
        _cubePool.InstanceGet += OnCubeSpawn;
        _cubePool.InstanceRelease += OnCubeDespawn;
    }

    private void OnCubeSpawn(object sender, EventArgs e)
    {
        SFXPlayer.Play(SFXPlayer.SoundType.CubeSpawn);
    }

    private void OnCubeDespawn(object sender, EventArgs e)
    {
        SFXPlayer.Play(SFXPlayer.SoundType.CubeDespawn);
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer <= 0)
        {
            Transform cubeInstance = _cubePool.Get<Transform>();
            if (cubeInstance)
            {
                cubeInstance.position = transform.position;
                cubeInstance.SetParent(null);
                Cube cubeScript = cubeInstance.GetComponent<Cube>();
                cubeScript.Pool = _cubePool;
                var speed = _speedField.CurrentValue;
                var distance = _distanceField.CurrentValue;
                cubeScript.Init(transform.forward * speed, distance);
                _timer = _spawnIntervalField.CurrentValue;
            }
        }
        _timer -= Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2f);
    }
}
