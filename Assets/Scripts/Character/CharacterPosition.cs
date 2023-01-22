using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPosition : MonoBehaviour
{
    public Transform KayakPos;
    Transform _transform;

    private void Start()
    {
        _transform = this.transform;
    }
    void Update()
    {
        /*        transform.position = KayakPos.position;
                transform.rotation = KayakPos.rotation;*/
        MatchCharacterWithBoat();
    }

    private void MatchCharacterWithBoat()
    {
        Vector3 position = KayakPos.position;
        _transform.position = position;
        Vector3 rotation = _transform.rotation.eulerAngles;
        Vector3 boatRotation = KayakPos.rotation.eulerAngles;
        _transform.rotation = Quaternion.Euler(rotation.x, boatRotation.y + 180, -boatRotation.z + 180);
    }
}
