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

        //MergeSort(range, barInterval);

        BubbleSort(range, barInterval);
    }

    void Swap(int index1, int index2)
    {
        float tempCoorX = bars[index1].transform.position.x;
        bars[index1].transform.position = new Vector2(bars[index2].transform.position.x, bars[index1].transform.position.y);
        bars[index2].transform.position = new Vector2(tempCoorX, bars[index2].transform.position.y);

        GameObject temp = bars[index1];
        bars[index1] = bars[index2];
        bars[index2] = temp;
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
    
    void MergeSort(int barLenghtRange, float barIncerese)
    {
        /*Generate Bars*/
        SetUp(range, barIncerese);

        /*Set up merge parts repository*/
        //Item1 remember start point this merged part
        //Item2 remember this merged part's length
        //Item3 remember this merged part has been again merge
        Stack<Tuple<int, int, int>> dp = new Stack<Tuple<int, int, int>>();
        dp.Push(new Tuple<int, int, int>(0, range, 0));
        
        /*Declare Compare repositoty list, when compare left part and right part*/
        //Item1 remember Bar's height
        //Item2 remember Bar's index
        List<Tuple<float, int>> listTemp = new List<Tuple<float, int>>();

        /*Sortting funtion*/
        IEnumerator MergeSort(float second, int leftStart = -1, int rightStart = -1, int index = 0)
        {

            /*wait*/
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

                    dp.Pop();

                    if (bars[temp.Item1].transform.localScale.y > bars[temp.Item1 + 1].transform.localScale.y)
                        Swap(temp.Item1, temp.Item1 + 1);

                    StartCoroutine(MergeSort(second, leftStart, rightStart, index));
                }

                else if (temp.Item2 == 1)
                {
                    dp.Pop();
                    StartCoroutine(MergeSort(second, leftStart, rightStart, index));
                }

                else
                {
                    var left = new Tuple<int, int, int>(temp.Item1, temp.Item2 / 2, 0);
                    var right = new Tuple<int, int, int>(temp.Item1 + temp.Item2 / 2, temp.Item2 / 2 + (temp.Item2 % 2 == 1 ? 1 : 0), 0);

                    dp.Push(left);
                    dp.Push(right);

                    StartCoroutine(MergeSort(0, leftStart, rightStart, index));

                }
            }

            else
            {
                if (leftStart == -1 && rightStart == -1)
                {
                    if (listTemp != null) listTemp.Clear();

                    for (int i = 0; i < temp.Item2; i++)
                    {
                        listTemp.Add(new Tuple<float, int>(bars[temp.Item1 + i].transform.localScale.y, temp.Item1 + i));
                    }

                    leftStart = 0;
                    rightStart = temp.Item2 / 2;
                    index = temp.Item1;

                    StartCoroutine(MergeSort(0, leftStart, rightStart, index));
                }
                else
                {
                    for (int i = 0; i < temp.Item2; i++)
                    {
                        if (listTemp[i].Item2 == index)
                        {


                            if (rightStart >= listTemp.Count)
                            {
                                Swap(listTemp[leftStart].Item2, index);

                                int tempData = listTemp[leftStart].Item2;
                                listTemp[leftStart] = new Tuple<float, int>(listTemp[leftStart].Item1, listTemp[i].Item2);
                                listTemp[i] = new Tuple<float, int>(listTemp[i].Item1, tempData);

                                leftStart++;
                            }

                            else if (leftStart >= temp.Item2 / 2 || listTemp[rightStart].Item1 < listTemp[leftStart].Item1)
                            {
                                Swap(listTemp[rightStart].Item2, index);

                                int tempData = listTemp[rightStart].Item2;
                                listTemp[rightStart] = new Tuple<float, int>(listTemp[rightStart].Item1, listTemp[i].Item2);
                                listTemp[i] = new Tuple<float, int>(listTemp[i].Item1, tempData);

                                rightStart++;

                            }

                            else
                            {
                                Swap(listTemp[leftStart].Item2, index);

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

                    if (dp.Count == 0)
                    {
                        StartCoroutine(CheckArray(second));
                    }

                    else StartCoroutine(MergeSort(second, leftStart, rightStart, index));

                }


            }
        }

        /*Start sortting*/
        StartCoroutine(MergeSort(updateInterval));

    }

    void BubbleSort(int barLenghtRange, float barIncerese)
    {
        /*Generate Bars*/
        SetUp(range, barIncerese);

        /*Sorttring funtion*/
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

                if (bars[j].transform.localScale.y > bars[j + 1].transform.localScale.y)
                    Swap(j, j + 1);

                changeColor = bars[j].GetComponent<SpriteRenderer>();
                changeColor.color = Color.white;

                j++;
                if (j == range - 1 - i)
                {
                    changeColor = bars[j].GetComponent<SpriteRenderer>();
                    changeColor.color = Color.white;
                    i++;
                    j = 0;
                }

                StartCoroutine(BubbleSort(second, i, j));

            }

            else
                StartCoroutine(CheckArray(updateInterval));
        }

        /*Start sortting*/
        StartCoroutine(BubbleSort(updateInterval));
    }
    
    

    void Update()
    {
        
    }
}
