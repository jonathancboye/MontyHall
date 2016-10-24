using UnityEngine;
using System.Collections;

public class Raycaster : MonoBehaviour {

    public LayerMask mask;

    RaycastHit hit;
    Ray ray;
    bool hovering;
   
    void Start()
    {
        hovering = false;
    }

	void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
 
        if (Physics.Raycast(ray, out hit, 20f, mask.value))
        {
            if(hovering != true)
            {
                hovering = true;
                EventManager.Instance.Notify(EVENT_TYPE.BLOCK_HOVER, hit.transform);            
            }

            if(Input.GetButtonDown("Fire1"))
            {
                EventManager.Instance.Notify(EVENT_TYPE.BLOCK_SELECTED, hit.transform);
            }
        }
        else
        {
            if (hovering != false)
            {
                hovering = false;
                EventManager.Instance.Notify(EVENT_TYPE.BLOCK_NOHOVER, null);
            }
        }

    }
}
