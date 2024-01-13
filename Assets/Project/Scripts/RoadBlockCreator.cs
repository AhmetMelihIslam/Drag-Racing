using UnityEngine;

public class RoadBlockCreator : MonoBehaviour
{
    private GameObject _roadBlock;

    private void Start()
    {
        _roadBlock = transform.GetChild(0).gameObject;

        var finishObject = Utils.FindGameObjectByName("Finish");
        
        int roadCount = 0;
        while (true)
        {
            roadCount += 1;
            
            Vector3 position = _roadBlock.transform.position;
            // 7.286 weight of road block
            position.x += -7.286f * roadCount;
            
            Instantiate(_roadBlock, position, _roadBlock.transform.rotation, transform);
            
            if (position.x < finishObject.transform.position.x) break;
        }
        
    }

}
