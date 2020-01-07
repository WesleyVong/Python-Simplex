using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipHandler : MonoBehaviour
{
    public string text;
    public int xShift;
    public int yShift;
    private Vector3 mousePosition;
    // Start is called before the first frame update
    void Start()
    {
        SetText(text);
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Input.mousePosition;
        //mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = (Vector2)mousePosition + new Vector2(xShift,yShift);
    }
    public void SetText(string text)
    {
        this.GetComponent<Text>().text = text;
    }
    public void Clear()
    {
        this.GetComponent<Text>().text = "";
    }
}
