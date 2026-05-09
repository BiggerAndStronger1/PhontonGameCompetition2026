using UnityEngine;

public class Tutorial : MonoBehaviour
{
    //定义节点
    public GameObject[] nodes;//教程节点

    void Start()
    {
        //隐藏节点
        foreach (GameObject node in nodes)
        {
            node.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void play(int index)
    {
        //显示节点
        nodes[index].gameObject.SetActive(true);
    }
    public void end()
    {
        //隐藏所有节点
        foreach (GameObject node in nodes)
        {
            node.SetActive(false);
        }
    }
}
