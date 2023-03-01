using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    public float jumpForce = 10.0f;
    private int direction = -1;

    IEnumerator Jump(float time)
    {
        yield return new WaitForSeconds(time);

        //Jump Code
        gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(jumpForce * direction, jumpForce, 0));
        StartCoroutine(Jump(time));
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Jump(2.0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

}
