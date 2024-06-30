using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum NPCStateName
{
    IDLE,
    WALK,
    ORDER,
    WAITINGPIZZA,
    LEAVE,

    LAST
}

public class NPCIdleState : BaseState<NPCController>
{
    public NPCIdleState(NPCController npcController) : base(npcController)
    {

    }

    public override void OnEnterState()
    {
       
    }

    public override void OnExitState()
    {
    }

    public override void OnUpdateState()
    {
    }
}

public class NPCWalkState : BaseState<NPCController>
{
    public NPCWalkState(NPCController npcController) : base(npcController)
    {

    }

    public override void OnEnterState()
    {
        Owner.SetNPCIsStopping(false);
        Owner.SetBoolNPCAnimotorPropertyToName("IsWalk",true);
        Owner.SetNPCDestinationPos(NavmeshManger.Instance.GetDestinationPos());
    }

    public override void OnExitState()
    {
        Owner.SetBoolNPCAnimotorPropertyToName("IsWalk", false);
        Owner.SetNPCIsStopping(true);
    }

    public override void OnUpdateState()
    {
        if(Owner.CheckArriveDesrtination())
        {
            Owner.ChangeNPCState(NPCStateName.ORDER);
        }
    }
}

public class NPCOrderState : BaseState<NPCController>
{
    public NPCOrderState(NPCController npcController) : base(npcController)
    {

    }

    public override void OnEnterState()
    {
        UIManger.Instance.PrintText("주문이요");
        Owner.RegisterCheckState();
    }

    public override void OnExitState()
    {
        
    }

    public override void OnUpdateState()
    {
    }
}
public class NPCWaitingPizzaState : BaseState<NPCController>
{
    private int _randomPizzaIndex;
    public NPCWaitingPizzaState(NPCController npcController) : base(npcController)
    {

    }

    public override void OnEnterState()
    {
        SetRandomIndex();
        var oederPizza= DataManger.Inst.GetPizzaData(PlayerController.Instance.PizaaRecipe[_randomPizzaIndex]);
        Owner._orderPizza = oederPizza.Name;
        UIManger.Instance.PrintText($"{Owner._orderPizza} 주세요");
        InteractionObjectManger.Instance.UnRegisterChangeNPCState();
        
    }

    public override void OnExitState()
    {
        Owner.UnRegisterCheckState();
    }

    public override void OnUpdateState()
    {
    }

    private void SetRandomIndex()
    {
        _randomPizzaIndex = 0;
        _randomPizzaIndex = Random.Range(0, PlayerController.Instance.PizaaRecipe.Count);
    }
}

public class NPCLeaveState : BaseState<NPCController>
{
    public NPCLeaveState(NPCController npcController) : base(npcController)
    {

    }

    public override void OnEnterState()
    {
    }

    public override void OnExitState()
    {
    }

    public override void OnUpdateState()
    {
    }
}
