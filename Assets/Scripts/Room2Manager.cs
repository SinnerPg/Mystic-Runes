using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Room2Manager : MonoBehaviour
{
    private int step = 1;
    private int highlighted = 0;
    private bool startControl = false, timelinePlayed = false;
    [Header("Down Section")]
    public MeshRenderer[] tilesRenderer;
    public CapsuleCollider[] tilesColliders;
    public AudioSource[] sounds;
    public BoxCollider triggerElevator;
    public Animator platform;
    private GameObject player;
    [Header("Up Section")]
    public GameObject[] platformHighlights;
    public GameObject[] platformColliders;
    [Header("Cinematics")]
    public GameObject[] timelines;

    void Awake()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if(startControl)
        {
            if(highlighted == 7)
            {
                if(!timelinePlayed)
                {
                    timelines[2].SetActive(true);
                    timelinePlayed = true;
                }
            }
        }
    }
    public void checkTile(int tile)
    {
        switch(step)
        {
            case 1:
                if(tile == 3)
                {
                    tilesRenderer[2].enabled = true;
                    tilesColliders[2].enabled = false;
                    step++;
                }
                break;
            case 2:
                if(tile == 7)
                {
                    tilesRenderer[6].enabled = true;
                    tilesColliders[6].enabled = false;
                    step++;
                }
                else
                {
                    refreshRenderer();
                    sounds[0].Play();
                    step = 1;
                }
                break;
            case 3:
                if(tile == 13)
                {
                    tilesRenderer[12].enabled = true;
                    tilesColliders[12].enabled = false;
                    step++;
                }
                else
                {
                    refreshRenderer();
                    sounds[0].Play();
                    step = 1;
                }
                break;
            case 4:
                if(tile == 18)
                {
                    tilesRenderer[17].enabled = true;
                    tilesColliders[17].enabled = false;
                    step++;
                }
                else
                {
                    refreshRenderer();
                    sounds[0].Play();
                    step = 1;
                }
                break;
            case 5:
                if(tile == 22)
                {
                    tilesRenderer[21].enabled = true;
                    tilesColliders[21].enabled = false;
                    step++;
                    timelines[0].SetActive(true);
                    refreshRenderer();
                }
                else
                {
                    refreshRenderer();
                    sounds[0].Play();
                    step = 1;
                }
                break;
            case 6:
                if(tile == 7)
                {
                    tilesRenderer[6].enabled = true;
                    tilesColliders[6].enabled = false;
                    step++;
                }
                break;
            case 7:
                if(tile == 9)
                {
                    tilesRenderer[8].enabled = true;
                    tilesColliders[8].enabled = false;
                    step++;
                }
                else
                {
                    refreshRenderer();
                    sounds[0].Play();
                    step = 6;
                }
                break;
            case 8:
                if(tile == 13)
                {
                    tilesRenderer[12].enabled = true;
                    tilesColliders[12].enabled = false;
                    step++;
                }
                else
                {
                    refreshRenderer();
                    sounds[0].Play();
                    step = 6;
                }
                break;
            case 9:
                if(tile == 16)
                {
                    tilesRenderer[15].enabled = true;
                    tilesColliders[15].enabled = false;
                    step++;
                }
                else
                {
                    refreshRenderer();
                    sounds[0].Play();
                    step = 6;
                }
                break;
            case 10:
                if(tile == 22)
                {
                    tilesRenderer[21].enabled = true;
                    tilesColliders[21].enabled = false;
                    step++;
                    timelines[1].SetActive(true);
                    refreshRenderer();
                }
                else
                {
                    refreshRenderer();
                    sounds[0].Play();
                    step = 6;
                }
                break;
            default: 
                break;
        }
    }

    public void goUp()
    {
        triggerElevator.enabled = false;
        platform.enabled = true;
        startControl = true;
    }

    public void lightPlatform(int platform)
    {
        switch(platform)
        {
            case 1:
                platformHighlights[0].SetActive(true);
                platformColliders[0].SetActive(false);
                highlighted++;
                break;
            case 2:
                platformHighlights[1].SetActive(true);
                platformColliders[1].SetActive(false);
                highlighted++;
                break;
            case 3:
                platformHighlights[2].SetActive(true);
                platformColliders[2].SetActive(false);
                highlighted++;
                break;
            case 4:
                platformHighlights[3].SetActive(true);
                platformColliders[3].SetActive(false);
                highlighted++;
                break;
            case 5:
                platformHighlights[4].SetActive(true);
                platformColliders[4].SetActive(false);
                highlighted++;
                break;
            case 6:
                platformHighlights[5].SetActive(true);
                platformColliders[5].SetActive(false);
                highlighted++;
                break;
            case 7:
                platformHighlights[6].SetActive(true);
                platformColliders[6].SetActive(false);
                highlighted++;
                break;
        }
    }

    public void respawn()
    {
        player.transform.position = new Vector3(-0.94f, 14, 13.6f);
    }
    private void refreshRenderer()
    {
        for(int i = 0; i < tilesRenderer.Length; i++)
        {
            tilesRenderer[i].enabled = false;
            tilesColliders[i].enabled = true;
        }
    }

    public void endgame()
    {
        SceneManager.LoadScene("002");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
