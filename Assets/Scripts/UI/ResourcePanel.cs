using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourcePanel : MonoBehaviour
{
    public GameObject goldPanel;

    private void Start()
    {
        GuildMain.Instance.GuildResourceChanged += UpdateResources;
        UpdateResources();
    }

    public void UpdateResources()
    {
        TextMeshProUGUI textGold = goldPanel.GetComponent<TextMeshProUGUI>();
        textGold.text = GuildMain.Instance.gold.ToString();
    }
}
