using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SGD.Game
{
    public class GameController : MonoBehaviour
    {

        public ArrayList potArr;
        //public ArrayList pot2Arr;

        public GameObject pot1;
        //public GameObject pot2;
        public GameObject cat;

        public GameObject startGame;
        public GameObject failed;
        public GameObject weizhu;
        public GameObject replay;

        public Sprite _NormalSprite,
            _SelectedSprite;

        public int rowNum = 9;
        public int columnNum = 9;

        private bool started = false;
        private bool gameover = false;

        private int c = 0;
        public Text tt;

        // Use this for initialization
        void Start()
        {

            //初始化两个点的数组

            potArr = new ArrayList();
            //pot2Arr = new ArrayList();

            //绘制小灰点的矩形阵  9*9 
            for (int rowIndex = 0; rowIndex < rowNum; rowIndex++)
            {

                ArrayList tmp = new ArrayList();
                for (int columnIndex = 0; columnIndex < columnNum; columnIndex++)
                {
                    Item item = CreatePot(pot1, rowIndex, columnIndex);
                    item.SetSprite(_NormalSprite);
                    tmp.Add(item);
                }
                potArr.Add(tmp);

            }
        }

        //动态加载小点
        Item CreatePot(GameObject pot, int rowIndex, int columnIndex)
        {
            GameObject o = Instantiate(pot) as GameObject;
            o.transform.parent = this.transform;

            Item item = o.GetComponent<Item>();
            item.Goto(rowIndex, columnIndex);
            return item;
        }

        //开始游戏
        public void StartGame()
        {

            started = true;
            gameover = false;
            c = 0;

            //隐藏和显示的UI
            startGame.SetActive(false);
            failed.SetActive(false);
            weizhu.SetActive(false);
            replay.SetActive(false);
            cat.SetActive(true);


            //小猫的动画，小猫会随机一个位置
            MoveCat(Random.Range(3, rowNum - 3), Random.Range(3, columnNum - 3));

            //重置小灰点
            for (int rowIndex = 0; rowIndex < rowNum; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < columnNum; columnIndex++)
                {
                    Item item = GetPot(rowIndex, columnIndex);
                    item.moveable = true;
                }

            }


            ////清除掉所有橙色点
            //for (int i = 0; i < pot2Arr.Count; i++)
            //{
            //    Item pot2 = pot2Arr[i] as Item;
            //    Destroy(pot2.gameObject);
            //}
            //pot2Arr = new ArrayList();

        }

        //移动小猫
        void MoveCat(int rowIndex, int columnIndex)
        {
            Item item = cat.GetComponent<Item>();
            item.Goto(rowIndex, columnIndex);
        }


        //点击小灰点
        public void Select(Item item)
        {
            c++;

            //游戏开始前和游戏结束后不执行
            if (!started || gameover)
                return;

            //判断该item里面的moveable是否能被点击
            if (item.moveable)
            {
                //添加橙色点
                //Item pot2Item = CreatePot(pot2, item.rowIndex, item.columnIndex);
                //pot2Arr.Add(pot2Item);
                ArrayList tmp = potArr[item.rowIndex] as ArrayList;
                Item itemCom = tmp[item.columnIndex] as Item;
                itemCom.SetSprite(_SelectedSprite);


                //设置moveable不可点击
                item.moveable = false;

                //小猫的AI

                //小猫能够移动的点的数组
                ArrayList steps = FindSteps();
                if (steps.Count > 0)
                {
                    //小猫会移动
                    int index = Random.Range(0, steps.Count);
                    Vector2 v = (Vector2) steps[index];
                    MoveCat((int) v.y, (int) v.x);

                    //判断小猫是否逃离
                    if (Escaped())
                    {
                        gameover = true;
                        failed.SetActive(true);
                        replay.SetActive(true);
                    }
                }
                else
                {
                    gameover = true;
                    weizhu.SetActive(true);
                    replay.SetActive(true);
                    string cc = c.ToString();
                    tt.text = "你用了" + cc + "步成功围住神经猫！";
                }

            }


        }

        //寻找可移动点
        ArrayList FindSteps()
        {
            Item item = cat.GetComponent<Item>();
            int rowIndex = item.rowIndex;
            int columnIndex = item.columnIndex;

            ArrayList steps = new ArrayList();
            Vector2 v = new Vector2();

            //左边
            v.y = rowIndex;
            v.x = columnIndex - 1;
            if (MoveAble(v))
                steps.Add(v);

            //右边
            v.y = rowIndex;
            v.x = columnIndex + 1;
            if (MoveAble(v))
                steps.Add(v);

            //上
            v.y = rowIndex + 1;
            v.x = columnIndex;
            if (MoveAble(v))
                steps.Add(v);

            //下
            v.y = rowIndex - 1;
            v.x = columnIndex;
            if (MoveAble(v))
                steps.Add(v);

            //奇数行左上，偶数行的右上
            v.y = rowIndex + 1;
            if (rowIndex%2 == 1)
                v.x = columnIndex - 1;
            else
                v.x = columnIndex + 1;
            if (MoveAble(v))
                steps.Add(v);

            //奇数左下，偶数行右下
            v.y = rowIndex - 1;
            if (rowIndex%2 == 1)
                v.x = columnIndex - 1;
            else
                v.x = columnIndex + 1;
            if (MoveAble(v))
                steps.Add(v);

            return steps;
        }

        //判断移动点
        bool MoveAble(Vector2 v)
        {
            Item item = GetPot((int) v.y, (int) v.x);
            if (item == null)
                return false;
            return item.moveable;
        }

        //获得点的行列索引
        Item GetPot(int rowIndex, int columnIndex)
        {
            if (rowIndex < 0 || rowIndex > rowNum - 1 || columnIndex < 0 || columnIndex > columnNum - 1)
            {
                return null;
            }

            ArrayList tmp = potArr[rowIndex] as ArrayList;
            Item item = tmp[columnIndex] as Item;
            return item;
        }

        //判断小猫是否逃离
        bool Escaped()
        {
            Item item = cat.GetComponent<Item>();
            int rowIndex = item.rowIndex;
            int columnIndex = item.columnIndex;
            if (rowIndex == 0 || rowIndex == rowNum - 1 || columnIndex == 0 || columnIndex == columnNum - 1)
                return true;
            return false;
        }
    }
}
