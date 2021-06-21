using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityEngineTool
{
    public static Vector3 With(this Vector3 origin, float? x=null,float? y=null,float? z=null)
    {
        return new Vector3(x?? origin.x,y?? origin.y,z?? origin.z);
    }
}
