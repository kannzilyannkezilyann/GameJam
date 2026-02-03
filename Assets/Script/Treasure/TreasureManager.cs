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
    [System.Serializable]
    public struct TreasureData
    {
        public string name;
        public GameObject prefab;
    }
// データメンバの宣言 -----------------------------------------------

    public TreasureData[] treasureList;
    private Dictionary<string, GameObject> treasureDict;
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
        //宝登録
        treasureDict = new Dictionary<string, GameObject>();
        foreach (var t in treasureList)
        {
            treasureDict[t.name] = t.prefab;
        }
    }

    /**
     * @brief 初期化処理
     *
     * @param[in] なし
     *
     * @return なし
     */
    public GameObject SpawnTreasure(string name, Vector3 position)
    {
        if (!treasureDict.TryGetValue(name, out GameObject prefab))
        {
            Debug.LogError($"Treasure {name} が見つかりません");
            return null;
        }
        return Instantiate(prefab, position, Quaternion.identity);
    }
}
