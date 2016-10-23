using UnityEngine;
using System.Collections;

namespace SGD.Game
{
    public class Item : MonoBehaviour
    {
        // 所在行列的索引
        public int rowIndex = 9;
        public int columnIndex = 9;

        public float xOff = -2.25f;
        public float yOff = -3f;

        public bool moveable = true;
        public GameController controller;

        void OnMouseDown()
        {
            controller = GameObject.Find("GameController").GetComponent<GameController>();
            controller.Select(this);
        }


        //更新坐标
        public void UpdatePosition()
        {
            Vector3 V = new Vector3(0, 0, 0);
            V.x = 0.5f*columnIndex + xOff;
            if (rowIndex%2 == 0)
            {
                V.x = 0.5f*columnIndex + xOff + 0.25f;
            }

            V.y = 0.5f*rowIndex + yOff;
            transform.position = V;

        }

        //移动到目标行列索引坐标
        public void Goto(int row, int column)
        {
            this.rowIndex = row;
            this.columnIndex = column;
            UpdatePosition();
        }

        // 设置 精灵，堵截的   和   没有堵截的
        public void SetSprite(Sprite image)
        {
            GetComponent<SpriteRenderer>().sprite = image;
        }

    }
}
