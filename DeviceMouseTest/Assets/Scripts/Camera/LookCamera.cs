using UnityEngine;
using System.Collections;

public class LookCamera : MonoBehaviour {

  public GameObject target;

	// Use this for initialization
	void Start () {
    if(target == null) {
      target = GameObject.FindGameObjectWithTag(Tags.Player);
    }
	}
	
	// Update is called once per frame
	void Update () {
    if(target != null) {
      LookToTarget();
    }
	}

  private void LookToTarget() {
    transform.LookAt(target.transform);
  }
}
