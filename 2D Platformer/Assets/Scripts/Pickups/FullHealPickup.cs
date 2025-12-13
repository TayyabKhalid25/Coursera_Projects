using UnityEngine;

public class FullHealPickup : MonoBehaviour
{

    public GameObject pickupEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Health obj = collision.gameObject.GetComponent<Health>();
            if (obj.currentHealth < obj.maximumHealth)
            {
                obj.ReceiveHealing(obj.maximumHealth);
                if (pickupEffect != null)
                {
                    Instantiate(pickupEffect, transform.position, Quaternion.identity);
                }
                Destroy(this.gameObject);
            }
        }
    }
}
