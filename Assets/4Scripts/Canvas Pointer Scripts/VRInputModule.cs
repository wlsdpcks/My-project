using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

public class VRInputModule : BaseInputModule
{
    public Camera m_Camera;

    //public SteamVR_Input_Sources m_TargetSource;
    //public SteamVR_Action_Boolean m_ClickAction;

    private GameObject m_CurrentObject = null;
    protected PointerEventData m_Data = null; 

    protected override void Awake()
    {
        base.Awake();

        m_Data = new PointerEventData(eventSystem);
    }

    public override void Process()
    {
        // Reset data, set camera
        m_Data.Reset();
        m_Data.position = new Vector2(m_Camera.pixelWidth / 2, m_Camera.pixelHeight / 2);

        // Raycast(어떤 위치에서 광선을 발사하여 그 광선에 닿는 물체가 있는지 검사하는 방식)
        eventSystem.RaycastAll(m_Data, m_RaycastResultCache);
        m_Data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        m_CurrentObject = m_Data.pointerCurrentRaycast.gameObject;

        // Clear
        m_RaycastResultCache.Clear();

        // Hover(커서 갖다 대기)
        HandlePointerExitAndEnter(m_Data, m_CurrentObject); //(현재 레이저 포인터의 데이터, 타겟의 오브젝트)

    }

    public PointerEventData GetData() { return m_Data; }

    protected void ProcessPress(PointerEventData data) // 외부접근X, 자식클래스에서 접근O
    {
        // Set raycast
        data.pointerPressRaycast = data.pointerCurrentRaycast;

        // Check for object hit, get the down handler, call
        GameObject newPointerPress = ExecuteEvents.ExecuteHierarchy(m_CurrentObject, data, ExecuteEvents.pointerDownHandler);

        // If no downhandler, try and get click handler 
        if(newPointerPress == null)
        {
            newPointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(m_CurrentObject); 
        }

        // Set data
        data.pressPosition = data.position;
        data.pointerPress = newPointerPress;
        data.rawPointerPress= m_CurrentObject;
    }

    protected void ProcessRelease(PointerEventData data) 
    {
        // Execute pointer up
        ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerUpHandler);

        // Check for click handler
        GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(m_CurrentObject);

        // Check if actual
        if(data.pointerPress == pointerUpHandler)
        {
            ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerClickHandler);
        }

        // Clear selected gameobject
        eventSystem.SetSelectedGameObject(null);

        // Reset data
        data.pressPosition = Vector2.zero;
        data.pointerPress = null;
        data.rawPointerPress= null;
    }
}
