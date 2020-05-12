using UnityEngine;

public class AutoDestroyParticles : MonoBehaviour
{
    private void OnDisable()
    {
        Destroy(this.gameObject);
    }
}
