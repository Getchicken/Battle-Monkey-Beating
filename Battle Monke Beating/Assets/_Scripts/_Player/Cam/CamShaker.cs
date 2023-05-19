using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CamShaker : MonoBehaviour
{
    public static CamShaker Instance;

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
        Instance = this;
        _startPos = _camera.localPosition;
        rb = player.GetComponent<Rigidbody>();
        _pm = player.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        CancelLeaning();
    }
    private void OnShake(float duration, float strength)
    {
        _camera.DOComplete();
        _camera.DOShakePosition(duration, strength);
        _camera.DOShakeRotation(duration, strength);
    }

    public void Leaning(float zTilt)
    {
        if (_pm == null) return;
        if (_pm.state == PlayerMovement.MovementState.wallrunning)
        {
            _camera.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
            return;
        }

        if (_pm.state == PlayerMovement.MovementState.walking || _pm.state == PlayerMovement.MovementState.sprinting)
        {
            if (_pm.horizontalInput > 0) _camera.DOLocalRotate(new Vector3(0, 0, -zTilt), 0.25f);
            if (_pm.horizontalInput < 0) _camera.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
        }
    }

    private void CancelLeaning()
    {
        if ((_pm.horizontalInput == 0f && _pm.verticalInput == 0f) || _pm.state == PlayerMovement.MovementState.climbing || _pm.state == PlayerMovement.MovementState.air)
        {
            if (_pm.state == PlayerMovement.MovementState.wallrunning) return;

            _camera.DOLocalRotate(new Vector3(0, 0, 0), 0.25f);
        }
    }
    public static void DoOnShake(float duration, float strength) => Instance.OnShake(duration, strength);

    public static void DoLeaning(float zTilt) => Instance.Leaning(zTilt);

    public static void DoFov(float endValue) => Instance.Fov(endValue);

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
        if (speed < _toggleSpeed) return;
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

    private void Fov(float endValue)
    {
        GetComponentInChildren<Camera>().DOFieldOfView(endValue, 0.25f);
    }
}
