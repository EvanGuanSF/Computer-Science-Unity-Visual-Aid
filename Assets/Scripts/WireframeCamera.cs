using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireframeCamera : MonoBehaviour {
    public bool enableWireframe = true;
    void OnPreRender()
    {
        if(enableWireframe)
        {
            GL.wireframe = true;
        }
    }

    void OnPostRender()
    {
        if (enableWireframe)
        {
            GL.wireframe = false;
        }
    }
}
