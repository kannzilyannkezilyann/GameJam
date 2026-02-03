/**
 * @file   Logo.cs
 *
 * @brief  ロゴに関するヘッダファイル
 *
 * @author 制作者名　福地貴翔
 *
 * @date   日付　2026/02/02
 */
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
// クラスの定義 ===============================================================
/**
  * @brief ロゴ
  */
public class Logo : MonoBehaviour
{
// クラス定数の宣言 -------------------------------------------------
    const int START_FADE_IN  = 1;
    const int START_FADE_OUT = 0;
// データメンバの宣言 -----------------------------------------------

    //遷移先のシーン
    [SerializeField] SceneAsset m_scene;
    //表示するロゴ
    [SerializeField] Sprite[] m_logoSprites;
    //表示するためのスプライト
    SpriteRenderer m_spriteRenderer;
    //スプライトの色
    Color m_color;
    //変化時間
    private float m_duration = 1.5f;
    //表示するロゴの番号
    private int m_index = 0;
    //フェード状態
    private int m_fade = START_FADE_IN;

    // メンバ関数の定義 -------------------------------------------------
    /**
     * @brief 生成時処理
     *
     * @param[in] なし
     *
     * @return なし
     */
    void Start()
    {
        //FPS設定
        Application.targetFrameRate = 60;
        //表示するためのスプライト
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        //始めに表示するロゴ
        m_spriteRenderer.sprite = m_logoSprites[m_index];
        //スプライトの色
        m_color = m_spriteRenderer.color;
        //透明にする
        m_color.a = 0;

        SpriteScaling();
    }

    /**
     * @brief 更新処理
     *
     * @param[in] なし
     *
     * @return なし
     */
    void Update()
    {
        //スキップ
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            m_color.a = m_fade;
            m_spriteRenderer.color = m_color;
        }

        if (Fade(m_fade))
        {
            if (m_fade == START_FADE_IN)
            {
                m_fade = START_FADE_OUT;
            }
            else if (m_fade == START_FADE_OUT)
            {
                m_index++;
                //次のロゴを表示
                if (m_index < m_logoSprites.Length)
                {


                    m_spriteRenderer.sprite = m_logoSprites[m_index];
                    m_fade = START_FADE_IN;
                    SpriteScaling();
                }
                else
                {
                    //すべてのロゴが表示し終えたら遷移
                    SceneManager.LoadScene(m_scene.name);
                }

            }
        }
    }

    /**
     * @brief ロゴのフェード
     *
     * @param[in] targetAlpha  目標値
     *
     * @return true   フェード完了
     * @return false  フェード未了
     */
    private bool Fade(float targetAlpha)
    {
        if (!Mathf.Approximately(m_color.a, targetAlpha))
        {
            //変化率
            float changePerFrame = Time.deltaTime / m_duration;
            //少しずつ変化させる
            m_color.a = Mathf.MoveTowards(m_color.a, targetAlpha, changePerFrame);
            m_spriteRenderer.color = m_color;
            return false;
        }
        else
        {
            return true;
        }

    }
    /**
     * @brief スプライトのサイズ調整
     *
     * @param[in] なし
     *
     * @return なし
     */
    private void SpriteScaling()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        //カメラ取得
        Camera cam = Camera.main;
        float screenHeight = cam.orthographicSize * 2f;
        float screenWidth = screenHeight * cam.aspect;

        Vector2 spriteSize = sr.sprite.bounds.size;
        //画面いっぱいのサイズに調整
        transform.localScale = new Vector3(
            screenWidth / spriteSize.x,
            screenHeight / spriteSize.y,
            1f
        );

    }
}
