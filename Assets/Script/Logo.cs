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
    [SerializeField] SceneAsset scene;
    //表示するロゴ
    [SerializeField] Sprite[] logoSprites;
    //表示するためのスプライト
    SpriteRenderer SpriteRenderer;
    Color color;
    private float duration = 1.5f;
    int index = 0;
    int fade = 1;

// メンバ関数の定義 -------------------------------------------------
    /**
     * @brief 初期化処理
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
        SpriteRenderer = GetComponent<SpriteRenderer>();
        //始めに表示するロゴ
        SpriteRenderer.sprite = logoSprites[index];
        //スプライトの色
        color = SpriteRenderer.color;
        //透明にする
        color.a = 0;
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
        if (Input.GetMouseButtonDown(0)) 
        {
            color.a = fade;
            SpriteRenderer.color = color;
        }

        if (Fade(fade))
        {
            if (fade == START_FADE_IN)
            {
                fade = START_FADE_OUT;
            }
            else if (fade == START_FADE_OUT)
            {
                index++;
                //次のロゴを表示
                if (index < logoSprites.Length)
                {
                    SpriteRenderer.sprite = logoSprites[index];
                    fade = START_FADE_IN;
                }
                else
                {
                    //すべてのロゴが表示し終えたら遷移
                    SceneManager.LoadScene(scene.name);
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
        if (!Mathf.Approximately(color.a, targetAlpha))
        {
            float changePerFrame = Time.deltaTime / duration;
            color.a = Mathf.MoveTowards(color.a, targetAlpha, changePerFrame);
            SpriteRenderer.color = color;
            return false;
        }
        else
        {
            return true;
        }

    }

}
