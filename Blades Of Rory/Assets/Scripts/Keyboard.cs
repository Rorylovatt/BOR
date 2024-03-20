using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class Keyboard : MonoBehaviour
{
    public TextMeshProUGUI nameText, scoreText;
    public int score;
    public Button selectFirst;
    public bool active;
    // Start is called before the first frame update
    void Start()
    {
        selectFirst.Select();

    }

    // Update is called once per frame
    void Update()
    {
        if (score > 1000)
        {
            scoreText.text = "Score : " + score.ToString().Substring(0, score.ToString().Length - 3) + "." + score.ToString().Substring(score.ToString().Length - 3, 3);
        }
    }
    public void OnClickAdd()
    {
        if(nameText.text.Length < 12)
        {
            nameText.text += EventSystem.current.currentSelectedGameObject.name.ToString();
        }

    }
    public void OnClickDelete()
    {
        if(nameText.text.Length > 0)
        {
            nameText.text = nameText.text.Substring(0, nameText.text.Length - 1);
        }
    }
}
