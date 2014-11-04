using UnityEngine;
using System.Collections;

public class VisualizeCollision : MonoBehaviour
{
    public bool Enter = true;
    public bool Stay = true;
    public bool Exit = true;

    void OnCollisionEnter(Collision col)
    {
        if (Enter)
            foreach (ContactPoint c in col.contacts)
            {
                Util.Draw.Target(c.point, 1, Color.green, 0.2f);
            }
    }

    void OnCollisionStay(Collision col)
    {
        if (Stay)
            foreach (ContactPoint c in col.contacts)
            {
                Util.Draw.Target(c.point, 0.5f, Color.yellow);
            }
    }

    void OnCollisionExit(Collision col)
    {
        if (Exit)
            foreach (ContactPoint c in col.contacts)
            {
                Util.Draw.Target(c.point, 1, Color.red, 0.2f);
            }
    }
}
