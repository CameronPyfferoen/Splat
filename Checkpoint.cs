using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Checkpoint : MonoBehaviour {

    public Inventory _Inventory;
    public bool isTriggered = false;
    public Sprite checkMark;
    //public Timer _time;
    //public float CPtimes;
    private SpriteRenderer spriteRenderer;
    public GameObject[] allPaints;
    public List<GameObject> unusedPaints = new List<GameObject>();
    public List<GameObject> CPFloorPaints = new List<GameObject>();
    public List<GameObject> unpressedbuttons = new List<GameObject>();
    public List<Transform> cratesTransforms = new List<Transform>();
    public List<Vector3> cratePositions = new List<Vector3>();
    public List<GameObject> openBois = new List<GameObject>();
    public List<Vector3> elevatorPositions = new List<Vector3>();
    public List<Transform> elevatorTransforms = new List<Transform>();

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (spriteRenderer.sprite != checkMark)
        {
            if (collider.tag == "Player")
            {
                _Inventory.Clear();
                CPFloorPaints.Clear();
                foreach (GameObject BluePaint in GameObject.FindGameObjectsWithTag("BluePaint"))
                {
                    CPFloorPaints.Add(BluePaint);
                }
                foreach (GameObject YellowPaint in GameObject.FindGameObjectsWithTag("YellowPaint"))
                {
                    CPFloorPaints.Add(YellowPaint);
                }
                foreach (GameObject WhitePaint in GameObject.FindGameObjectsWithTag("WhitePaint"))
                {
                    CPFloorPaints.Add(WhitePaint);
                }
                foreach(GameObject Button in GameObject.FindGameObjectsWithTag("Button"))
                {
                    if (Button.GetComponent<ButtonScript>() != null)
                    {
                        if(!Button.GetComponent<ButtonScript>().pressed)
                        {
                            unpressedbuttons.Add(Button);
                        }
                    }
                    else if (Button.GetComponent<HoldButton>() != null)
                    {
                        if(!Button.GetComponent<HoldButton>().pressed)
                        {
                            unpressedbuttons.Add(Button);
                        }
                    }
                    else if (Button.GetComponent<ButtonRepeat>() != null)
                    {
                        if(!Button.GetComponent<ButtonRepeat>().pressed)
                        {
                            unpressedbuttons.Add(Button);
                        }
                    }
                }
                GameObject[] Crates = GameObject.FindGameObjectsWithTag("Object");
                foreach(GameObject crate in Crates)
                {
                    cratesTransforms.Add(crate.transform);
                }
                foreach(Transform crateTrans in cratesTransforms)
                {
                    cratePositions.Add(crateTrans.localPosition);
                }
                foreach(GameObject garage in GameObject.FindGameObjectsWithTag("GarageDoor"))
                {
                    if (garage.GetComponent<GarageDoor>() != null)
                    {
                        if (!garage.GetComponent<GarageDoor>().isClosed)
                        {
                            openBois.Add(garage);
                        }
                    }
                    else if (garage.GetComponent<BackwardsGarageDoor>() != null)
                    {
                        if(!garage.GetComponent<BackwardsGarageDoor>().isClosed)
                        {
                            openBois.Add(garage);
                        }
                    }
                }
                
                foreach(GameObject elevator in GameObject.FindGameObjectsWithTag("Elevator"))
                {
                    elevatorTransforms.Add(elevator.transform);
                    elevator.GetComponent<PlatformStage>().position = elevator.GetComponent<PlatformStage>().startingPosition;
                }
                foreach(Transform et in elevatorTransforms)
                {
                    elevatorPositions.Add(et.localPosition);
                }
                
                unusedPaints.Clear();
                isTriggered = true;
                //CPtimes = _time.playTime;
                //_time.StoreTime(CPtimes);
                foreach (GameObject used in allPaints)
                {
                    if (used.activeInHierarchy)
                    {
                        unusedPaints.Add(used);
                    }
                }
            }
        }
        

        spriteRenderer.sprite = checkMark;
    }

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //CPtimes = 0;
        allPaints = GameObject.FindGameObjectsWithTag("Buckets");
	}
	
	// Update is called once per frame
	void Update () {

	}
    
}
