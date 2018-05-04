using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {
    public float speed = 5f;
    private Vector3 start, end, next;
    public Transform childTransform, endTransform;
    public GameObject moveButton;
    public bool reset;
    private GameObject player;
    private bool move;
    GameObject[] paints;
    GameObject paint;
    // Use this for initialization
    void Start () {
        start = childTransform.localPosition;
        end = endTransform.localPosition;
        next = end;
        player = GameObject.FindGameObjectWithTag("Player");
        reset = false;
	}
	
	// Update is called once per frame
	void Update () {
        if(reset)
        {
            Unmove();
        }
        else if(move)
        {
            Move();
        }
	}

    private IEnumerator WaitMove(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Move();
    }

    private void Move()
    {
        childTransform.localPosition = Vector3.MoveTowards(childTransform.localPosition, next, speed * Time.deltaTime);
    }

    private void Unmove()
    {
        childTransform.localPosition = start;
        childTransform.localPosition = Vector3.MoveTowards(childTransform.localPosition, next, 0);
        reset = false;
        move = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Player on");
            collision.collider.transform.SetParent(childTransform);
            move = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

        Debug.Log("collision off:" + collision.gameObject.name);
        if (collision.gameObject.tag == "Player" && player.GetComponent<PlayerMovement>().isGrounded)
        {
            paints = GameObject.FindGameObjectsWithTag("BluePaint");
            foreach(GameObject p in paints)
            {
                if(p.transform.IsChildOf(childTransform))
                {
                    paint = p;
                }
            }
            Debug.Log("player off");
            childTransform.DetachChildren();
            endTransform.SetParent(childTransform);
            if(paint != null)
            {
                paint.transform.SetParent(childTransform);
            }
            
        }
    }
}
