using UnityEngine;

public class Cube : MonoBehaviour
{
    private Vector3 _startingPosition, _velocity;
    private float _distance;
    public ObjectPool Pool
    {
        get; set;
    }

    public void Init(Vector3 velocity, float distance)
    {
        _startingPosition = transform.position;
        _velocity = velocity;
        _distance = distance;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _velocity * Time.deltaTime;
        if (Vector3.Distance(_startingPosition, transform.position) > _distance)
        {
            Pool.Release(transform);
        }
    }
}
