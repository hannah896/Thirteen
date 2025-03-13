using TMPro;
using UnityEngine;

public class BodyTemp : MonoBehaviour
{
    [Header("PlayerTemp")]
    public float curTemp;
    public float startTemp;
    public float maxTemp;
    public float minTemp;
    public float passiveTemp;
    public TextMeshProUGUI tempText;

    [Header("Damage")]
    public int tempZoneDamage;
    public float damageRate;
    private float damageTimer;

    private void Start()
    {
        curTemp = startTemp;
    }

    private void Update()
    {
        tempText.text = curTemp.ToString("F2") + "¡ÆC"; ;

        damageTimer += Time.deltaTime;

        if (curTemp >= maxTemp || curTemp <= minTemp)
        {
            if (damageTimer >= damageRate)
            {
                CharacterManager.Instance.Player.condition.TakeDamage(tempZoneDamage);
                damageTimer = 0;
            }
        }
    }

    public void Hot()
    {
        curTemp = Mathf.Min(curTemp + (passiveTemp * Time.deltaTime), maxTemp);
    }

    public void Cold()
    {
        curTemp = Mathf.Max(curTemp - (passiveTemp * Time.deltaTime), minTemp);
    }
}
