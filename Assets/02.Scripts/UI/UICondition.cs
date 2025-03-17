using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition hp;
    public Condition hunger;
    public Condition stamina;
    public Condition thirst;
    public BodyTemp bodyTemp;

    private void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}
