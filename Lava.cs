using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour {

    Vector3 startPos, endPos;
    public Transform lavaTransform;
    public Rigidbody2D lavaBody;
    public float intervalTime, fallDistance;
	// Use this for initialization
	void Start () {
        startPos = lavaTransform.localPosition;
        Vector3 down = new Vector2(0f, -fallDistance);
        endPos = lavaTransform.localPosition + down;
        //Debug.Log("End position: " + endPos);
	}
	
	// Update is called once per frame
	void Update () {
		if(lavaTransform.localPosition.y <= endPos.y)
        {
            //Debug.Log("Freeze");
            lavaBody.constraints = RigidbodyConstraints2D.FreezePositionY;
            StartCoroutine(WaitReset(intervalTime));
        }
	}

    private IEnumerator WaitReset(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Reset();
    }

    private void Reset()
    {
        lavaTransform.localPosition = startPos;
        lavaBody.constraints = RigidbodyConstraints2D.None;
        lavaBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }
}
