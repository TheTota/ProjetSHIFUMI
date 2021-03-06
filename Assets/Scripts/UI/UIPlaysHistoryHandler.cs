﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlaysHistoryHandler : MonoBehaviour
{
    [SerializeField]
    private BattleManager battleManager;

    [SerializeField]
    private Image playerPlaysHistoryImg;
    [SerializeField]
    private RectTransform playerPlaysParent;
    [SerializeField]
    private Image enemyPlaysHistoryImg;
    [SerializeField]
    private RectTransform enemyPlaysParent;

    [SerializeField]
    private GameObject playsHistoryItemPrefab;

    [Header("Units Icons")]
    public Sprite knightSprite;
    public Sprite shieldsSprite;
    public Sprite spearSprite;
    public Sprite mageSprite;
    public Sprite archerSprite;

    private void Start()
    {
        InitBackgroundColor();
    }

    /// <summary>
    /// Inits the color of the background of the players history panels.
    /// </summary>
    private void InitBackgroundColor()
    {
        // set background color for player
        Color playerColor = battleManager.PlayerBC.Commander.Color;
        playerPlaysHistoryImg.color = new Color(playerColor.r, playerColor.g, playerColor.b, .4f);
        // set background color for enemy
        Color enemyColor = battleManager.EnemyBC.Commander.Color;
        enemyPlaysHistoryImg.color = new Color(enemyColor.r, enemyColor.g, enemyColor.b, .4f);
    }

    /// <summary>
    /// Gathers data and calls methods to render plays history for both the player and the enemy.
    /// </summary>
    /// <param name="playerUnit">Unit picked by the player.</param>
    /// <param name="enemyUnit">Unit picked by the enemy.</param>
    /// <param name="winner">Battle commander that won the round.</param>
    public void RenderPlaysHistoryUI(UnitType playerUnit, UnitType enemyUnit, BattleCommander winner)
    {
        // Remove old items to make room
        if (playerPlaysParent.childCount == 8)
        {
            Destroy(playerPlaysParent.GetChild(7).gameObject);
            Destroy(enemyPlaysParent.GetChild(7).gameObject);
        }

        // if not a draw
        if (battleManager.PlayerBC == winner || battleManager.EnemyBC == winner)
        {
            RenderPlayHistory(playerPlaysParent, playerUnit, battleManager.PlayerBC == winner);
            RenderPlayHistory(enemyPlaysParent, enemyUnit, battleManager.EnemyBC == winner);
        }
        else // if draw
        {
            RenderPlayHistory(playerPlaysParent, playerUnit, false, true);
            RenderPlayHistory(enemyPlaysParent, enemyUnit, false, true);
        }
    }

    /// <summary>
    /// Actually renders the plays history by adding a new element to it, based on received data.
    /// </summary>
    /// <param name="historyParent"></param>
    /// <param name="ut"></param>
    /// <param name="isWinner"></param>
    private void RenderPlayHistory(RectTransform historyParent, UnitType ut, bool isWinner, bool isDraw = false)
    {
        // instantiate the history item
        GameObject item = Instantiate(playsHistoryItemPrefab, historyParent);
        item.transform.SetAsFirstSibling();

        // color depending on if win or not 
        if (isDraw)
        {
            item.GetComponent<Image>().color = new Color(.71f, .71f, .71f); // some light grey
        }
        else
        {
            if (isWinner)
            {
                item.GetComponent<Image>().color = new Color(.235f, .94f, .235f); // some green
            }
            else
            {
                item.GetComponent<Image>().color = new Color(.94f, .235f, .235f); // some pomegranate red
            }
        }

        // display the right unit 
        Image unitImg = item.transform.Find("UnitIcon").GetComponent<Image>();
        switch (ut)
        {
            case UnitType.Knights:
                unitImg.sprite = knightSprite;
                break;
            case UnitType.Shields:
                unitImg.sprite = shieldsSprite;
                break;
            case UnitType.Spearmen:
                unitImg.sprite = spearSprite;
                break;
            case UnitType.Mages:
                unitImg.sprite = mageSprite;
                break;
            case UnitType.Archers:
                unitImg.sprite = archerSprite;
                break;
            default:
                throw new Exception("ut is wrong");
        }

        // fill round text
        item.GetComponentInChildren<TextMeshProUGUI>().text = battleManager.CurrentRound.ToString();
    }
}
