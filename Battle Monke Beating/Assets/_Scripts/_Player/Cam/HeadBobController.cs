using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobController : MonoBehaviour
{
    [SerializeField] private bool _enable = true;

    [SerializeField, Range(0, 0.1f)] private float _amplitude = 0.015f;
    [SerializeField, Range(0, 30f)] private float _frequency = 10.0f;

    [SerializeField] private Transform _camera = null;
    private float _toggleSpeed = 3f;
    private Vector3 _startPos;

    [SerializeField] private PlayerMovement _pm;
    [SerializeField] private GameObject player;
    Rigidbody rb;

    void Awake()
    {
        _startPos = _camera.localPosition;
        rb = player.GetComponent<Rigidbody>();
        _pm = player.GetComponent<PlayerMovement>();
    }

    void LateUpdate()
    {
        if (!_enable) return;

        CheckMotion();
        ResetPosition();
    }

    private void CheckMotion()
    {
        float speed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
        ResetPosition();
        if(speed < _toggleSpeed) return;
        if (!_pm.grounded && _pm.state == PlayerMovement.MovementState.sprinting || _pm.state == PlayerMovement.MovementState.walking)
        {
            PlayMotion(FootStepMotion());
        }
        else return;
    }
    private Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * _frequency) * _amplitude;
        pos.x += Mathf.Cos(Time.time * _frequency / 2) * _amplitude * 2;
        return pos;
    }

    private void ResetPosition()
    {
        if (_camera.localPosition == _startPos) return;
        _camera.localPosition = Vector3.Lerp(_camera.localPosition, _startPos, 1 * Time.deltaTime);
    }
 
    private void PlayMotion(Vector3 motion)
    {
        _camera.localPosition += motion;
    }
}
