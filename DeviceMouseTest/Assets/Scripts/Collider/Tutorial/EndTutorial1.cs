﻿using UnityEngine;
using System.Collections;

public class EndTutorial1 : MonoBehaviour {

  public float waitTime = 2.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  void OnCollisionEnter(Collision collision) {
    if(collision.gameObject.tag.Equals(Tags.Terrain)) {
      StartCoroutine(EndLevelTutorial(waitTime));
    }
  }

  IEnumerator EndLevelTutorial(float timer) {
    yield return new WaitForSeconds(timer);
    Time.timeScale = 0;
    Debug.Log("This level was finished, waiting the load of the next");
  }
}
