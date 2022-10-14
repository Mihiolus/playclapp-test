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
    private NumberField _spawnIntervalField, _speedField, _distanceField;

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
        SFXPlayer.Play(SFXPlayer.SoundType.CubeSpawn);
    }

    private Transform CreateCube()
    {
        var instance = Instantiate(_cubePrefab);
        instance.SetParent(transform);
        instance.gameObject.SetActive(false);
        instance.GetComponent<Cube>().Pool = _cubePool;
        return instance;
    }

    private void ReleaseCube(Transform cube)
    {
        cube.SetParent(transform);
        cube.gameObject.SetActive(false);
        SFXPlayer.Play(SFXPlayer.SoundType.CubeDespawn);
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
            Transform cubeInstance = _cubePool.Get();
            cubeInstance.position = transform.position;
            Cube cubeScript = cubeInstance.GetComponent<Cube>();
            var speed = _speedField.CurrentValue;
            var distance = _distanceField.CurrentValue;
            cubeScript.Init(transform.forward * speed, distance);
            _timer = _spawnIntervalField.CurrentValue;
        }
        _timer -= Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2f);
    }
}
