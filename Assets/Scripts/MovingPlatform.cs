using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    Vector2 startPos;
    public Transform endPos;
    public float speed;
    float xDiff;
    float yDiff;
    float xRatio;
    int direction = 1;

    public float waitTime;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;

        xDiff = endPos.transform.localPosition.x - startPos.x;
        yDiff = endPos.transform.localPosition.y - startPos.y;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
