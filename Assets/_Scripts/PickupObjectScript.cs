using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObjectScript : MonoBehaviour {

    public enum PickupObjectType {
        KEYCARD, HP, OXYGEN
    };

    public PickupObjectType type;
    public Transform spriteTransform;
    private AudioSource audioSource;
    private Vector2 startPosition;
    private Vector2 nextPosition;

    private float elapsed;
    private float bobDuration;
    private float bobDurationOffset;

    // Use this for initialization
    void Start() {
        startPosition = this.transform.position;
        nextPosition = startPosition + new Vector2(0, 0.15f);
        audioSource = GetComponent<AudioSource>();
        bobDurationOffset = Random.Range(-0.25f, 0.1f);
    }

    // Update is called once per frame
    void Update() {
        BobObject();
    }

    private void BobObject() {
        elapsed += Time.deltaTime;
        bobDuration = 1.0f + bobDurationOffset;
        this.transform.position = Vector2.Lerp(startPosition, nextPosition, elapsed / bobDuration);
        if (elapsed > 1.0f * bobDuration) {
            Vector2 tempPosition = startPosition;
            startPosition = nextPosition;
            nextPosition = tempPosition;
            elapsed = 0.0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag.Equals("Player")) {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            bool pickedUp = false;
            switch (type) {
                case PickupObjectType.KEYCARD:
                player.GetInventory().AddItem(type);
                pickedUp = true;
                break;
                case PickupObjectType.HP:
                if (!player.IsHealthFull()) {
                    player.ReplenishHealth(7.5f);
                    pickedUp = true;
                }
                break;
                case PickupObjectType.OXYGEN:
                if (!player.IsOxygenFull()) {
                    player.ReplenishOxygen(0.3f);
                    pickedUp = true;
                }
                break;
            }
            if (pickedUp) {
                audioSource.Play();
                spriteTransform.gameObject.SetActive(false);
                GetComponent<Collider2D>().enabled = false;
                StartCoroutine(RemoveFromScene());
            }
        }
    }

    public IEnumerator RemoveFromScene() {
        float timer = 0.0f;
        while (timer < 1.0f) {
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(this.gameObject);
        StopAllCoroutines();
        yield return null;
    }

    private void OnCollisionEnter2D(Collision2D collision) {

    }
}
