using UnityEngine;
using UnityEngine.UI;

public class BuildingSlot : MonoBehaviour
{
    public BuildingData building;

    public Button button;
    public Outline outline;

    public UIBuilding uiBuilding;

    public int index;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    public void OnClickButton()
    {
        uiBuilding.SelectBuilding(index);
    }
}