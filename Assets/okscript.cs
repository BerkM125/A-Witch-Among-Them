using UnityEngine;

public class okscript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DismissParentPanels () {
        // Dismiss all parent panels of this GameObject
        Transform parent = transform.parent;
        parent.transform.parent.gameObject.SetActive(false);
    }
}
