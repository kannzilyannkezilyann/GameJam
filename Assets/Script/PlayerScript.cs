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
// クラス定数の宣言 -------------------------------------------------
    //プレイヤーの重さ
    public const float PLAYER_WIEGHT = 100.0f;
    
// データメンバの宣言 -----------------------------------------------
    //現在の重さ
    float m_mass = 100.0f;
    //合計スコア
    int m_score = 0;
    //入手宝配列
    public List<GameObject> m_takeTresures ;
    //宝管理クラス
    [SerializeField] TreasureManager m_treasureManager;
    //false：タイトル用　true：ゲーム用
    [SerializeField] bool m_flag;
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
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");
        if (!m_flag)
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

        float speedIndex = 1 / (m_mass / 100.0f);
        // 移動
        if (!m_rolling )
            m_body2d.velocity = new Vector2(inputX * m_speed * speedIndex, m_body2d.velocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // -- Handle Animations --
        //Wall Slide
        //m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        //m_animator.SetBool("WallSlide", m_isWallSliding);

        //Death
        //if (Input.GetKeyDown("e") && !m_rolling)
        //{
        //    m_animator.SetBool("noBlood", m_noBlood);
        //    m_animator.SetTrigger("Death");
        //}
            
        //Hurt
        //else if (Input.GetKeyDown("q") && !m_rolling)
        //    m_animator.SetTrigger("Hurt");

        //Attack
        //if(Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling && m_flag)
        //{
        //    m_currentAttack++;

        //    // Loop back to one after third attack
        //    if (m_currentAttack > 3)
        //        m_currentAttack = 1;

        //    // Reset Attack combo if time since last attack is too large
        //    if (m_timeSinceAttack > 1.0f)
        //        m_currentAttack = 1;

        //    // Call one of three attack animations "Attack1", "Attack2", "Attack3"
        //    m_animator.SetTrigger("Attack" + m_currentAttack);

        //    // Reset timer
        //    m_timeSinceAttack = 0.0f;
        //}

        // Block
        //else if (Input.GetMouseButtonDown(1) && !m_rolling && m_flag)
        //{
        //    m_animator.SetTrigger("Block");
        //    m_animator.SetBool("IdleBlock", true);
        //}

        //else if (Input.GetMouseButtonUp(1) && m_flag)
        //    m_animator.SetBool("IdleBlock", false);

        // ローリング
        //if (Input.GetKeyDown("left shift") && !m_rolling && !m_isWallSliding && m_flag)
        //{
        //    m_rolling = true;
        //    m_animator.SetTrigger("Roll");
        //    m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
        //}
            

        //ジャンプモーション
        if (Input.GetKeyDown("space") && m_grounded && !m_rolling)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce * speedIndex);
            m_groundSensor.Disable(0.2f);
        }

        //移動モーション
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //待機モーション
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
                if(m_delayToIdle < 0)
                    m_animator.SetInteger("AnimState", 0);
        }

        //宝を捨てる
        if (Input.GetKeyDown(KeyCode.E))
        {
            ThrowTreasure();
        }

        //死亡判定
        if(this.transform.position.y < m_deadY)
        {
            OnDead();
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
        GameManager.instance.SetCurrentScore(GetScore());
        Debug.Log(GetScore());

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
        return m_mass;
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
}
