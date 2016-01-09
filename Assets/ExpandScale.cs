using UnityEngine;

/// <summary>
///     This behavior scales the associated transform over a timespan.
/// </summary>
public class ExpandScale : MonoBehaviour
{
    [SerializeField] private Vector3 _endVector = Vector3.one;
    [SerializeField] private Vector3 _startVector = Vector3.zero;
    [SerializeField] private float _speed = 10;

    private void OnEnable()
    {
        transform.localScale = _startVector;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, _endVector, _speed * Time.deltaTime);
    }
}