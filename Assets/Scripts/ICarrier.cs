using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GrabbableManager
{
    public static List<IGrabbable> Grabbables { get; private set; } =
        new List<IGrabbable>();

    public static IGrabbable GetCloserAvailableGrabbable(Vector3 position, IGrabbable self = null)
    {
        float closer = float.MaxValue;
        IGrabbable closerOne = null;
        foreach (var grabbable in Grabbables)
        {
            if (grabbable == self)
                continue;

            float dist = Vector2.Distance(grabbable.transform.position, position);
            if (dist < closer)
            {
                if(grabbable.IsAvailableToGrab())
                {
                    closerOne = grabbable;
                    closer = dist;
                }
            }
        }

        return closerOne;
    }
}

public interface IGrabbable
{
    bool Grab(ICarrier carryer);
    void Release();
    bool IsAvailableToGrab();

    bool WasThrown { get; set; }
    Transform transform { get;}
    Rigidbody2D rb2D { get;}
    ICarrier Carryer { get; }
}

public interface ICarrier
{
    void Throw();
    void Drop();
    void GrabCloserGrabbable();
    void GrabGrabbable(IGrabbable grabbable);
    Transform GrabPosition { get;}
    Transform transform { get;}
    Rigidbody2D rb2D { get;}
    IGrabbable CarriedGrabbable { get; }
}
