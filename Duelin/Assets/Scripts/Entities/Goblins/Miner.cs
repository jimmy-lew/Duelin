public class Miner : BaseGoblin, IPassiveAbility
{
    public override int Cost() => 2;

    public void HandlePassive(){}

    public string PassiveAbilityDescription() => "Double gold of mine unit has captured";
}
