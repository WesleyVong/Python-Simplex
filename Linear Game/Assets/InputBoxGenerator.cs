using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputBoxGenerator : MonoBehaviour
{
    public const string SLOTTEXT = "Slot ";
    public const string INPUTTEXT = "User input";
    public const string COMPTEXT = "Computer Move";
    public const string DESCRIPTION = "Individual charger output: {0} amps\nTotal charger output: {1} amps";
    public const string CHARGERUSETEXT = "Used: {0}/{1} amps";
    
    public int slots = 0;
    public int yHeight = -50;
    private int yShift;
    private int slotShift;
    private int inputShift = 0;
    private int computerShift;
    public GameObject textField;
    public GameObject inputField;
    public GameObject[] inputFields;
    private GameObject chargeUsedField;
    public float[] userInputs;

    public bool Error = false;

    public float chargerStrength = 20;
    public float maximumPower = 100;
    private float totalInput = 0;
    // Start is called before the first frame update
    void Start()
    {
        yShift = (-yHeight * slots / 2) - 100;
        inputFields = new GameObject[slots];
        userInputs = new float[slots];
        slotShift = -(int)this.GetComponent<RectTransform>().rect.width / 4;
        computerShift = (int)this.GetComponent<RectTransform>().rect.width / 3;
        // Creates Headers
        GameObject header = Instantiate(textField, transform);
        Text headerText = header.GetComponent<Text>();
        headerText.text = SLOTTEXT;
        header.GetComponent<RectTransform>().localPosition = new Vector2(slotShift, yShift - yHeight);

        header = Instantiate(textField, transform);
        headerText = header.GetComponent<Text>();
        headerText.text = INPUTTEXT;
        header.GetComponent<RectTransform>().localPosition = new Vector2(inputShift, yShift - yHeight);

        header = Instantiate(textField, transform);
        headerText = header.GetComponent<Text>();
        headerText.text = COMPTEXT;
        header.GetComponent<RectTransform>().localPosition = new Vector2(computerShift, yShift - yHeight);

        header = Instantiate(textField, transform);
        headerText = header.GetComponent<Text>();
        headerText.text = string.Format(DESCRIPTION,chargerStrength,maximumPower);
        header.GetComponent<RectTransform>().localPosition = new Vector2(inputShift, yShift - yHeight * 3);

        chargeUsedField = Instantiate(textField, transform);
        chargeUsedField.GetComponent<Text>().text = string.Format(CHARGERUSETEXT, totalInput, maximumPower);
        //chargeUsedField.GetComponent<Text>().color = new Color(1 - totalInput / maximumPower, totalInput / maximumPower, 0);
        chargeUsedField.GetComponent<RectTransform>().localPosition = new Vector2(computerShift, yShift - yHeight * 2f);

        for (int i = 0; i < slots; i++)
        {
            // Creates Font
            GameObject slotObj = Instantiate(textField, transform);
            slotObj.name = string.Format("SlotName_{0}", i);
            Text text = slotObj.GetComponent<Text>();
            text.text = string.Format("Slot {0}", i);
            slotObj.GetComponent<RectTransform>().localPosition = new Vector2(slotShift, yShift + yHeight * i);

            // Creates User Input
            GameObject input = Instantiate(inputField, transform);
            input.name = string.Format("InputField_{0}", i);
            input.GetComponent<RectTransform>().localPosition = new Vector2(inputShift, yShift + yHeight * i);
            input.GetComponent<InputFieldHandler>().num = i;
            inputFields[i] = input;

            // Creates Computer Prediction List
            GameObject compObj = Instantiate(textField, transform);
            compObj.name = string.Format("Comp_{0}", i);
            Text compText = compObj.GetComponent<Text>();
            compText.text = string.Format("{0}",i);
            compObj.GetComponent<RectTransform>().localPosition = new Vector2(computerShift, yShift + yHeight * i);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void SetComp(float[] val)
    {
        GameObject obj;
        for (int i = 0; i < val.Length; i++)
        {
            obj = this.transform.Find(string.Format("Comp_{0}", i)).gameObject;
            // Changes Computer Prediction List
            Text compText = obj.GetComponent<Text>();
            compText.text = string.Format("{0}", val[i]);
        }
    }
    public void InputEvent(int slotNum)
    {
        string tmp = inputFields[slotNum].transform.Find("Text").GetComponent<Text>().text;
        //Debug.Log(tmp);
        if (tmp != "")
        {
            inputFields[slotNum].transform.Find("Text").GetComponent<Text>().color = new Color(0, 0, 0);
            float parsedVal = float.Parse(tmp);
            totalInput -= userInputs[slotNum];
            Debug.Log(totalInput);
            if (parsedVal <= chargerStrength && totalInput + parsedVal <= maximumPower)
            {
                totalInput += parsedVal;
                userInputs[slotNum] = parsedVal;
                Error = false;
            }
            else
            {
                totalInput += userInputs[slotNum];
                Debug.Log("Overload");
                inputFields[slotNum].transform.Find("Text").GetComponent<Text>().color = new Color(255,0,0);
                Error = true;
            }
            chargeUsedField.GetComponent<Text>().text = string.Format(CHARGERUSETEXT, totalInput, maximumPower);
            //chargeUsedField.GetComponent<Text>().color = new Color(1 - totalInput / maximumPower, totalInput / maximumPower, 0);
        } else if (tmp == "" && userInputs[slotNum] != 0)
        {
            
            totalInput -= userInputs[slotNum];
            userInputs[slotNum] = 0;
            chargeUsedField.GetComponent<Text>().text = string.Format(CHARGERUSETEXT, totalInput, maximumPower);
            //chargeUsedField.GetComponent<Text>().color = new Color(1 - totalInput / maximumPower, totalInput / maximumPower, 0);
        }
    }
}
