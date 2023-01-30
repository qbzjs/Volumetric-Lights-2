using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Transform _posSednaTarget;
    [SerializeField] Transform _rotationKayak;

    private Rigidbody _rigidbody;
    private Vector3 _velocity;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    void LateUpdate()
    {
        Position();
        Rotation();
    }
    private void Position()
    {
        Vector3 current = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 target = new Vector3(_posSednaTarget.transform.position.x, 0, _posSednaTarget.transform.position.z);
        Vector3 pos = Vector3.SmoothDamp(current, target, ref _velocity, 0.5f);

        Vector3 velocity = new Vector3(_velocity.x, _rigidbody.velocity.y, _velocity.z);

        _rigidbody.velocity = velocity;
    }
    private void Rotation()
    {
        Quaternion targetQuaternion = Quaternion.Euler(new Vector3(0, _rotationKayak.rotation.eulerAngles.y, 0));

        Quaternion rota = Quaternion.Slerp(_rotationKayak.rotation, targetQuaternion, 0.5f);

        _rigidbody.MoveRotation(rota);
    }
}
