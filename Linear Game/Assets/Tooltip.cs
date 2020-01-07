using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    Vector2 localPoint;
    public GameObject tooltip;
    public GameObject vehicle;
    public int slots;
    private GameObject[] vehicles;
    private float increments;
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
    void Awake()
    {
        this.vehicles = new GameObject[slots];
        this.width = this.GetComponent<RectTransform>().rect.width;
        this.height = this.GetComponent<RectTransform>().rect.height;
        this.XMin = this.GetComponent<RectTransform>().rect.xMin;
        this.XMax = this.GetComponent<RectTransform>().rect.xMax;
        this.YMin = this.GetComponent<RectTransform>().rect.yMin;
        this.YMax = this.GetComponent<RectTransform>().rect.yMax;
        this.Pos = this.GetComponent<RectTransform>().rect.position;
        this.increments = this.width / this.slots;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!inhibit)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this.GetComponent<RectTransform>(), Input.mousePosition, null, out localPoint))
            {
                if (localPoint.x > XMin && localPoint.x < XMax
                    && localPoint.y > YMin + height / 2 && localPoint.y < YMax)
                {
                    correctedPos = new Vector2(localPoint.x + width / 2, localPoint.y + height / 2);
                    tooltip.GetComponent<TooltipHandler>().SetText(((int)(correctedPos.x / increments)).ToString());
                    hasExited = false;
                }
                else if (!hasExited)
                {
                    tooltip.GetComponent<TooltipHandler>().Clear();
                    hasExited = true;
                }
            }
        }
        
    }
    public void Inhibit(bool inh)
    {
        inhibit = inh;
    }
    public void Summon(int slot)
    {
        this.vehicles[slot] = Instantiate(vehicle, this.transform);
        this.vehicles[slot].transform.localPosition = new Vector2((XMin + increments/2) + increments * slot, YMax * 0.6f);
    }
    public void Summon(int slot, float chargeValue)
    {
        this.vehicles[slot] = Instantiate(vehicle, this.transform);
        this.vehicles[slot].transform.localPosition = new Vector2((XMin + increments / 2) + increments * slot, YMax * 0.6f);
        this.vehicles[slot].GetComponent<VehicleTooltip>().chargeLeft = chargeValue;
        this.vehicles[slot].GetComponent<VehicleTooltip>().originalCharge = chargeValue;
    }
    public void ChangeValue(int slot, float chargeValue)
    {
        try
        {
            this.vehicles[slot].GetComponent<VehicleTooltip>().chargeLeft = chargeValue;
        } catch
        {
            //Debug.Log(string.Format("No vehicle at slot {0}", slot));
        }
        
    }
    public void Remove(int slot)
    {
        Destroy(this.vehicles[slot]);
    }
    
}
