using UnityEngine;

public class PrefabDestroy : MonoBehaviour
{
    [SerializeField] float destroyY = -20f;

    void Update()
    {
        if(transform.position.y < destroyY)
        {
            Destroy(gameObject);
        }
    }
}
