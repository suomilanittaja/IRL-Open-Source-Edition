using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDis : MonoBehaviour
{

    public GameObject button;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void disable()
    { button.SetActive(false); }
}
