/**
 * @file   TreasureManager.cs
 *
 * @brief  宝管理に関するヘッダファイル
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
  * @brief 宝管理
  */
public class TreasureManager : MonoBehaviour
{
    public struct TreasureData
    {
        public int masterID; //種類識別ID
        public int id;//個別ID
        public Vector3 scale;        // 個別スケール
        public int score;            // 個別スコア
        public float weight;           // 個別重量
    }
    [System.Serializable]
    public class TreasurePrefabEntry
    {
        public int masterID;
        public GameObject prefab;
    }

    // データメンバの宣言 -----------------------------------------------
    [SerializeField]
    private List<TreasurePrefabEntry> m_treasurePrefabList;

    private Dictionary<int, GameObject> m_treasurePrefabDict;
    //宝管理クラスインスタンス
    public static TreasureManager instance;
    private int m_countTreasureID = -1;

    //ステージ上ある宝のデータ
    private Dictionary<int,TreasureData> m_treasureData = new();
// メンバ関数の定義 -------------------------------------------------
    /**
     * @brief 初期化処理
     *
     * @param[in] なし
     *
     * @return なし
     */
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            m_treasurePrefabDict = new Dictionary<int, GameObject>();
            foreach (var e in m_treasurePrefabList)
            {
                m_treasurePrefabDict[e.masterID] = e.prefab;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /**
     * @brief 初期化処理
     *
     * @param[in] なし
     *
     * @return なし
     */
    public GameObject GetPrefab(int masterID)
    {
        return m_treasurePrefabDict[masterID];
    }


    /**
     * @brief プレイヤーが取得した宝のデータの登録
     *
     * @param[in] data 宝のデータ
     *
     * @return なし
     */
    public void RegistrationTreasure(TreasureData data)
    {
        m_treasureData[data.id] = data;
    }

    /**
     * @brief プレイヤーが取得した宝のデータの解除
     *
     * @param[in] data 宝のデータ
     *
     * @return なし
     */
    public void UnRegisterTreasure()
    {
        m_treasureData.Clear();
    }

    /**
     * @brief プレイヤーが取得した宝のデータ
     *
     * @param[in] なし
     *
     * @return 宝データ
     */
    public Dictionary<int,TreasureData> GetGotTreasureData()
    {
        return m_treasureData;
    }

    /**
     * @brief 加算したIDを取得
     *
     * @param[in] なし
     *
     * @return ID
     */
    public int CountUpID()
    {
        ++m_countTreasureID;
        return m_countTreasureID;
    }

    /**
     * @brief IDをリセット
     *
     * @param[in] なし
     *
     * @return なし
     */
    public void ResetID()
    {
        m_countTreasureID = 0;
    }

}
