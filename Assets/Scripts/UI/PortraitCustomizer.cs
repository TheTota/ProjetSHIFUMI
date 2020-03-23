﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Allows the player to customize his commander with the unlocked portrait elements.
/// </summary>
public class PortraitCustomizer : MonoBehaviour
{
    [SerializeField]
    private PortraitRenderer portraitRenderer;

    [SerializeField]
    private GameObject mainMenuUI;

    // buttons references to disable them if only 1 available hair/eyes/mouth
    [SerializeField]
    private Button prevHairBtn;
    [SerializeField]
    private Button prevEyesBtn;
    [SerializeField]
    private Button prevMouthBtn;
    [SerializeField]
    private Button nextHairBtn;
    [SerializeField]
    private Button nextEyesBtn;
    [SerializeField]
    private Button nextMouthBtn;

    // browsing indexes
    private int currentHairIndex;
    private int currentEyesIndex;
    private int currentMouthIndex;

    private void OnEnable()
    {
        RenderPlayerPortrait();

        // init browsing indexes
        Portrait p = GameManager.Instance.Player.Portrait;
        currentHairIndex = Array.IndexOf(PortraitGenerator.Instance.GetUnlockedHair(), p.Hair);
        currentEyesIndex = Array.IndexOf(PortraitGenerator.Instance.GetUnlockedEyes(), p.Eyes);
        currentMouthIndex = Array.IndexOf(PortraitGenerator.Instance.GetUnlockedMouth(), p.Mouth);

        HandleButtonsAvailabilityBasedOnUnlockedElements();
    }

    /// <summary>
    /// Enables/disable the interactivity with the customization buttons, depending on the amound of unlocked elements.
    /// </summary>
    private void HandleButtonsAvailabilityBasedOnUnlockedElements()
    {
        // disable hair buttons if only 1 unlocked hair
        if (PortraitGenerator.Instance.GetUnlockedHair().Length <= 1)
        {
            nextHairBtn.interactable = false;
            prevHairBtn.interactable = false;
        }
        else
        {
            nextHairBtn.interactable = true;
            prevHairBtn.interactable = true;
        }

        // disable eyes buttons if only 1 unlocked eyes
        if (PortraitGenerator.Instance.GetUnlockedEyes().Length <= 1)
        {
            nextEyesBtn.interactable = false;
            prevEyesBtn.interactable = false;
        }
        else
        {
            nextEyesBtn.interactable = true;
            prevEyesBtn.interactable = true;
        }

        // disable mouth buttons if only 1 unlocked mouth
        if (PortraitGenerator.Instance.GetUnlockedMouth().Length <= 1)
        {
            nextMouthBtn.interactable = false;
            prevMouthBtn.interactable = false;
        }
        else
        {
            nextMouthBtn.interactable = true;
            prevMouthBtn.interactable = true;
        }
    }

    /// <summary>
    /// Renders the player portrait in the customization popup.
    /// </summary>
    public void RenderPlayerPortrait()
    {
        portraitRenderer.RenderPortrait(GameManager.Instance.Player);
    }

    /// <summary>
    /// Saves the modified portrait and closes the popup to bring back the main menu with the commanders grid.
    /// </summary>
    public void ClosePopup()
    {
        // Save portrait
        Portrait p = GameManager.Instance.Player.Portrait;
        PlayerPrefs.SetInt("player_portrait_hair", Array.IndexOf(PortraitGenerator.Instance.availableHair, p.Hair));
        PlayerPrefs.SetInt("player_portrait_eyes", Array.IndexOf(PortraitGenerator.Instance.availableEyes, p.Eyes));
        PlayerPrefs.SetInt("player_portrait_mouth", Array.IndexOf(PortraitGenerator.Instance.availableMouth, p.Mouth));

        // Go back to menu
        mainMenuUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Goes to the next available hair : sets it for the player and renders it.
    /// </summary>
    public void NextHair()
    {
        GameManager.Instance.Player.Portrait.Hair = GetNextElement(PortraitGenerator.Instance.GetUnlockedHair(), ref currentHairIndex);
    }

    /// <summary>
    /// Goes to the previous available hair : sets it for the player and renders it.
    /// </summary>
    public void PrevHair()
    {
        GameManager.Instance.Player.Portrait.Hair = GetPreviousElement(PortraitGenerator.Instance.GetUnlockedHair(), ref currentHairIndex);
    }

    /// <summary>
    /// Goes to the next available eyes : sets it for the player and renders it.
    /// </summary>
    public void NextEyes()
    {
        GameManager.Instance.Player.Portrait.Eyes = GetNextElement(PortraitGenerator.Instance.GetUnlockedEyes(), ref currentEyesIndex);
    }

    /// <summary>
    /// Goes to the previous available eyes : sets it for the player and renders it.
    /// </summary>
    public void PrevEyes()
    {
        GameManager.Instance.Player.Portrait.Eyes = GetPreviousElement(PortraitGenerator.Instance.GetUnlockedEyes(), ref currentEyesIndex);
    }

    /// <summary>
    /// Goes to the next available mouth : sets it for the player and renders it.
    /// </summary>
    public void NextMouth()
    {
        GameManager.Instance.Player.Portrait.Mouth = GetNextElement(PortraitGenerator.Instance.GetUnlockedMouth(), ref currentMouthIndex);
    }

    /// <summary>
    /// Goes to the previous available mouth : sets it for the player and renders it.
    /// </summary>
    public void PrevMouth()
    {
        GameManager.Instance.Player.Portrait.Mouth = GetPreviousElement(PortraitGenerator.Instance.GetUnlockedMouth(), ref currentMouthIndex);
    }

    /// <summary>
    /// Returns the next element of the given list, based on the given index.
    /// </summary>
    /// <param name="unlockedElements">List of portrait elements, example: the list of unlocked hair.</param>
    /// <param name="index">Index used to browse</param>
    /// <returns>The next portrait element in the list.</returns>
    private PortraitElement GetNextElement(PortraitElement[] unlockedElements, ref int index)
    {
        PortraitElement elementToChange;
        if (index + 1 < unlockedElements.Length) // if we're browsing the available cuts and havent reached end of the list
        {
            index++;
            elementToChange = unlockedElements[index];
        }
        else // if we reached the end of list, go back to the first index
        {
            index = 0;
            elementToChange = unlockedElements[index];
        }

        return elementToChange;
    }

    /// <summary>
    /// Returns the previous element of the given list, based on the given index.
    /// </summary>
    /// <param name="unlockedElements">List of portrait elements, example: the list of unlocked hair.</param>
    /// <param name="index">Index used to browse</param>
    /// <returns>The previous portrait element in the list.</returns>
    private PortraitElement GetPreviousElement(PortraitElement[] unlockedElements, ref int index)
    {
        PortraitElement elementToChange;
        if (index - 1 >= 0) // if we're browsing the available cuts and havent reached end of the list
        {
            index--;
            elementToChange = unlockedElements[index];
        }
        else // if we reached the end of list, go back to the first index
        {
            index = unlockedElements.Length - 1;
            elementToChange = unlockedElements[index];
        }

        return elementToChange;
    }

}