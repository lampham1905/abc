using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AchivementDialog : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public static AchivementDialog Achi;


    private void Awake()
    {
        Achi = this;
    }
    public  void Show(bool showAchivementDialog)
    {
        this.gameObject.SetActive(showAchivementDialog);
        
    }
    public void close()
    {
        this.gameObject.SetActive(false);
    }
    


}
