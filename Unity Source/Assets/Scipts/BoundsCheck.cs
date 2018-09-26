using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCheck : MonoBehaviour {

    // Make sure the player stays within play zone.
	void LateUpdate () {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp(pos.x, 0.02f, 0.98f);
        pos.y = Mathf.Clamp(pos.y, 0.03f, 0.96f);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }
}
