using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class Keyboard : MonoBehaviour
{
    public TextMeshProUGUI name;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickAdd()
    {
        name.text += EventSystem.current.currentSelectedGameObject.name.ToString();

    }
    public void OnClickDelete()
    {
        if(name.text.Length > 0)
        {
            name.text = name.text.Substring(0, name.text.Length - 1);
        }
    }
}
