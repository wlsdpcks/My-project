using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SteamInputModule : VRInputModule
{
    public SteamVR_Input_Sources m_TargetSource;
    public SteamVR_Action_Boolean m_ClickAction;

    public override void Process()
    {
        base.Process();
        // Press
        if (m_ClickAction.GetStateDown(m_TargetSource))
            ProcessPress(m_Data);

        // Release(«ÿ¡¶)
        if (m_ClickAction.GetStateUp(m_TargetSource))
            ProcessRelease(m_Data);
    }
    
}
