using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleScript : MonoBehaviour {

	ParticleSystem particles;

	// Use this for initialization
	void Awake () {
		particles = gameObject.GetComponent<ParticleSystem>();
		particles.Stop();
	}

	public void StartEmitting(){
		particles.Play();
	}
	public void StopEmitting(){
		particles.Stop();
	}
	public void moveToMouse(Vector3 mousePos){

		mousePos.z = 12;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);

		gameObject.transform.position = mousePos;
	}
}
