using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour, IListener
{
 
    Renderer material;
    Rigidbody rbody;

    // Use this for initialization
    void Start()
    {
        rbody = GetComponent<Rigidbody>();
        material = GetComponent<Renderer>();
        material.material.SetColor("_Color", Color.cyan);
        EventManager.Instance.AddListener(EVENT_TYPE.BLOCK_HOVER, this);
        EventManager.Instance.AddListener(EVENT_TYPE.BLOCK_NOHOVER, this);
        EventManager.Instance.AddListener(EVENT_TYPE.REVEAL_BLOCK, this);

    }

    void OnDestory() {
        EventManager.Instance.RemoveEvent( EVENT_TYPE.BLOCK_HOVER);
        EventManager.Instance.RemoveEvent( EVENT_TYPE.BLOCK_NOHOVER);
        EventManager.Instance.RemoveEvent( EVENT_TYPE.REVEAL_BLOCK);
    }

    public void OnEvent(EVENT_TYPE event_type, Component sender, Object param = null)
    {
        switch(event_type)
        {
            case EVENT_TYPE.REVEAL_BLOCK:
                if (sender.tag.Equals(tag))
                {
                    OnReveal();
                }
                break;
            case EVENT_TYPE.BLOCK_HOVER:
                if(sender.tag.Equals(tag))
                {
                    OnHover();
                }
                break;
            case EVENT_TYPE.BLOCK_NOHOVER:
                OnNoHover();
                break;
        }

    }

    public void OnReveal()
    {
        rbody.AddForce(transform.forward * 1000f);
    }

    void OnHover()
    {
        material.material.SetColor("_Color", Color.yellow);
    }

    void OnNoHover()
    {
        material.material.SetColor("_Color", Color.cyan);
    }

}
