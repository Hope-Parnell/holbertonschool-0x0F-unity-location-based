using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class showHideButton : MonoBehaviour
{
    [SerializeField] string hideText = "hide";
    [SerializeField] string showText = "show";
    [SerializeField] private TMP_Text button_text;
    [SerializeField] private GameObject item;
    private void Awake() {
        if (item.activeSelf)
        {
            button_text.text = hideText;
        } else {
            button_text.text = showText;
        }
    }
    public void showHide(){
        if (item.activeSelf)
        {
            item.SetActive(false);
            button_text.text = showText;
        } else {
            item.SetActive(true);
            button_text.text = hideText;
        }
    }
}
