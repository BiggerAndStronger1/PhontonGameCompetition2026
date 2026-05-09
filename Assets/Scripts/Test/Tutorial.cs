using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class Tutorial : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //定义节点
    public GameObject[] nodes;
    void Start()
    {
        //初始隐藏节点
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].SetActive(false);
        }
    }

    // Update is called once per frame
    //显示节点
    public void ShowNode(int index)
    {
        nodes[index].SetActive(true);
    }
    //隐藏节点
    public void HideNode(int index)
    {
        nodes[index].SetActive(false);
    }
    //隐藏所有节点
    public void HideAllNodes()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].SetActive(false);
        }
    }

    void Update()
    {

        //如果按键ESC键，显示节点1
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (nodes[0].activeSelf != true)
            {
                ShowNode(0);
            }
            else
            {
                HideAllNodes();
            }
        }

    }

}
