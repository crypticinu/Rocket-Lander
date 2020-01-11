using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour

{
    [SerializeField] Vector3 MovementVector;

    [Range(0,1)]
    [SerializeField] float MovementFactor; //0 moved // 1 for fully moved

    Vector3 StartingPos;

    // Start is called before the first frame update
    void Start()
    {
        StartingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = MovementFactor * MovementVector;
        transform.position = StartingPos + offset;
    }
}
