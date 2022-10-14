using System;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPool
{
    T Get<T>() where T : Component;
    void Release<T>(T instance) where T : Component;

    event EventHandler InstanceGet, InstanceRelease;

    int ActiveNumber { get; }
}

[Serializable]
public class ObjectPool : IObjectPool
{
    private Stack<GameObject> _inactiveInstances = new Stack<GameObject>();
    private List<GameObject> _activeInstances = new List<GameObject>();
    [SerializeField]
    private GameObject _prefab;
    [SerializeField]
    private int _initialNumber, _maxNumber;

    private Transform _poolTransform;

    public int ActiveNumber => _activeInstances.Count;

    public event EventHandler InstanceGet;
    public event EventHandler InstanceRelease;

    public void Init(Transform poolTransform)
    {
        _poolTransform = poolTransform;
        for (int i = 0; i < _initialNumber; i++)
        {
            GameObject instance = UnityEngine.Object.Instantiate(_prefab, poolTransform);
            instance.SetActive(false);
            _inactiveInstances.Push(instance);
        }
    }

    public T Get<T>() where T : Component
    {
        if (!_prefab.GetComponent<T>())
        {
            return default(T);
        }
        GameObject instance;
        if (_inactiveInstances.Count == 0)
        {
            if (_activeInstances.Count == _maxNumber)
            {
                return default(T);
            }
            instance = UnityEngine.Object.Instantiate(_prefab);
        }
        else
        {
            instance = _inactiveInstances.Pop();
        }
        _activeInstances.Add(instance);
        instance.SetActive(true);

        InstanceGet?.Invoke(this, EventArgs.Empty);

        return instance.GetComponent<T>();
    }

    public void Release<T>(T instance) where T : Component
    {
        Debug.Assert(_activeInstances.Contains(instance.gameObject), instance.gameObject);
        if (!_activeInstances.Contains(instance.gameObject))
        {
            Debug.Break();
        }
        instance.transform.SetParent(_poolTransform);
        instance.gameObject.SetActive(false);
        _activeInstances.Remove(instance.gameObject);
        _inactiveInstances.Push(instance.gameObject);

        InstanceRelease?.Invoke(this, EventArgs.Empty);
    }
}
