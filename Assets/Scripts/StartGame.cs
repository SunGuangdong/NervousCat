using UnityEngine;
using System.Collections;

namespace SGD.Game
{
    public class StartGame : MonoBehaviour
    {
        public GameController controller;

        // 开始 和 重新开始 按钮的响应
        void OnMouseDown()
        {
            controller.StartGame();
            //Debug.Log ("现在开始游戏来！");
        }

    }
}

