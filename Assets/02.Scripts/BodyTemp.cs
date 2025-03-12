using TMPro;
using UnityEngine;

public class BodyTemp : MonoBehaviour
{
    public float curTemp;
    public float startTemp;
    public float maxTemp;
    public float passiveTemp;
    public TextMeshProUGUI tempText;

    private void Start()
    {
        curTemp = startTemp;
    }

    private void Update()
    {
        tempText.text = curTemp.ToString("F2") + "��C"; ;

        //if() �߰ſ� ��
        //Hot();

        //if() �߿� ��
        //Cold();
    }

    public void Hot()
    {
        curTemp = Mathf.Min(curTemp + (passiveTemp * Time.deltaTime), maxTemp);        
    }

    public void Cold()
    {
        curTemp = Mathf.Max(curTemp - (passiveTemp * Time.deltaTime), 0f);
    }
}
