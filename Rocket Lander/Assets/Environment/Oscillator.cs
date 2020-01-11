using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour

{
    [SerializeField] Vector3 movementVector = new Vector3(15.8f, 0f, 0f);
    [SerializeField] float period = 2f;

    [Range(0, 1)]
    float movementFactor; //0 moved // 1 for fully moved

    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon)
            return;

        float cycles = Time.time / period; //grows continually from 0. 1 second game time = half a cycle (if period is defaulted at 2f)

        const float tau = Mathf.PI * 2f; //about 6.28
        Vector3 offset = movementFactor * movementVector;

        float rawSinWave = Mathf.Sin(cycles * tau); //goes from -1 to +1

        movementFactor = rawSinWave / 2f + 0.5f; // goes from 0 to 1

        transform.position = startingPos + offset;
    }
}
