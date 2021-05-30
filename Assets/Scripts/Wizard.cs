using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{
    // 4 5
    // 2 3
    // 0 1
    public int cellNumber = 2;
    public bool isHuman = false;
    public int playerNumber = 1;
    public GameObject spellPrefab;
    public Sprite[] transformationSprites;
    public GameObject wand;
    public GameController gameController;

    float castRate = 2f;
    float moveRate = 1f;
    float spellSpeed = 4;
    public int life = 1;
    bool invinsible = false;
    Color originalColor;
    bool dead = false;
    Vector2 newPosition;

    // Start is called before the first frame update
    void Start()
    {
        originalColor = GetComponent<SpriteRenderer>().color;
        Invoke("CastSpell", 1.0f);
        if (!isHuman) {
            Invoke("MoveRandomly", 1.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dead) { return; }
        if (isHuman) {
            if (playerNumber == 1) {
                if (Input.GetKeyDown(KeyCode.W)) {
                    MoveUp();
                }
                if (Input.GetKeyDown(KeyCode.A)) {
                    MoveLeft();
                }
                if (Input.GetKeyDown(KeyCode.D)) {
                    MoveRight();
                }
                if (Input.GetKeyDown(KeyCode.S)) {
                    MoveDown();
                }
            } else {
                if (Input.GetKeyDown(KeyCode.UpArrow)) {
                    MoveUp();
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                    MoveLeft();
                }
                if (Input.GetKeyDown(KeyCode.RightArrow)) {
                    MoveRight();
                }
                if (Input.GetKeyDown(KeyCode.DownArrow)) {
                    MoveDown();
                }
            }
        }
    }

    void MoveUp() {
        if (cellNumber == 4 || cellNumber == 5) { 
            return;
        }
        newPosition = new Vector2(transform.position.x, transform.position.y + 1);
        cellNumber += 2;
        Blink();
    }

    void MoveDown() {
        if (cellNumber == 0 || cellNumber == 1) { 
            return;
        }
        newPosition = new Vector2(transform.position.x, transform.position.y - 1);
        cellNumber -= 2;
        Blink();
    }

    void MoveLeft() {
        if (cellNumber == 0 || cellNumber == 2 || cellNumber == 4) { 
            return;
        }
        newPosition = new Vector2(transform.position.x - 1.5f, transform.position.y);
        cellNumber -= 1;
        Blink();
    }

    void MoveRight() {
        if (cellNumber == 1 || cellNumber == 3 || cellNumber == 5) { 
            return;
        }
        newPosition = new Vector2(transform.position.x + 1.5f, transform.position.y);
        cellNumber += 1;
        Blink();
    }

    void Blink() {
        Shrink();
        Invoke("Shrink", 0.02f);
        Invoke("Shrink", 0.04f);
        Invoke("ChangePosition", 0.06f);
        Invoke("Grow", 0.08f);
        Invoke("Grow", 0.1f);
        Invoke("Grow", 0.12f);
    }

    void Shrink() {
        transform.localScale = transform.localScale / 2;
    }

    void ChangePosition() {
        transform.position = newPosition;
    }
    
    void Grow() {
        transform.localScale = transform.localScale * 2;
    }

    void CastSpell() {
        if (dead) { return; }
        Vector2 spellPos = new Vector2(transform.position.x + 0.4f, transform.position.y + 0.3f);
        if (playerNumber == 2) {
            spellPos = new Vector2(transform.position.x - 0.4f, transform.position.y + 0.3f);
        }
        GameObject spell = Instantiate(spellPrefab, spellPos, Quaternion.identity);
        spell.GetComponent<Spell>().playerNumber = playerNumber;
        var main = spell.GetComponent<ParticleSystem>().main;
        main.startColor = GetComponent<SpriteRenderer>().color;
        spell.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
        if (playerNumber == 1) {
            spell.GetComponent<Rigidbody2D>().velocity = new Vector2(spellSpeed, 0);
        } else {
            spell.GetComponent<Rigidbody2D>().velocity = new Vector2(-spellSpeed, 0);
        }
        Invoke("CastSpell", castRate);
        if (castRate > 0.5f) {
            castRate -= 0.15f;
        }
    }

    void MoveRandomly()
    {
        if (dead) { return; }
        if (cellNumber == 0) {
            var rand = Random.Range(0, 2);
            switch (rand)
            {
                case 0:
                    MoveUp();
                    break;
                case 1:
                    MoveRight();
                    break;
            }
        }
        else if (cellNumber == 1) {
            var rand = Random.Range(0, 2);
            switch (rand)
            {
                case 0:
                    MoveUp();
                    break;
                case 1:
                    MoveLeft();
                    break;
            }
        } else if (cellNumber == 2) {
            var rand = Random.Range(0, 3);
            switch (rand)
            {
                case 0:
                    MoveUp();
                    break;
                case 1:
                    MoveRight();
                    break;
                case 2:
                    MoveDown();
                    break;
            }
        } else if (cellNumber == 3) {
            var rand = Random.Range(0, 3);
            switch (rand)
            {
                case 0:
                    MoveUp();
                    break;
                case 1:
                    MoveLeft();
                    break;
                case 2:
                    MoveDown();
                    break;
            }
        } else if (cellNumber == 4) {
            var rand = Random.Range(0, 2);
            switch (rand)
            {
                case 0:
                    MoveDown();
                    break;
                case 1:
                    MoveRight();
                    break;
            }
        } else if (cellNumber == 5) {
            var rand = Random.Range(0, 2);
            switch (rand)
            {
                case 0:
                    MoveDown();
                    break;
                case 1:
                    MoveLeft();
                    break;
            }
        }
        Invoke("MoveRandomly", moveRate);
        if (moveRate > 0.75f) {
            moveRate -= 0.02f;
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (dead) { return; }
        if (col.gameObject.GetComponent<Spell>().playerNumber != playerNumber) {
            Destroy(col.gameObject);
            if (invinsible) { return; }
            FlashWhite();
            life--;
            if (life <= 0) {
                dead = true;
                GetComponent<SpriteRenderer>().sprite = transformationSprites[Random.Range(0, transformationSprites.Length)];
                Destroy(wand);
                gameController.PlayerDied(playerNumber);
            }
            invinsible = true;
        }
    }

    void FlashWhite() {
        GetComponent<SpriteRenderer>().color = Color.white;
        Invoke("FlashBack", 0.1f);
    }

    void FlashBack() {
        GetComponent<SpriteRenderer>().color = originalColor;
        invinsible = false;
    }
}
