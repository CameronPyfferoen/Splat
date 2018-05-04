using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Placing : MonoBehaviour {

    public Inventory PlayerInventory;
    public Dropdown paints;
    private int currentValue;
    public Button placeButton;
    public Sprite yellowPlace, bluePlace, whitePlace;
    public GameObject player;
    public GameObject placeObject;
    public GameObject childObject;
    private GameObject belowObject;
    private bool frontHit;
    private RaycastHit2D belowHit;
    private RaycastHit2D fHit;
    public GameObject highlight;
    public LayerMask mask = 0;
    int orderinlayer = 3;
    private AudioSource placeSource;
    public AudioClip placeSound;
    private GameObject pb;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        highlight.GetComponent<SpriteRenderer>().enabled = false;
        placeSource = GetComponent<AudioSource>();
        pb = placeButton.gameObject;
    }

    void Update()
    {
        Vector3 front = new Vector2(1.0f, 0);
        Vector3 feet = new Vector2(0, -0.75f);
        Vector3 below = new Vector2(0, 1.5f);
        if(player.GetComponent<PlayerMovement>().isFacingRight && !player.GetComponent<PlayerMovement>().upsideDown)
        {
            frontHit = Physics2D.Linecast(player.transform.position + feet, player.transform.position + front + feet, 1 << 0);
            fHit = Physics2D.Linecast(player.transform.position + feet, player.transform.position + front + feet, 1 << 0);
            Debug.DrawLine(player.transform.position + feet, player.transform.position + front + feet, Color.green);
        }
        else if(!player.GetComponent<PlayerMovement>().isFacingRight && !player.GetComponent<PlayerMovement>().upsideDown)
        {
            frontHit = Physics2D.Linecast(player.transform.position + feet, player.transform.position - front + feet, 1 << 0);
            fHit = Physics2D.Linecast(player.transform.position + feet, player.transform.position - front + feet, 1 << 0);
            Debug.DrawLine(player.transform.position + feet, player.transform.position - front + feet, Color.green);
        }
        else if (player.GetComponent<PlayerMovement>().isFacingRight && player.GetComponent<PlayerMovement>().upsideDown)
        {
            frontHit = Physics2D.Linecast(player.transform.position - feet, player.transform.position + front - feet, 1 << 0);
            fHit = Physics2D.Linecast(player.transform.position + feet, player.transform.position + front - feet, 1 << 0);
            Debug.DrawLine(player.transform.position - feet, player.transform.position + front - feet, Color.green);
        }
        else if (!player.GetComponent<PlayerMovement>().isFacingRight && player.GetComponent<PlayerMovement>().upsideDown)
        {
            frontHit = Physics2D.Linecast(player.transform.position - feet, player.transform.position - front - feet, 1 << 0);
            fHit = Physics2D.Linecast(player.transform.position + feet, player.transform.position - front - feet, 1 << 0);
            Debug.DrawLine(player.transform.position - feet, player.transform.position - front - feet, Color.green);
        }
        if (!player.GetComponent<PlayerMovement>().upsideDown)
        {
            belowHit = Physics2D.Linecast(player.transform.position, player.transform.position - below, 1 << 0);
            Debug.DrawLine(player.transform.position, player.transform.position - below, Color.magenta);
        }
        else if (player.GetComponent<PlayerMovement>().upsideDown)
        {
            belowHit = Physics2D.Linecast(player.transform.position, player.transform.position + below, 1 << 0);
            Debug.DrawLine(player.transform.position, player.transform.position + below, Color.magenta);
        }
        if (placeObject != null)
        {
            Vector3 up = new Vector2(0, 1.0f);
            Debug.DrawLine(placeObject.transform.position, placeObject.transform.position + up, Color.red);
        }
        PaintSelected();
        //disable this section if need be
        if(paints.options.Count != 0)
        {
            if (paints.options[currentValue].image.name == "blue paint" || paints.options[currentValue].image.name == "yellow paint")
            {
                if(FindClosestGround() != null)
                {
                    placeObject = FindClosestGround();
                }
                else if(frontHit)
                {
                    placeObject = belowHit.collider.gameObject;
                }
                else if(belowHit.collider != null)
                {
                    placeObject = belowHit.collider.gameObject;
                }
                if(placeObject != null)
                {
                    if (placeObject.tag != "Unplaceable")
                    {
                        highlight.transform.position = placeObject.transform.position;
                        
                        highlight.GetComponent<SpriteRenderer>().enabled = true;
                        if(placeObject.tag == "MovingPlatform")
                        {
                            highlight.transform.SetParent(placeObject.transform);
                            Vector3 resize = new Vector3(.78f, 1.565f, 1f);
                            highlight.transform.localScale = resize;
                        }
                        else
                        {
                            highlight.transform.SetParent(null);
                            highlight.transform.localScale = placeObject.transform.localScale;
                        }
                        highlight.transform.localRotation = placeObject.transform.localRotation;
                    }
                    else
                    {
                        highlight.GetComponent<SpriteRenderer>().enabled = false;
                    }
                }
                else
                {
                    highlight.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
            else if (paints.options[currentValue].image.name == "white paint")
            {
                placeObject = FindClosestObject();
                if (placeObject != null)
                {
                    highlight.transform.position = placeObject.transform.position;
                    highlight.GetComponent<SpriteRenderer>().enabled = true;
                }
                else
                {
                    highlight.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
            else if (paints.options[currentValue].image.name == "red paint")
            {
                placeObject = FindClosestSpinner();
                if(placeObject != null)
                {
                    highlight.transform.position = placeObject.transform.position;
                    highlight.GetComponent<SpriteRenderer>().enabled = true;
                }
                else
                {
                    highlight.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
            else
            {
                highlight.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        else
        {
            highlight.GetComponent<SpriteRenderer>().enabled = false;
        }
        if (paints.options.Count != 0)
        {
            pb.SetActive(true);
            if (paints.options[currentValue].image.name == "blue paint")
            {
                placeButton.GetComponent<Image>().sprite = bluePlace;
            }
            else if (paints.options[currentValue].image.name == "yellow paint")
            {
                placeButton.GetComponent<Image>().sprite = yellowPlace;
            }
            else if(paints.options[currentValue].image.name == "white paint")
            {
                placeButton.GetComponent<Image>().sprite = whitePlace;
            }
        }
        else
        {
            pb.SetActive(false);
        }
    }

    void PaintSelected()
    {
        currentValue = paints.value;
    }

    public void PlacePaint()
    {
        if(paints.options[currentValue].image.name == "white paint")
        {
            placeObject = FindClosestObject();
            placeObject.GetComponent<ObjectMovement>().positionLocked = false;
            childObject = Instantiate(Resources.Load("wpFloor")) as GameObject;
            childObject.transform.parent = placeObject.transform;
            childObject.transform.position = placeObject.transform.position;
            orderinlayer++;
            childObject.GetComponent<SpriteRenderer>().sortingOrder = orderinlayer;
            PlayerInventory.RemoveItem(currentValue);
        }

        else if(paints.options[currentValue].image.name == "blue paint")
        {
            placeObject = FindClosestGround();
            if (frontHit || placeObject == null)
            {
                    //Debug.Log("Front object: " + fHit.collider.gameObject);
                    placeObject = belowHit.collider.gameObject;
                    Debug.Log("Object below: " + placeObject);
                    childObject = Instantiate(Resources.Load("bpfloor")) as GameObject;
                    childObject.transform.parent = placeObject.transform;
                    //childObject.transform.localScale = placeObject.transform.localScale;
                    Debug.Log("blue paint created!");
                    childObject.transform.position = placeObject.transform.position;
                    orderinlayer++;
                    childObject.GetComponent<SpriteRenderer>().sortingOrder = orderinlayer;
                    if (player.GetComponent<PlayerMovement>().upsideDown)
                    {
                        childObject.transform.position = placeObject.transform.position;
                        //Vector3 lower = new Vector2(0, -1.0f);
                        //childObject.transform.position += lower;
                        childObject.transform.Rotate(0, 0, 180);
                        if (placeObject.tag == "Walkway")
                        {
                            Debug.Log("paint descended");
                            Vector3 descend = new Vector2(0, -.18f);
                            Vector3 size = new Vector2(0, -1.5f);
                            childObject.transform.position += descend;
                            childObject.transform.localScale += size;
                        }
                        else if (placeObject.tag == "MovingPlatform")
                        {
                            Vector3 raise = new Vector2(0, .76f);
                            childObject.transform.localPosition += raise;
                        }
                     }
                    else if (!player.GetComponent<PlayerMovement>().upsideDown)
                    {
                        childObject.transform.position = placeObject.transform.position;
                        if (placeObject.tag == "Walkway")
                        {
                            Debug.Log("paint descended");
                            Vector3 descend = new Vector2(0, -.18f);
                            Vector3 size = new Vector2(0, -1.5f);
                            childObject.transform.position += descend;
                            childObject.transform.localScale += size;
                        }
                        else if (placeObject.tag == "MovingPlatform")
                        {
                            Vector3 raise = new Vector2(0, .76f);
                            childObject.transform.localPosition += raise;
                        }
                }
                    //childObject.transform.position = placeObject.transform.position;
                    PlayerInventory.RemoveItem(currentValue);
            }
            else if(!ObjectObstruction(placeObject))
            {
                Debug.Log("Player position:" + player.transform.position);
                Debug.Log("Object position:" + placeObject.transform.position);
                childObject = Instantiate(Resources.Load("bpfloor")) as GameObject;
                childObject.transform.parent = placeObject.transform;
                Debug.Log("blue paint created!");
                childObject.transform.position = placeObject.transform.position;
                //childObject.transform.localScale = placeObject.transform.localScale;
                orderinlayer++;
                childObject.GetComponent<SpriteRenderer>().sortingOrder = orderinlayer;
                if (player.GetComponent<PlayerMovement>().upsideDown)
                {
                    childObject.transform.position = placeObject.transform.position;
                    //Vector3 lower = new Vector2(0, -1.0f);
                    //childObject.transform.position += lower;
                    childObject.transform.Rotate(0, 0, 180);
                    if (placeObject.tag == "Walkway")
                    {
                        Debug.Log("paint descended");
                        Vector3 descend = new Vector2(0, -.18f);
                        Vector3 size = new Vector2(0, -1.5f);
                        childObject.transform.position += descend;
                        childObject.transform.localScale += size;
                    }
                    else if (placeObject.tag == "MovingPlatform")
                    {
                        Vector3 raise = new Vector2(0, .76f);
                        childObject.transform.localPosition += raise;
                    }
                }
                else if (!player.GetComponent<PlayerMovement>().upsideDown)
                {
                    childObject.transform.position = placeObject.transform.position;
                    if (placeObject.tag == "Walkway")
                    {
                        Debug.Log("paint descended");
                        Vector3 descend = new Vector2(0, -.18f);
                        Vector3 size = new Vector2(0, -1.5f);
                        childObject.transform.position += descend;
                        childObject.transform.localScale += size;
                    }
                    else if (placeObject.tag == "MovingPlatform")
                    {
                        Vector3 raise = new Vector2(0, .76f);
                        childObject.transform.localPosition += raise;
                    }
                }
                //childObject.transform.position = placeObject.transform.position;
                PlayerInventory.RemoveItem(currentValue);
            }
        }
            

        else if(paints.options[currentValue].image.name == "red paint")
        {
            placeObject = FindClosestSpinner();
            placeObject.GetComponent<SpinnerScript>().redPaint = true;
            PlayerInventory.RemoveItem(currentValue);
        }

        else if(paints.options[currentValue].image.name == "yellow paint")
        {
            placeObject = FindClosestGround();
            if (frontHit || placeObject == null)
            {
                placeObject = belowHit.collider.gameObject;
                childObject = Instantiate(Resources.Load("ypfloor")) as GameObject;
                childObject.transform.parent = placeObject.transform;
                Debug.Log("yellow paint created!");
                childObject.transform.position = placeObject.transform.position;
                if (player.GetComponent<PlayerMovement>().upsideDown)
                {
                    childObject.transform.position = placeObject.transform.position;
                    //Vector3 lower = new Vector2(0, -1.0f);
                    //childObject.transform.position += lower;
                    childObject.transform.Rotate(0, 0, 180);
                    if (placeObject.tag == "Walkway")
                    {
                        Debug.Log("paint descended");
                        Vector3 descend = new Vector2(0, -.18f);
                        Vector3 size = new Vector2(0, -1.5f);
                        childObject.transform.position += descend;
                        childObject.transform.localScale += size;
                    }
                    else if (placeObject.tag == "MovingPlatform")
                    {
                        Vector3 raise = new Vector2(0, .76f);
                        childObject.transform.localPosition += raise;
                    }
                }
                else if (!player.GetComponent<PlayerMovement>().upsideDown)
                {
                    childObject.transform.position = placeObject.transform.position;
                    if (placeObject.tag == "Walkway")
                    {
                        Debug.Log("paint descended");
                        Vector3 descend = new Vector2(0, -.18f);
                        Vector3 size = new Vector2(0, -1.5f);
                        childObject.transform.position += descend;
                        childObject.transform.localScale += size;
                    }
                    else if (placeObject.tag == "MovingPlatform")
                    {
                        Vector3 raise = new Vector2(0, .76f);
                        childObject.transform.localPosition += raise;
                    }
                }
               // childObject.transform.position = placeObject.transform.position;
                orderinlayer++;
                childObject.GetComponent<SpriteRenderer>().sortingOrder = orderinlayer;
                PlayerInventory.RemoveItem(currentValue);
            }
            else if (!ObjectObstruction(placeObject))
            {
                childObject = Instantiate(Resources.Load("ypFloor")) as GameObject;
                childObject.transform.parent = placeObject.transform;
                Debug.Log("yellow paint created!");
                if (player.GetComponent<PlayerMovement>().upsideDown)
                {
                    childObject.transform.position = placeObject.transform.position;
                    //Vector3 lower = new Vector2(0, -1.0f);
                    //childObject.transform.position += lower;
                    childObject.transform.Rotate(0, 0, 180);
                    if (placeObject.tag == "Walkway")
                    {
                        Debug.Log("paint descended");
                        Vector3 descend = new Vector2(0, -.18f);
                        Vector3 size = new Vector2(0, -1.5f);
                        childObject.transform.position += descend;
                        childObject.transform.localScale += size;
                    }
                    else if(placeObject.tag == "MovingPlatform")
                    {
                        Vector3 raise = new Vector2(0, .76f);
                        childObject.transform.localPosition += raise;
                    }
                }
                else if (!player.GetComponent<PlayerMovement>().upsideDown)
                {
                    childObject.transform.position = placeObject.transform.position;
                    if (placeObject.tag == "Walkway")
                    {
                        Debug.Log("paint descended");
                        Vector3 descend = new Vector2(0, -.18f);
                        Vector3 size = new Vector2(0, -1.5f);
                        childObject.transform.position += descend;
                        childObject.transform.localScale += size;
                    }
                    else if (placeObject.tag == "MovingPlatform")
                    {
                        Vector3 raise = new Vector2(0, .76f);
                        childObject.transform.localPosition += raise;
                    }
                }
                //childObject.transform.position = placeObject.transform.position;
                orderinlayer++;
                childObject.GetComponent<SpriteRenderer>().sortingOrder = orderinlayer;
                PlayerInventory.RemoveItem(currentValue);
                placeSource.PlayOneShot(placeSound);
            }
        }
    }

    public GameObject FindClosestObject()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Object");
        List<GameObject> unmovable = new List<GameObject>();
        foreach(GameObject crate in gos)
        {
            if(crate.GetComponent<ObjectMovement>().positionLocked)
            {
                unmovable.Add(crate);
            }
        }
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in unmovable)
        {
            Vector3 diff = go.transform.position - player.transform.position;
            float curDistance = diff.magnitude;
            if ((Mathf.Abs(curDistance) < Mathf.Abs(distance)) && Mathf.Abs(curDistance) < 3)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    public GameObject FindClosestGround()
    {
        string[] placeTags = { "Ground", "Walkway", "MovingPlatform" };
        List<GameObject> gos = new List<GameObject>();
        foreach(string tag in placeTags)
        {
            GameObject[] find = GameObject.FindGameObjectsWithTag(tag);
            foreach(GameObject place in find)
            {
                gos.Add(place);
            }
        }
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        Vector3 dist = new Vector3(0,0,0);
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - player.transform.position;
            float curDistance = diff.magnitude;
            if (player.GetComponent<PlayerMovement>().upsideDown)
            {
                if (player.GetComponent<PlayerMovement>().isFacingRight)
                {
                    if (diff.x > 0 && diff.x < 2 && diff.y > 0 && diff.y < 2)
                    {
                        if ((Mathf.Abs(curDistance) < Mathf.Abs(distance)) && Mathf.Abs(curDistance) > 1.5)
                        {
                            closest = go;
                            dist = closest.transform.position - player.transform.position;
                            distance = curDistance;
                        }
                    }
                }
                else if (!player.GetComponent<PlayerMovement>().isFacingRight)
                {
                    if (diff.x < 0 && diff.x > -2 && diff.y > 0 && diff.y < 2)
                    {
                        if ((Mathf.Abs(curDistance) < Mathf.Abs(distance)) && Mathf.Abs(curDistance) > 1.5)
                        {
                            closest = go;
                            dist = closest.transform.position - player.transform.position;
                            distance = curDistance;
                        }
                    }
                }
            }
            else if(!player.GetComponent<PlayerMovement>().upsideDown)
            {
                if (player.GetComponent<PlayerMovement>().isFacingRight)
                {
                    if (diff.x > 0 && diff.x < 2 && diff.y < 0 && diff.y > -2)
                    {
                        if ((Mathf.Abs(curDistance) < Mathf.Abs(distance)) && Mathf.Abs(curDistance) > 1.5)
                        {
                            closest = go;
                            dist = closest.transform.position - player.transform.position;
                            distance = curDistance;
                        }
                    }
                }
                else if (!player.GetComponent<PlayerMovement>().isFacingRight)
                {
                    if (diff.x < 0 && diff.x > -2 && diff.y < 0 && diff.y > -2)
                    {
                        if ((Mathf.Abs(curDistance) < Mathf.Abs(distance)) && Mathf.Abs(curDistance) > 1.5)
                        {
                            closest = go;
                            dist = closest.transform.position - player.transform.position;
                            distance = curDistance;
                        }
                    }
                }
            }            
        }
        /*Debug.Log("Difference:" + dist);
        if(closest == null)
        {
            Debug.Log("Could not find suitable object.");
        }
        */
        return closest;
    }

    public GameObject FindClosestSpinner()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Spinner");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - player.transform.position;
            float curDistance = diff.magnitude;
            if ((Mathf.Abs(curDistance) < Mathf.Abs(distance)) && Mathf.Abs(curDistance) < 5)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    public bool ObjectObstruction(GameObject test)
    {
        Vector3 up = new Vector2(0, 1.0f);
        if (!player.GetComponent<PlayerMovement>().upsideDown)
        {
            if (Physics2D.Linecast(test.transform.position, test.transform.position + up, 1 << 0))
            {
                Debug.Log("There is an object in the way.");
                return true;
            }
        }
        else if(player.GetComponent<PlayerMovement>().upsideDown)
        {
            if (Physics2D.Linecast(test.transform.position, test.transform.position - up, 1 << 0))
            {
                Debug.Log("There is an object in the way.");
                return true;
            }
        }
        return false;
    }
}
