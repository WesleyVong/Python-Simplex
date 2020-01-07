using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldHandler : MonoBehaviour
{
    InputBoxGenerator ibg;
    InputField field;
    public int num;
    // Start is called before the first frame update
    void Start()
    {
        field = GetComponent<InputField>();
        ibg = transform.parent.gameObject.GetComponent<InputBoxGenerator>();
        field.onEndEdit.AddListener(delegate { ibg.InputEvent(num); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
