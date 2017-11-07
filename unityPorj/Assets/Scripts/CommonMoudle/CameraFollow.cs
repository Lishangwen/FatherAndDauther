using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour 
{
    public Transform target;
    private Vector3 offset;

    // Use this for initialization
    void Start()
    {
        offset = target.position - this.transform.position;

    }
    // Update is called once per frame
    void Update()
    {
        if(target.localPosition.x>0.1f&&target.localPosition.x<76.6f)
        {
            this.transform.position = target.position - offset;
        }
    }
}
