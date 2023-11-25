using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public PlayerMovement player;

    public GameObject meleePanel;
    public GameObject rangedPanel;

    public Button chargedButton;
    public Button ultimateButton;


    //This is only temporary until a full inventory system is made
    public Inventory inventory;

    public GameObject inventoryPanel;
    public GameObject pausePanel;
    public GameObject actions;
    public GameObject meleeEQ;
    public GameObject rangedEQ;

    public TextMeshProUGUI potionCount;

    public bool paused = false;
    public bool melee = true;

    private void Start()
    {
        Time.timeScale = 1;
    }

    private void Update()
    {
        player.melee = melee;

        if(player.canUseCharged)
        {
            chargedButton.interactable = true;
        }
        else
        {
            chargedButton.interactable = false;
        }

        if(player.ultimateReady)
        {
            ultimateButton.interactable = true;
        }
        else
        {
            ultimateButton.interactable = false;
        }

        if (melee)
        {
            meleePanel.SetActive(true);
            rangedPanel.SetActive(false);
            meleeEQ.SetActive(true);
            rangedEQ.SetActive(false);
        }
        else
        {
            rangedPanel.SetActive(true);
            meleePanel.SetActive(false);
            meleeEQ.SetActive(false);
            rangedEQ.SetActive(true);
        }

        potionCount.text = inventory.items[0].itemCount.ToString();
    }

    public void Switch()
    {
        if (melee)
        {
            
            melee = false;
        }
        else
        {
            melee = true;
        }
    }

    //This is only temporary until a system is made
    public void UseHealth()
    {
        if (inventory.items[0].itemCount > 0)
        {
            player.gameObject.GetComponent<AttributeManager>().Heal(inventory.items[0].amount);
            inventory.items[0].itemCount -= 1;
            actions.SetActive(false);
        }
    }

    public void ShowHealth()
    {
        actions.SetActive(true);
    }

    public void ShowInventory()
    {
        inventoryPanel.SetActive(true);
    }
    public void HideInventory()
    {
        inventoryPanel.SetActive(false);
    }

    public void ShowPause()
    {
        pausePanel.SetActive(true);
    }
    public void HidePause()
    {
        pausePanel.SetActive(false);
    }

    public void TogglePause()
    {
        paused = !paused;

        if (paused)
        {
            Time.timeScale = 0;
            ShowPause();
        }
        else
        {
            Time.timeScale = 1;
            HidePause();
        }
    }

}
