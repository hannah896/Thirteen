using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BodyTemp : MonoBehaviour
{
    [Header("PlayerTemp")]
    public float curTemp;
    public float startTemp;
    public float maxTemp;
    public float minTemp;
    public float passiveTemp;
    public float recoveryTemp;
    public TextMeshProUGUI tempText;

    [Header("Damage")]
    public int tempZoneDamage;
    public float damageRate;
    private float damageTimer;

    [Header("Indicator")]
    public Image indicatorImage;
    public float flashSpeed;

    private Coroutine coroutine;
    private bool isCoroutine = false;
    private bool isInTempZone = false;

    private void Start()
    {
        curTemp = startTemp;
    }

    private void Update()
    {
        tempText.text = curTemp.ToString("F2") + "¡ÆC"; ;

        damageTimer += Time.deltaTime;

        if (curTemp >= 38f || curTemp <= 34f)
        {
            if (damageTimer >= damageRate)
            {
                CharacterManager.Instance.Player.condition.TakeDamage(tempZoneDamage);
                damageTimer = 0;
            }
        }

        if (!isInTempZone)
        {
            curTemp = Mathf.Lerp(curTemp, 36.5f, recoveryTemp * Time.deltaTime);                       
        }
    }

    public void Hot()
    {
        isInTempZone = true;
        curTemp = Mathf.Min(curTemp + (passiveTemp * Time.deltaTime), maxTemp);
        ShowIndicator(new Color(1f, 0f, 0f, 0.35f));
    }

    public void Cold()
    {
        isInTempZone = true;
        curTemp = Mathf.Max(curTemp - (passiveTemp * Time.deltaTime), minTemp);
        ShowIndicator(new Color(0f, 0f, 1f, 0.35f));
    }

    private void ShowIndicator(Color targetColor)
    {
        if (indicatorImage != null)
        {
            if (isCoroutine)
            {
                indicatorImage.color = targetColor;
                return;
            }

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            indicatorImage.enabled = true;
            indicatorImage.color = targetColor;
            coroutine = StartCoroutine(FadeIndicator());
        }
    }

    private IEnumerator FadeIndicator()
    {
        isCoroutine = true;
        float startAlpha = indicatorImage.color.a;
        float alpha = startAlpha;

        while (alpha > 0)
        {
            alpha -= (startAlpha / flashSpeed) * Time.deltaTime;
            indicatorImage.color = new Color(indicatorImage.color.r, indicatorImage.color.g, indicatorImage.color.b, alpha);
            yield return null;
        }

        indicatorImage.enabled = false;
        isCoroutine = false;
    }

    public void ExitTempZone()
    {
        isInTempZone = false;
    }
}
