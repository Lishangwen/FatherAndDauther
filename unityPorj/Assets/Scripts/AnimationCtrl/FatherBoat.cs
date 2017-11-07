using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatherBoat : MonoBehaviour 
{
    private Animator myAnimator;
    bool isMove = false;
    public float leaveSpeed;
    public float scaleSpeed;
	// Use this for initialization
	void Start () 
    {
        myAnimator = this.GetComponent<Animator>();
        Idle();
	}

    public void Idle()
    {
        isMove = false;
        myAnimator.SetBool("isStop", true);
    }
    public void Leave()
    {
        isMove = true;
        myAnimator.SetBool("isStop", false);
    }
	// Update is called once per frame
	void Update () 
    {
		if(isMove)
        {
            this.transform.localPosition += new Vector3(0, Time.deltaTime * leaveSpeed, 0);
            if (this.transform.localScale.x>0)
            {
                this.transform.localScale -= new Vector3(Time.deltaTime * scaleSpeed, Time.deltaTime * scaleSpeed, 0);
            }
            if(this.transform.localScale.x<0)
            {
                this.transform.localScale = new Vector3(0, 0, 0);
            }
            
        }
	}
}
