using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StageDestroyer : MonoBehaviour
{

    [SerializeField] private Tilemap m_tileMap; ///< 崩すタイルマップ

    [SerializeField] float m_timer = 0; ///< タイマー

    [SerializeField] int m_destroyInterval = 5; ///< ブロックを破壊する間隔（秒単位）

    [SerializeField] int m_destroyCount = 1; ///< ステージを一気に破壊する列数


    // Start is called before the first frame update
    void Start()
    {
        //タイマーリセット
        m_timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //時間加算
        m_timer += Time.deltaTime;

        
        //間隔が来たら破壊させる
        if(m_timer > m_destroyInterval)
        {


            m_timer = 0;
        }

    }
}
