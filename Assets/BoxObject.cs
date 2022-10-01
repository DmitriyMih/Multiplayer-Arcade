using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxObject : BaseInteractionObject
{
    public override void TakeObject(Transform target)
    {
        base.TakeObject(target);
    }

    public override void DropObject(/*Transform target*/)
    {
        base.DropObject();
    }
}
