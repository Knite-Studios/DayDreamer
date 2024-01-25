using UnityEngine;

public class BlockTrigger : MonoBehaviour
{
    [SerializeField] private int triggerNumber;

    [HideInInspector] public bool block1, block2, block3;

    void OnTriggerEnter(Collider other)
    {
        UpdateBlockStatus(other, true);
    }

    void OnTriggerExit(Collider other)
    {
        UpdateBlockStatus(other, false);
    }

    private void UpdateBlockStatus(Collider other, bool status)
    {
        blockType block = other.GetComponent<blockType>();
        if (block != null && block.blockNumber == triggerNumber)
        {
            switch (triggerNumber)
            {
                case 1:
                    block1 = status;
                    break;
                case 2:
                    block2 = status;
                    break;
                case 3:
                    block3 = status;
                    break;
            }
        }
    }
}
