using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxObject : BaseInteractionObject
{
    public override void TakeObject(Transform target, string sendler)
    {
        base.TakeObject(target, sendler);
    }

    public override void DropObject(/*Transform target*/)
    {
        base.DropObject();
    }
}
