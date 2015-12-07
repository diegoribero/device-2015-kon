using UnityEngine;
using System.Collections;

public class EnableGift : MonoBehaviour {

  public Rigidbody m_gift;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  void OnTriggerEnter(Collider collider) {
    if(collider.tag.Equals(Tags.Player) && m_gift != null) {
      m_gift.isKinematic = false;
    }
  }
}
