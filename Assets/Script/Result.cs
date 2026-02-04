using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{
    //遷移先のシーン
    [SerializeField] private string m_scene;

    [SerializeField] GameObject m_scoreText;
    [SerializeField] Canvas m_canvas;
    //生成したテキスト
    [SerializeField] CountUpText m_countUpText;
    // Start is called before the first frame update
    void Start()
    {
        if (m_countUpText == null)
        {
            Debug.LogError("CountUpText component missing");
            return;
        }

        m_countUpText.Initialize(0, GameManager.instance.GetScore(), 800);
        Debug.Log("Initialize OK");
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && m_countUpText.IsFinish())
        {
            SceneManager.LoadScene(m_scene, LoadSceneMode.Single);
        }
    }
}
