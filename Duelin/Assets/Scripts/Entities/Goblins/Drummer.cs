using UnityEngine;


public class Drummer : BaseGoblin
{
    public override int Cost() => 5;
    
    new void Start()
    {
        base.Start();
        GameObject.FindGameObjectsWithTag("Unit").ForEach(obj => {
            BaseGoblin goblin = obj.GetComponent<BaseGoblin>();
            if(goblin == null) {
                Debug.Log(obj.name);
                return;
            }
            goblin.Damage -= 1;
        });
    }
}