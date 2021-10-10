using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemIconHandler : MonoBehaviour
{
    [SerializeField] private GameObject iconPartPrefab;
    [SerializeField] private GameObject iconHolder;
    private Item currentItem;
    private List<GameObject> iconParts;

    public void InitItemIconHandler(Item item)
    {
        iconParts = new List<GameObject>();
        foreach(Transform child in iconHolder.transform)
        {
            Destroy(child.gameObject);
        }

        currentItem = item;
        if (currentItem == null)
            return;

        if (currentItem.itemIcon != null)
        {
            for (int i = 0; i < currentItem.itemIcon.Count; i++)
            {
                IconPartWithShadow iconPart = currentItem.itemIcon[i];
                GameObject gO = Instantiate(iconPartPrefab, iconHolder.transform);
                Image iconImage = gO.transform.GetChild(0).GetComponent<Image>();
                if (iconPart.spritePart != null)
                {
                    iconImage.enabled = true;
                    iconImage.sprite = iconPart.spritePart;
                    iconImage.color = iconPart.partColor;
                }
                else
                {
                    iconImage.enabled = false;
                }

                Image iconShadow = gO.transform.GetChild(1).GetComponent<Image>();
                if (iconPart.spritePartShadow != null)
                {
                    iconShadow.enabled = true;
                    iconShadow.sprite = iconPart.spritePartShadow;
                }
                else
                {
                    iconShadow.enabled = false;
                }
                iconParts.Add(gO);
            }
        }
    }

    public void SetIconTransparency(float tVal)
    {
        foreach(GameObject iconPart in iconParts)
        {
            foreach(Image img in iconPart.GetComponentsInChildren<Image>())
            {
                Color clr = img.color;
                img.color = new Color(clr.r, clr.g, clr.b, tVal);
            }
        }
    }

    public void IconSetActive(bool setActive)
    {
        iconHolder.SetActive(setActive);
    }
}
