using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilPuddle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider2D) {
        var shockable = collider2D.GetComponent<ShockableCar>();

        if(shockable) {
            shockable.Shock(collider2D.transform.up, collider2D.attachedRigidbody.velocity.magnitude);
            Destroy(gameObject);
        }
    }
}
