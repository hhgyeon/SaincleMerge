using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaincleController : MonoBehaviour
{
    public GameController gC;

    public ParticleSystem effect;

    public int level;
    public bool isDrag;
    public bool isMerge;

    public Rigidbody2D rgbd;
    Animator anim;
    CircleCollider2D circle;
    SpriteRenderer spriteRenderer;

    float deadTime;

    void Awake()
    {
        rgbd = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        circle = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnEnable()
    {
        anim.SetInteger("Level", level);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDrag) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float leftB = -4.2f + transform.localScale.x / 2;
            float rightB = 4.2f - transform.localScale.x / 2;
            if (mousePos.x < leftB)
            {
                mousePos.x = leftB;
            }
            else if (mousePos.x > rightB)
            {
                mousePos.x = rightB;
            }
            mousePos.y = 7;
            mousePos.z = 0;
            transform.position = Vector3.Lerp(transform.position, mousePos, 0.3f);
        }
    }

    public void Drag() {
        isDrag = true;
    }

    public void Drop() {
        isDrag = false;
        rgbd.simulated = true;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Saincle")
        {
            SaincleController other = collision.gameObject.GetComponent<SaincleController>();
            if (level == other.level && !isMerge && !other.isMerge && level < 7)
            {
                //합쳐지기
                float meX = transform.position.x;
                float meY = transform.position.y;
                float youX = other.transform.position.x;
                float youY = other.transform.position.y;

                if (meY < youY || (meY == youY && meX > youX))
                {
                    other.Hide(transform.position);
                    LevelUp();
                }
            }
        }
    }

    public void Hide(Vector3 tPos) {
        isMerge = true;
        rgbd.simulated = false; // 물리 효과 끄기
        circle.enabled = false;

        if(tPos == Vector3.up * 100)
        {
            EffectPlay();
        }

        StartCoroutine(HideRoutine(tPos));
    }

    IEnumerator HideRoutine(Vector3 tPos)
    {
        int frameCount = 0;
        while(frameCount < 20)
        {
            frameCount++;
            if (tPos != Vector3.up * 100)
            {
                transform.position = Vector3.Lerp(transform.position, tPos, 0.5f);
            }
            else if (tPos == Vector3.up * 100)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.2f);
            }
            yield return null;
        }
        gC.score += (int)Mathf.Pow(2, level);
        isMerge = false;
        gameObject.SetActive(false);
    }

    void LevelUp()
    {
        isMerge = true;
        rgbd.velocity = Vector2.zero;
        rgbd.angularVelocity = 0;

        StartCoroutine("LevelUpRoutine");
    }

    IEnumerator LevelUpRoutine()
    {
        yield return new WaitForSeconds(0.2f);
        anim.SetInteger("Level", level + 1);
        EffectPlay();
        yield return new WaitForSeconds(0.3f);
        level++;

        gC.maxLevel = Mathf.Max(level, gC.maxLevel);

        isMerge = false;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Finish")
        {
            deadTime += Time.deltaTime;

            if(deadTime > 2)
            {
                spriteRenderer.color = new Color(0.9f, 0.4f, 0.4f);
            }
            if(deadTime > 5)
            {
                gC.GameOver();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Finish")
        {
            deadTime = 0;
            spriteRenderer.color = Color.white;
        }
    }

    void EffectPlay()
    {
        effect.transform.position = transform.position;
        effect.transform.localScale = transform.localScale / 2;
        effect.Play();
    }

}
