/**
 * @file   GameManager.cs
 *
 * @brief  ゲーム管理に関するヘッダファイル
 *
 * @author 制作者名　福地貴翔
 *
 * @date   日付　2026/02/03
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// クラスの定義 ===============================================================
/**
  * @brief ゲーム管理
  */

public class GameManager : MonoBehaviour
{
// データメンバの宣言 -----------------------------------------------
    //ゲーム管理クラスインスタンス
    public static GameManager instance;
    //スコア
    private int m_score =  0;
    //選択された
    private ItemKinds m_selectItem;
    //スコアデータ
    private ScoreData m_scoreData;
    //ステージ
    private string m_stage = "first";
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
        //スコアデータ管理スクリプト作成
        m_scoreData = gameObject.AddComponent<ScoreData>();
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
            }
        }

    /**
    * @brief 現在スコアの設定
    *
    * @param[in] なし
    *
    * @return なし
    */
    public void SetCurrentScore(int score)
    {
        m_score = score;
    }

    /**
    * @brief 現在スコアの取得
    *
    * @param[in] なし
    *
    * @return スコア
    */
    public int GetCurrentScore()
    {
        return m_score;
    }

    /**
    * @brief ハイスコアの設定
    *
    * @param[in] stage  ハイスコアを設定するステージ
    *
    * @return　true  ハイスコアだった
    * @return　false ハイスコアではなかった
    */
    public bool SetHighScore(string stage,int score)
    {
        return m_scoreData.SetHighScore(stage,score);
    }

    /**
    * @brief ハイスコアの取得
    *
    * @param[in] stage  ハイスコアを取得するステージ
    *
    * @return 引数のステージのハイスコア
    */
    public int GetHighScore(string stage)
    {
        return m_scoreData.GetHighScore(stage);
    }

    /**
    * @brief アイテムの設定
    *
    * @param[in] なし
    *
    * @return なし
    */
    public void SetSelectItem(ItemKinds selectItem)
    {
        m_selectItem = selectItem;
    }

    /**
    * @brief 選択したアイテムの取得
    *
    * @param[in] なし
    *
    * @return アイテム
    */
    public ItemKinds GetSelectItem()
    {
        return m_selectItem;
    }

    /**
    * @brief 選択したステージ名の取得
    *
    * @param[in] なし
    *
    * @return ステージ名
    */
    public string GetStageName()
    {
        return m_stage;
    }

    /**
    * @brief 選択したステージ名の設定
    *
    * @param[in] stage  ステージ
    *
    * @return なし
    */
    public void SetStageName(string stage)
    {
        m_stage = stage;
    }
}
