using UnityEngine;

public class ItemSequenceManager : MonoBehaviour
{
    [Header("物品序列")]
    public GameObject item1;
    public GameObject item2;
    public GameObject item3;
    public Animator door;
    public bool IsDoorOpen;

    [Header("控制设置")]
    public bool startFirstItemFade = false; // 控制第一个物品开始消失的布尔值

    private FadeOutItem item1FadeScript;

    private void Start()
    {
        // 初始状态：只激活第一个物品
        item1.SetActive(true);
        item2.SetActive(false);
        item3.SetActive(false);

        // 获取第一个物品的淡出脚本
        item1FadeScript = item1.GetComponent<FadeOutItem>();

        // 确保第一个物品不会自动开始消失
        if (item1FadeScript != null)
        {
            item1FadeScript.autoStart = false;
        }
    }

    private void Update()
    {
        // 当布尔值变为true时启动第一个物品的消失
        if (startFirstItemFade && item1FadeScript != null && !item1FadeScript.IsFading)
        {
            item1FadeScript.StartFade();
            startFirstItemFade = false; // 重置标志
        }
        if (IsDoorOpen == true)
        {
            door.SetBool("IsOpen", true);

        }
    }

    // 被物品调用的方法
    public void OnItemFaded(int itemIndex)
    {
        switch (itemIndex)
        {
            case 1:
                Debug.Log("第一个物品消失完毕");
                ActivateItem(item2, 2);
                break;

            case 2:
                Debug.Log("第二个物品消失完毕");
                ActivateItem(item3, 3);
                break;

            case 3:
                Debug.Log("第三个物品消失完毕");
                Debug.Log("所有物品消失完毕！游戏结束");
                IsDoorOpen = true;
                // 这里可以添加更多结束逻辑
                break;
        }
    }

    private void ActivateItem(GameObject item, int index)
    {
        if (item != null)
        {
            item.SetActive(true);
            var fadeScript = item.GetComponent<FadeOutItem>();
            if (fadeScript != null)
            {
                fadeScript.itemIndex = index;
            }
        }
    }

    // 外部调用以启动序列
    public void StartItemSequence()
    {
        startFirstItemFade = true;
    }
}