/**
 * @file   PlayerScript.cs
 *
 * @brief  プレイヤーに関するヘッダファイル
 *
 * @author 制作者名　深谷翔太
 *
 * @date   日付　2026/02/02
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public enum ItemKinds
{
    NON,
    TORCH,
    OXYGEN_CYLINDER,
    BALOON,
}
public class PlayerScript : MonoBehaviour
{
    public enum SoundKinds : int
    {
        WALK,
        JUMP,
        LANFING,
        THROW_TREASURE,
        SWIM
    }

    public enum PlayerMode
    {
        GAME_PLAY,
        TITLE,
        ITEM_SELECT,
    }

    [System.Serializable]
    struct Soundes
    {
        public AudioClip SE;
        public AudioSource audioSource;
        public SoundKinds soundKinds;
    }// クラス定数の宣言 -------------------------------------------------
    //プレイヤーの重さ
    public const float PLAYER_WIEGHT = 100.0f;

    // データメンバの宣言 -----------------------------------------------
    [SerializeField] private float m_deadTime = 20.0f;
    float m_currentTime = 0.0f;
    //足元の判定
    [SerializeField] private Collider2D m_footCollider;
    bool m_isFootHit = false;
    //頭の判定
    [SerializeField] private Collider2D m_headCollider;
    bool m_isHeadHit = false;
    //SE群
    [SerializeField] private Soundes[] m_soundes;
    //プレイヤーのモード
    [SerializeField] private PlayerMode m_playerMode;
    //現在の重さ
    float m_mass = 100.0f;
    //軽減分
    float m_minusMass;
    //合計スコア
    int m_score = 0;
    //入手宝配列
    public List<GameObject> m_takeTresures ;
    ///死亡した時に遷移するシーン
    [SerializeField] private string m_deadScene; 

    [SerializeField] float      m_speed = 4.0f;      ///< 移動速度
    [SerializeField] float      m_jumpForce = 7.5f;  ///< ジャンプ力
    [SerializeField] float      m_rollForce = 6.0f;  ///< 回転力
    [SerializeField] bool       m_noBlood = false;
    [SerializeField] GameObject m_slideDust;
    [SerializeField] float m_deadY;                  ///< プレイヤーが死亡するY座標

    [SerializeField] private ItemKinds m_itemKind;   ///< プレイヤーの所持アイテム


    private Animator            m_animator; ///< アニメーター
    private Rigidbody2D         m_body2d;
    private Sensor_HeroKnight   m_groundSensor;
    private Sensor_HeroKnight   m_wallSensorR1;
    private Sensor_HeroKnight   m_wallSensorR2;
    private Sensor_HeroKnight   m_wallSensorL1;
    private Sensor_HeroKnight   m_wallSensorL2;
    private bool                m_isWallSliding = false;
    private bool                m_grounded = false;
    private bool                m_rolling = false;
    private int                 m_facingDirection = 1;
    private int                 m_currentAttack = 0;
    private float               m_timeSinceAttack = 0.0f;
    private float               m_delayToIdle = 0.0f;
    private float               m_rollDuration = 8.0f / 14.0f;
    private float               m_rollCurrentTime;


  
    // Use this for initialization
    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
        //リスト初期化
        m_takeTresures = new List<GameObject>();
        //初期化
        m_itemKind = GameManager.instance.GetSelectItem();
        for (int i = 0; i < m_soundes.Length; i++)
        {
            if (m_soundes[i].audioSource == null) m_soundes[i].audioSource = GetComponent<AudioSource>();
        }//初期化

        TreasureManager.instance.UnRegisterTreasure();
        TreasureManager.instance.ResetID();

        m_currentTime = m_deadTime;

        if(m_itemKind == ItemKinds.BALOON)
        {
            m_minusMass = -300.0f;
        }
        else
        {
            m_minusMass = 0.0f;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;

        // Increase timer that checks roll duration
        if(m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        // Disable rolling if timer extends duration
        if(m_rollCurrentTime > m_rollDuration)
            m_rolling = false;

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
            m_soundes[(int)SoundKinds.LANFING].audioSource.PlayOneShot(m_soundes[(int)SoundKinds.LANFING].SE);

        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");
        if (m_playerMode == PlayerMode.TITLE)
        {
            inputX = 1.0f;
        }
        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }
            
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }

        float massIndex = m_mass + m_minusMass;
        if (massIndex <= 0.0f)
        {
            massIndex = 0.0f;
        }
        float calcMass = Mathf.Max(m_mass + m_minusMass, 0.0f);
        float speedIndex = 1.0f / Mathf.Max(calcMass / 100.0f, 1.0f);
        // 移動
        if (!m_rolling )
            m_body2d.velocity = new Vector2(inputX * m_speed * speedIndex, m_body2d.velocity.y);

        if (m_playerMode == PlayerMode.TITLE)
        {
            //画面端に行ったら戻る
            if (m_body2d.position.x >= 8.0f)
            {
                m_body2d.position = new Vector2(-8.0f, m_body2d.position.y);
            }

        }

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        if (m_isHeadHit)
        {
            float index = 1;
            if (m_itemKind == ItemKinds.OXYGEN_CYLINDER) index = 10.0f;
            m_currentTime -= Time.deltaTime / index;
        }
        else
        {
            if(m_currentTime <= m_deadTime)
                m_currentTime += Time.deltaTime;
            else
                m_currentTime = m_deadTime;
        }
        if(m_currentTime <= 0.0f)
        {
            OnDead();
        }

        //ジャンプモーション
        if ((Input.GetKeyDown("space") && m_playerMode != PlayerMode.TITLE) && (m_grounded || m_isFootHit) || (Input.GetKeyDown(KeyCode.UpArrow) && m_playerMode == PlayerMode.TITLE && m_grounded))
        {
            if(m_playerMode == PlayerMode.ITEM_SELECT) return;
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce * speedIndex);
            m_groundSensor.Disable(0.2f);
            if (m_isFootHit)
            {
                m_soundes[(int)SoundKinds.SWIM].audioSource.PlayOneShot(m_soundes[(int)SoundKinds.SWIM].SE);
            }
            else
            {
                m_soundes[(int)SoundKinds.JUMP].audioSource.PlayOneShot(m_soundes[(int)SoundKinds.JUMP].SE);
            }
            m_soundes[(int)SoundKinds.WALK].audioSource.Pause();
        }

        //移動モーション
        else if (Mathf.Abs(inputX) > Mathf.Epsilon && m_grounded)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);

            if (!m_soundes[(int)SoundKinds.WALK].audioSource.isPlaying)
            {
                m_soundes[(int)SoundKinds.WALK].audioSource.clip = m_soundes[(int)SoundKinds.WALK].SE;
                m_soundes[(int)SoundKinds.WALK].audioSource.Play();
            }
        }

        //待機モーション
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                m_animator.SetInteger("AnimState", 0);
            if (m_soundes[(int)SoundKinds.WALK].audioSource.isPlaying)
            {
                m_soundes[(int)SoundKinds.WALK].audioSource.Pause();
            }
        }

        //宝を捨てる
        if (Input.GetKeyDown(KeyCode.E) && m_playerMode == PlayerMode.GAME_PLAY)
        {
            m_soundes[(int)SoundKinds.THROW_TREASURE].audioSource.PlayOneShot(m_soundes[(int)SoundKinds.THROW_TREASURE].SE);
            ThrowTreasure();
        }

        //死亡判定
        if(this.transform.position.y < m_deadY)
        {
            OnDead();
        }

        if (m_isFootHit)
        {
            m_body2d.gravityScale = 0.5f;
        }
        else
        {
            m_body2d.gravityScale = 1.0f;
        }
    }

    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }

    /**
    * @brief 宝を捨てる
    *
    * @param[in] なし
    *
    * @return なし
    */
    private void ThrowTreasure()
    {
        //要素が二つ以上なら
        if (m_takeTresures.Count > 1)
        {
            //スコア昇順ソート
            m_takeTresures.Sort((a, b) => a.GetComponent<Treasure>().GetWeight().CompareTo(b.GetComponent<Treasure>().GetWeight()));
        }
        //要素があるなら
        if (m_takeTresures.Count > 0)
        {
            //末尾を取得
            GameObject last = m_takeTresures[m_takeTresures.Count - 1];
            Treasure treasure = last.GetComponent<Treasure>();
            //末尾を消す
            m_takeTresures.RemoveAt(m_takeTresures.Count - 1);
            //重量とスコアを引く
            AddMass(-treasure.GetWeight());
            AddScore(-treasure.GetScore());
            Debug.Log(treasure.name);
            //宝の座標をプレイヤーと同じに
            last.transform.position = gameObject.transform.position + new Vector3(m_facingDirection * 0.5f* gameObject.transform.localScale.x, 1.5f*gameObject.transform.localScale.y, 0.0f) ;
            last.SetActive(true);
            last.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
            //向いている方向に飛ばす
            last.GetComponent<Rigidbody2D>().AddForce(new Vector2(m_facingDirection*200.0f, 90.0f));
            //プレイヤー判定オフ
            treasure.DisableColliderTemporarily();
        }
    }

    /**
     * 
     * 
     * 
     * 
     * 
     * 
     */
    void OnDead()
    {
        //シーンに切り替わる前にプレイヤーのデータを受け渡す
        Debug.Log(GetScore());
        m_takeTresures.Clear();

        //ゴールしたらシーン遷移(nullの場合はデバッグ用ログを出すだけ)
        if (m_deadScene != null) SceneManager.LoadScene(m_deadScene, LoadSceneMode.Single);
        else Debug.Log("GAMEOVER");
    }

    /**
    * @brief 重量のセット
    *
    * @param[in] なし
    *
    * @return なし
    */
    public void SetMass(float mass)
    {
        m_mass = mass;
    }

    /**
    * @brief 重量の追加
    *
    * @param[in] なし
    *
    * @return なし
    */
    public void AddMass(float mass)
    {
        m_mass += mass;
    }

    /**
    * @brief 重量の取得
    *
    * @param[in] なし
    *
    * @return なし
    */
    public float GetMass()
    {
        float massIndex = Mathf.Max(m_mass + m_minusMass, 0.0f);
        return massIndex + PLAYER_WIEGHT;
    }

    /**
    * @brief スコアの追加
    *
    * @param[in] なし
    *
    * @return なし
    */
    public void AddScore(int score)
    {
        m_score += score;
    }

    /**
    * @brief スコアの取得
    *
    * @param[in] なし
    *
    * @return なし
    */
    public int GetScore()
    {
        return m_score;
    }
    /**
    * @brief 宝石を手に入れる
    *
    * @param[in] treasure  宝石
    *
    * @return なし
    */
    public void TakeTreasure(GameObject treasureObject)
    {
        if (treasureObject == null)
        {
            return;
        }
        //宝スクリプト取得
        Treasure treasure = treasureObject.GetComponent<Treasure>();
        if (treasure == null)
        {
            return;
        }
        //非表示
        treasureObject.SetActive(false);
        //情報を記録
        m_takeTresures.Add(treasureObject);
        //重量加算
        AddMass(treasure.GetWeight());
        //スコア加算
        AddScore(treasure.GetScore());
    }
    /**
    * @brief 宝を登録
    *
    * @param[in] なし
    *
    * @return なし
    */
    public void RegisterTreasure()
    {
        for (int i = 0; i < m_takeTresures.Count; i++)
        {
            Treasure treasure = m_takeTresures[i].GetComponent<Treasure>();
            treasure.RegisterGetTreasure();
        }
    }
    public ItemKinds GetItem() { return m_itemKind; }

    /**
    * @brief 水との判定処理
    *
    * @param[in] なし
    *
    * @return なし
    */
    void OnTriggerEnter2D(Collider2D other)
    {
        if (m_footCollider == null) return;
        if (m_headCollider == null) return;

        if(other.CompareTag("NotWater"))
        {
            m_isFootHit = false;
            m_isHeadHit = false;
        }

        if (!other.CompareTag("Water")) return;

        Debug.Log("HIT!!!!");
        m_isFootHit = true;
        m_isHeadHit = true;
    }

    /**
    * @brief 水との判定処理
    *
    * @param[in] なし
    *
    * @return 頭とヒットしているかどうか
    */
    public bool GetHeadHit()
    {
        return m_isHeadHit;
    }

    /**
    * @brief ゲージに渡すレート
    *
    * @param[in] なし
    *
    * @return レート
    */
    public float GetLate()
    {
        return m_currentTime / m_deadTime;
    }


}
