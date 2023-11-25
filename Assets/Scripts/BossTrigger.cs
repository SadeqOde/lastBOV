using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public PlayerMovement player;
    public Camera mainCamera;

    public GameObject trigger;

    public GameObject bossEnemy;
    public GameObject fightAnim;

    public GameObject ui1;
    public GameObject ui2;
    public GameObject ui3;

    public AudioSource mainMusic;
    public AudioSource bossMusic;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fightAnim.SetActive(true);
            trigger.SetActive(false);
            mainMusic.Pause();
            bossMusic.Play();
        }
    }


    public void StartCutscene()
    {
        player.canMove = false;
        ui1.SetActive(false);
        ui2.SetActive(false);
        ui3.SetActive(false);
        mainCamera.gameObject.SetActive(false);
    }

    public void EndCutscene()
    {
        mainCamera.gameObject.SetActive(true);
        ui1.SetActive(true);
        ui2.SetActive(true);
        ui3.SetActive(true);
        player.canMove = true;
        bossEnemy.SetActive(true);
    }
}
