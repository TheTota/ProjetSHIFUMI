﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimpleTranslator : MonoBehaviour
{
    [SerializeField] private string FR;
    [SerializeField] private string EN;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            UpdateText();
        }
    }

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            UpdateText();
        }
    }

    private void UpdateText()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        if (GameManager.Instance.Language == Lang.FR)
        {
            text.text = FR;
        }
        else if (GameManager.Instance.Language == Lang.EN)
        {
            text.text = EN;
        }
    }
}
