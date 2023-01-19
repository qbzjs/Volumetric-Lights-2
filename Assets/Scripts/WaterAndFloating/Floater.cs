using System;
using UnityEngine;

public class Floater : MonoBehaviour
{
     [SerializeField] private Waves _waves;
     [SerializeField] private Rigidbody _rigidbody;
     [SerializeField] private float _depthBeforeSubmerged = 1f;
     [SerializeField] private float _displacementAmount = 3f;
     [SerializeField] private int _floaterCount = 1;
     [SerializeField] private float _waterDrag = 0.99f;
     [SerializeField] private float _waterAngularDrag = 0.5f;

     private void FixedUpdate()
     {
          _rigidbody.AddForceAtPosition(Physics.gravity / _floaterCount, transform.position, ForceMode.Acceleration);
          
          float waveHeight = _waves.GetHeight(transform.position);
          if (transform.position.y < waveHeight)
          {
               //get displacement multiplier (how far is the floater from water surface)
               float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / _depthBeforeSubmerged) * _displacementAmount;
               //force at position
               _rigidbody.AddForceAtPosition(
                    new Vector3(0, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0), 
                    transform.position, 
                    ForceMode.Acceleration);
               //velocity * waterDrag
               _rigidbody.AddForce(
                    -_rigidbody.velocity * (displacementMultiplier * _waterDrag * Time.fixedDeltaTime),
                    ForceMode.VelocityChange);
               //angularVelocity * waterAngularDrag
               _rigidbody.AddTorque(
                    -_rigidbody.angularVelocity * (displacementMultiplier * _waterAngularDrag * Time.fixedDeltaTime),
                    ForceMode.VelocityChange);
          }
     }
}