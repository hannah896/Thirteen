using UnityEngine;
using UnityEngine.UI;

public class BuildingSlot : MonoBehaviour
{
    public BuildingData building;
    public Button button;
    public UIBuilding uiBuilding;

    public int index;

    public void OnClickButton()
    {
        uiBuilding.SelectBuilding(index);
    }
}