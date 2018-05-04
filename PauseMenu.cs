using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

    public GameObject pauseButton, pausePanel, player, _menu, _left, _right, _place, _paints;
    public Inventory _inventory;
    //public Timer gametime;
    public List<GameObject> allFloorPaints = new List<GameObject>();
    public List<GameObject> CopyallFloorPaints = new List<GameObject>();
    private List<Transform> allCratesTransforms = new List<Transform>();
    private List<Vector3> allCratesPosition = new List<Vector3>();
    private GameObject[] allCrates;
    public MovingPlatform platform;
    //private GameObject[] tutorials;
    private GameObject[] signs;

    void Start()
    {
        pausePanel.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        allCrates = GameObject.FindGameObjectsWithTag("Object");
        //tutorials = GameObject.FindGameObjectsWithTag("Tutorial");
        signs = GameObject.FindGameObjectsWithTag("Sign");
    }

    public void PauseGame()
    {
        foreach(GameObject sign in signs)
        {
            sign.GetComponent<SignScript>().tutorialUI.SetActive(false);
        }
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        _menu.SetActive(false);
        _left.SetActive(false);
        _right.SetActive(false);
        _place.SetActive(false);
        _paints.SetActive(false);
        Debug.Log("Game paused");
    }

    public void ContinueGame()
    {
        foreach(GameObject s in signs)
        {
            if(s.GetComponent<SignScript>().isActivated)
            {
                s.GetComponent<SignScript>().tutorialUI.SetActive(true);
            }
        }
        pausePanel.SetActive(false);
        _menu.SetActive(true);
        _left.SetActive(true);
        _right.SetActive(true);
        _place.SetActive(true);
        _paints.SetActive(true);
        Time.timeScale = 1;
        Debug.Log("Game resumed");
    }

    public void RestartCP()
    {
        GameObject cp = GetNearestActiveCheckpoint();
        if (cp != null)
        {
            _inventory.Clear();
            allFloorPaints.Clear();
            foreach (GameObject BluePaint in GameObject.FindGameObjectsWithTag("BluePaint"))
            {
                allFloorPaints.Add(BluePaint);
            }
            foreach (GameObject YellowPaint in GameObject.FindGameObjectsWithTag("YellowPaint"))
            {
                allFloorPaints.Add(YellowPaint);
            }
            foreach (GameObject WhitePaint in GameObject.FindGameObjectsWithTag("WhitePaint"))
            {
                allFloorPaints.Add(WhitePaint);
            }
            if (player.GetComponent<PlayerMovement>().upsideDown)
            {
                //reset gravity to normal
                player.GetComponent<Rigidbody2D>().gravityScale = 1;
                player.transform.Rotate(0, 180, 180);
                player.GetComponent<PlayerMovement>().upsideDown = false;
            }

            player.transform.position = cp.transform.position;
            foreach(GameObject reactivate in cp.GetComponent<Checkpoint>().unusedPaints)
            {
                reactivate.SetActive(true);
            }
            foreach(GameObject compare in allFloorPaints)
            {
                foreach(GameObject same in cp.GetComponent<Checkpoint>().CPFloorPaints)
                {
                    if(compare == same)
                    {
                        CopyallFloorPaints.Add(compare);
                    }
                }
            }
            foreach(GameObject button in cp.GetComponent<Checkpoint>().unpressedbuttons)
            {
                if(button.GetComponent<ButtonScript>() != null)
                {
                    button.GetComponent<ButtonScript>().pressed = false;
                }
                else if(button.GetComponent<HoldButton>() != null)
                {
                    button.GetComponent<HoldButton>().pressed = false;
                }
                else if(button.GetComponent<ButtonRepeat>() != null)
                {
                    button.GetComponent<ButtonRepeat>().pressed = false;
                }
            }
            foreach(GameObject _Crate in allCrates)
            {

                    Debug.Log("Positon locked!");
                    _Crate.GetComponent<ObjectMovement>().positionLocked = true;
                    if(_Crate.GetComponent<ObjectMovement>().upsideDown)
                    {
                    _Crate.GetComponent<ObjectMovement>().upsideDown = false;
                    _Crate.GetComponent<Rigidbody2D>().gravityScale *= -1;
                    }   
            }
            int i = 0;
            foreach(Transform _crateTrans in cp.GetComponent<Checkpoint>().cratesTransforms)
            {
                _crateTrans.localPosition = cp.GetComponent<Checkpoint>().cratePositions[i];
                i++;
            }
            foreach(GameObject delete in CopyallFloorPaints)
            {
                allFloorPaints.Remove(delete);
            }
            foreach(GameObject destruction in allFloorPaints)
            {
                Destroy(destruction);
            }
            GameObject[] Gates = GameObject.FindGameObjectsWithTag("Gate");
            foreach (GameObject gate in Gates)
            {
                gate.GetComponent<Door>().isClosed = true;
            }
            foreach(GameObject door in GameObject.FindGameObjectsWithTag("GarageDoor"))
            {
                bool open = false;
                foreach(GameObject check in cp.GetComponent<Checkpoint>().openBois)
                {
                    if(door == check)
                    {
                        if (door.GetComponent<GarageDoor>() != null)
                        {
                            door.GetComponent<GarageDoor>().isClosed = false;
                            open = true;
                            break;
                        }
                        else if(door.GetComponent<BackwardsGarageDoor>() != null)
                        {
                            door.GetComponent<BackwardsGarageDoor>().isClosed = false;
                            open = true;
                            break;
                        }
                    }
                }
                if(!open)
                {
                    if (door.GetComponent<GarageDoor>() != null)
                    {
                        door.GetComponent<GarageDoor>().isClosed = true;
                    }
                    else if(door.GetComponent<BackwardsGarageDoor>() != null)
                    {
                        door.GetComponent<BackwardsGarageDoor>().isClosed = true;
                    }
                }
            }
            foreach(GameObject e in GameObject.FindGameObjectsWithTag("Elevator"))
            {
                e.GetComponent<PlatformStage>().position = e.GetComponent<PlatformStage>().startingPosition;
            }
            int j = 0;
            foreach (Transform eTrans in cp.GetComponent<Checkpoint>().elevatorTransforms)
            {
                eTrans.localPosition = cp.GetComponent<Checkpoint>().elevatorPositions[j];
                j++;
            }
            if(platform != null)
            {
                platform.reset = true;
            }
            
            //gametime.SetTime();
            ContinueGame();
        }
        else
        {
            Restart();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    GameObject GetNearestActiveCheckpoint()
    {
        GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        GameObject nearestCheckpoint = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject checkpoint in checkpoints)
        {
            Vector3 checkpointPosition = checkpoint.transform.position;
            float distance = (checkpointPosition - transform.position).sqrMagnitude;

            checkpoint.GetComponent<Checkpoint>();

            if (distance < shortestDistance && checkpoint.GetComponent<Checkpoint>().isTriggered)
            {
                nearestCheckpoint = checkpoint;
                shortestDistance = distance;
            }
        }
        return nearestCheckpoint;
    }
}
