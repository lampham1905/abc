using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Great : MonoBehaviour
{
    public TextMeshProUGUI greatText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GreatLeft")
        {
            greatText.gameObject.SetActive(true);
        }
        Invoke("DeleteGreatText", .2f);

    }
    private void DeleteGreatText()
    {
        greatText.gameObject.SetActive(false);

    }
}
