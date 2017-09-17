using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetControl : MonoBehaviour {

    private Quaternion targetQuaternion;
    public float speed = 180f;
    public float rotation = 90f;

	void Start () {
        targetQuaternion = transform.localRotation;
    }
	
	void Update () {
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetQuaternion, speed * Time.deltaTime);		
	}

    public void TurnLeft() {
        Debug.Log("TurnLeft");
        targetQuaternion *= Quaternion.AngleAxis(-rotation, Vector3.up);
    }

    public void TurnRight() {
        Debug.Log("TurnRight");
        targetQuaternion *= Quaternion.AngleAxis(rotation, Vector3.up);
    }

}
