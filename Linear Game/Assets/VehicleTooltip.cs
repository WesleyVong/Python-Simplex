using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VehicleTooltip : MonoBehaviour
{
    public GameObject tooltip;
    public Tooltip[] tooltips;
    public Text chargeText;
    public float carNum = 0;
    public float chargeLeft = 0;
    public float originalCharge = 0;
    Vector2 localPoint;
    private float width;
    private float height;
    private float XMin;
    private float XMax;
    private float YMin;
    private float YMax;
    private Vector2 Pos;
    private Vector2 correctedPos;
    private bool hasExited = false;
    private bool inhibit = false;
    // Start is called before the first frame update
    void Start()
    {
        tooltip = GameObject.Find("Tooltip");
        tooltips[0] = transform.parent.gameObject.GetComponent<Tooltip>();

        width = this.GetComponent<RectTransform>().rect.width;
        height = this.GetComponent<RectTransform>().rect.height;
        XMin = this.GetComponent<RectTransform>().rect.xMin;
        XMax = this.GetComponent<RectTransform>().rect.xMax;
        YMin = this.GetComponent<RectTransform>().rect.yMin;
        YMax = this.GetComponent<RectTransform>().rect.yMax;
        Pos = this.GetComponent<RectTransform>().rect.position;
    }

    // Update is called once per frame
    void Update()
    {
        chargeText.color = new Color(chargeLeft / originalCharge, 1 - chargeLeft / originalCharge, 0);
        chargeText.text = string.Format("{0}/{1}", chargeLeft,originalCharge);
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this.GetComponent<RectTransform>(), Input.mousePosition, null, out localPoint))
        {
            if (localPoint.x > XMin && localPoint.x < XMax
                && localPoint.y > YMin && localPoint.y < YMax)
            {
                correctedPos = new Vector2(localPoint.x + width / 2, localPoint.y + height / 2);
                tooltip.GetComponent<TooltipHandler>().SetText(string.Format("Car #{0} \nCharge Needed: {1}",carNum,chargeLeft));
                hasExited = false;
                foreach (Tooltip other in tooltips)
                {
                    other.Inhibit(true);
                }
            }
            else if (!hasExited)
            {
                tooltip.GetComponent<TooltipHandler>().Clear();
                hasExited = true;
                foreach (Tooltip other in tooltips)
                {
                    other.Inhibit(false);
                }
            }
        }
    }

    
}
