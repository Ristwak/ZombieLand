using UnityEngine;

public class ObjectToHit : MonoBehaviour
{
    public float objectHealth = 100f;
    private bool isDestroyed;

    public void ObjectHitDamage(float amount)
    {
        if (isDestroyed || amount <= 0f) return;

        objectHealth = Mathf.Max(0f, objectHealth - amount);

        if (objectHealth <= 0f) Die();
    }

    void Die()
    {
        if (isDestroyed) return;

        isDestroyed = true;
        Destroy(gameObject);
    }
}