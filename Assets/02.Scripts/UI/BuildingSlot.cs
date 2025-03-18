using UnityEngine;

public class BuildingSlot : MonoBehaviour
{
    public BuildingData building;
    public UIBuilding uiBuilding;

    public int index;

    public void OnClickButton()
    {
        uiBuilding.SelectBuilding(index);
    }
}