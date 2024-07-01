using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum NPCStateName
{
    IDLE,
    WALK,
    ORDER,
    WAITINGPIZZA,
    CHECKPIZZA,
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
        UIManger.Instance.PrintNPCText("주문이요", false);
        Owner.RegisterCheckState();
        Owner.RegisterChangeStateToLeave();
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
    
    public NPCWaitingPizzaState(NPCController npcController) : base(npcController)
    {

    }

    public override void OnEnterState()
    {
        Owner.SetRandomIndex();
        string size = Owner.PizzaSize == 0 ? "레귤러 사이즈" : "라지 사이즈";
        UIManger.Instance.PrintNPCText($"{Owner.Pizza.Name} {size} 주세요",false);
        InteractionObjectManger.Instance.UnRegisterChangeNPCState();
        Owner.RegisterCheckPizza();
    }

    public override void OnExitState()
    {
       Owner.UnRegisterCheckPizza();
    }

    public override void OnUpdateState()
    {
    }

    
}
public class NPCCheckPizzaState : BaseState<NPCController>
{
    public NPCCheckPizzaState(NPCController npcController) : base(npcController)
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
public class NPCLeaveState : BaseState<NPCController>
{
    public NPCLeaveState(NPCController npcController) : base(npcController)
    {

    }

    public override void OnEnterState()
    {
        Owner.UnRegisterCheckState();
        Owner.UnRegisterChangeStateToLeave();
        Owner.SetNPCIsStopping(false);
        Owner.SetBoolNPCAnimotorPropertyToName("IsWalk", true);
        Owner.SetNPCDestinationPos(NavmeshManger.Instance.GetRespawnPos());
    }

    public override void OnExitState()
    {
        Owner.SetBoolNPCAnimotorPropertyToName("IsWalk", false);
        Owner.SetNPCIsStopping(true);
    }

    public override void OnUpdateState()
    {
        if (Owner.CheckArriveDesrtination())
        {
            Owner.ChangeNPCState(NPCStateName.IDLE);
        }
    }
}
