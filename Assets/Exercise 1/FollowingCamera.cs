using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    private GameObject _player;
    private Vector3 _cameraOffset;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        _cameraOffset = _player.transform.position - transform.position;
    }

    private void LateUpdate()
    {
        transform.position = _player.transform.position - _cameraOffset;
    }
}