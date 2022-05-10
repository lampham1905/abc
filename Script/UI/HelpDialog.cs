using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpDialog : MonoBehaviour
{
    public static HelpDialog Hel;

    private void Awake()
    {
        Hel = this;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void close()
    {
        this.gameObject.SetActive(false);
    }

}
