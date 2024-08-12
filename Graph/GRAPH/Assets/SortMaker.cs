using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class SortMaker : MonoBehaviour
{
    public int range = 50;
    public float incresePoint = 1;
    public float barInterval = 0.35f;
    public float updateInterval = 0.01f;
    public GameObject prefab;

    List<GameObject> bars = new List<GameObject>();
    void SetUp(int range, float interval = 0)
    {
        /*make height list*/
        List<float> list = new List<float>();
        for (int i = 1; i <= range; i++)
        {
            list.Add(i * incresePoint);
        }


        float x = -range / 2 * (1 + interval);

        /*copy object*/
        while (list.Count > 0)
        {

            int random = UnityEngine.Random.Range(0, list.Count);
            float r = list[random];
            list.Remove(r);

            GameObject newObject = Instantiate(prefab);

            newObject.transform.parent = null;
            newObject.transform.localScale = new Vector2(1, r);
            newObject.transform.position = new Vector2(x, ((float)r - 1) / 2);
            //Debug.Log(-newObject.transform.localScale.y);

            bars.Add(newObject);
            x += 1 + interval;

            //Debug.Log(r);
        }
    }


    void Start()
    {
        SetUp(range, barInterval);

        //check();
        //StartCoroutine(MergeSort(updateInterval));

        StartCoroutine(BubbleSort(updateInterval));
    }

    void CompareSwap(int index1, int index2)
    {
        if (bars[index1].transform.localScale.y > bars[index2].transform.localScale.y)
        {
            float tempCoorX = bars[index1].transform.position.x;
            bars[index1].transform.position = new Vector2(bars[index2].transform.position.x, bars[index1].transform.position.y);
            bars[index2].transform.position = new Vector2(tempCoorX, bars[index2].transform.position.y);

            GameObject temp = bars[index1];
            bars[index1] = bars[index2];
            bars[index2] = temp;


        }
    }
    void NormalSwap(int index1, int index2)
    {
        float tempCoorX = bars[index1].transform.position.x;
        bars[index1].transform.position = new Vector2(bars[index2].transform.position.x, bars[index1].transform.position.y);
        bars[index2].transform.position = new Vector2(tempCoorX, bars[index2].transform.position.y);

        GameObject temp = bars[index1];
        bars[index1] = bars[index2];
        bars[index2] = temp;
    }


    Stack<Tuple<int, int, int>> dp = new Stack<Tuple<int, int, int>>();
    int leftStart = -1, rightStart = -1, index = 0;
    void check()
    {

        dp.Push(new Tuple<int, int, int>(0, range, 0));
    }

    List<Tuple<float, int>> listTemp = new List<Tuple<float, int>>();

    IEnumerator MergeSort(float second)
    {

        yield return new WaitForSeconds(second);

        second = updateInterval;

        Tuple<int, int, int> temp = dp.Peek();

        if (temp.Item3 == 0)
        {
            dp.Pop();
            var change = new Tuple<int, int, int>(temp.Item1, temp.Item2, 1);
            dp.Push(change);

            //item1 = start point item2 = length
            if (temp.Item2 == 2)
            {
                //Debug.Log(temp);
                Debug.Log(-1);

                dp.Pop();
                
                CompareSwap(temp.Item1, temp.Item1 + 1);
                
                StartCoroutine(MergeSort(second));
            }

            else if (temp.Item2 == 1)
            {
                Debug.Log(-2);
                dp.Pop();
                StartCoroutine(MergeSort(second));
            }

            else
            {
                var left = new Tuple<int, int, int>(temp.Item1, temp.Item2 / 2, 0);
                var right = new Tuple<int, int, int>(temp.Item1 + temp.Item2 / 2, temp.Item2 / 2 + (temp.Item2 % 2 == 1 ? 1 : 0), 0);

                dp.Push(left);
                dp.Push(right);
                Debug.Log(left);
                Debug.Log(right);
                StartCoroutine(MergeSort(0));

            }
        }

        else
        {
            if (leftStart == -1 && rightStart == -1)
            {
                Debug.Log(temp);

                if(listTemp != null) listTemp.Clear();

                for (int i = 0; i < temp.Item2; i++)
                {
                    listTemp.Add(new Tuple<float, int>(bars[temp.Item1 + i].transform.localScale.y, temp.Item1 + i));
                    Debug.Log(listTemp[i]);
                }

                leftStart = 0;
                rightStart = temp.Item2 / 2;
                index = temp.Item1;

                //Debug.Log(782346782349678234);
                //Debug.Log(rightStart);
                //Debug.Log(listTemp.Count);

                StartCoroutine(MergeSort(0));
            }
            else
            {
                Debug.Log(123);
                for(int i = 0; i < temp.Item2; i++)
                {
                    if (listTemp[i].Item2 == index)
                    {

                        //Debug.Log(26);

                        if(rightStart >= listTemp.Count)
                        {
                            //Debug.Log(765);
                            NormalSwap(listTemp[leftStart].Item2, index);

                            int tempData = listTemp[leftStart].Item2;
                            listTemp[leftStart] = new Tuple<float, int>(listTemp[leftStart].Item1, listTemp[i].Item2);
                            listTemp[i] = new Tuple<float, int>(listTemp[i].Item1, tempData);

                            leftStart++;
                        }

                        else if (leftStart >= temp.Item2 / 2 || listTemp[rightStart].Item1 < listTemp[leftStart].Item1)
                        {
                            //Debug.Log(567);
                            NormalSwap(listTemp[rightStart].Item2, index);
                            
                            int tempData = listTemp[rightStart].Item2;
                            listTemp[rightStart] = new Tuple<float, int>(listTemp[rightStart].Item1, listTemp[i].Item2);
                            listTemp[i] = new Tuple<float, int>(listTemp[i].Item1, tempData);

                            rightStart++;

                        }

                        else
                        {
                            //Debug.Log(765);
                            NormalSwap(listTemp[leftStart].Item2, index);

                            int tempData = listTemp[leftStart].Item2;
                            listTemp[leftStart] = new Tuple<float, int>(listTemp[leftStart].Item1, listTemp[i].Item2);
                            listTemp[i] = new Tuple<float, int>(listTemp[i].Item1, tempData);

                            leftStart++;
                        }
    
                        index++;
                        break;
                    }
                }

                if (index >= temp.Item1 + temp.Item2)
                {
                    dp.Pop();
                    leftStart = -1;
                    rightStart = -1;
                }

                Debug.Log(-index);

                StartCoroutine(MergeSort(second));
                
            }


        }
    }
    IEnumerator BubbleSort(float second, int i = 0, int j = 0)
    {
        yield return new WaitForSeconds(second);

        SpriteRenderer changeColor;
        
        if (i < range - 1)
        {
            changeColor = bars[j].GetComponent<SpriteRenderer>();
            changeColor.color = Color.red;

            changeColor = bars[j + 1].GetComponent<SpriteRenderer>();
            changeColor.color = Color.red;

            CompareSwap(j, j + 1);

            changeColor = bars[j].GetComponent<SpriteRenderer>();
            changeColor.color = Color.white;
            
            j++;
            if (j == range - 1 - i)
            {
                changeColor = bars[j].GetComponent<SpriteRenderer>();
                changeColor.color = Color.white;

                //changeColor = bars[j].GetComponent<SpriteRenderer>();
                //changeColor.color = Color.green;
                i++;
                j = 0;
            }
 
            StartCoroutine(BubbleSort(second, i, j));

        }

        else 
            StartCoroutine(CheckArray(updateInterval));
    }
    IEnumerator CheckArray(float second, int i = 0)
    {
        yield return new WaitForSeconds(second);

        if (i < range)
        {
            SpriteRenderer changeColor = bars[i].GetComponent<SpriteRenderer>();
            changeColor.color = Color.green;
            i++;

            StartCoroutine(CheckArray(second,i));
        }
    }

    void Update()
    {
        
    }
}
