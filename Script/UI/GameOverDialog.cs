using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverDialog : MonoBehaviour
{
    public static GameOverDialog Ove;

    private void Awake()
    {
        Ove = this;
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }
    public void close()
    {
        this.gameObject.SetActive(false);
    }
   
}
