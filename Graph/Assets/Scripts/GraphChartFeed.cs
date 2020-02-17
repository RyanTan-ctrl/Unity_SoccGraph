#define Graph_And_Chart_PRO
using UnityEngine;
using ChartAndGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GraphChartFeed : MonoBehaviour
{
    Dictionary<string, Vector2[]> players = new Dictionary<string, Vector2[]>();
    Dictionary<string, Vector2[]> bPlayers = new Dictionary<string, Vector2[]>();

    private List<string> activeGraph = new List<string>();
    GraphChartBase graph = null;

    public Dropdown m_Dropdown;
    public Dropdown m_DropdownB;

    private int teamALineIndex = 0;
    private int teamBLineIndex = 0;

    private string[] teamALineStyles = new string[4];
    private string[] teamBLineStyles = new string[4];

    void Start ()
    {
        fillDic(players, " A", m_Dropdown);
        fillDic(bPlayers, " B", m_DropdownB);

        teamALineStyles[0] = ("GraphLine");
        teamALineStyles[1] = ("LineStyle");
        teamALineStyles[2] = ("lineStyle3");
        teamALineStyles[3] = ("lineStyle10");

        teamBLineStyles[0] = ("LineStyle2");
        teamBLineStyles[1] = ("lineStyle4");
        teamBLineStyles[2] = ("lineStyle11");
        teamBLineStyles[3] = ("lineStyle12");

        m_Dropdown.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged(m_Dropdown, players, ref teamALineIndex, teamALineStyles);
        });

        m_DropdownB.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged(m_DropdownB, bPlayers, ref teamBLineIndex, teamBLineStyles);
        });

        graph = GetComponent<GraphChartBase>();
        //if (graph != null)
        //{
            //graph.HorizontalValueToStringMap[0.0] = "Zero"; // example of how to set custom axis strings
        //    graph.DataSource.StartBatch();
        //    graph.DataSource.ClearCategory("Player 1");
        //    graph.DataSource.ClearAndMakeBezierCurve("Player 2");
            
        //    for (int i = 0; i <= 90; i += 5)
        //    {
        //        graph.DataSource.AddPointToCategory("Player 1",i,Random.value*10f + 20f);
        //        if (i == 0)
        //            graph.DataSource.SetCurveInitialPoint("Player 2",i, Random.value * 10f + 10f);
        //        else
        //            graph.DataSource.AddLinearCurveToCategory("Player 2", new DoubleVector2(i , Random.value * 10f + 10f));
        //    }
        //    graph.DataSource.MakeCurveCategorySmooth("Player 2");
        //    graph.DataSource.EndBatch();
        //}
       // StartCoroutine(ClearAll());
    }

    IEnumerator ClearAll()
    {
        yield return new WaitForSeconds(5f);
        GraphChartBase graph = GetComponent<GraphChartBase>();

        graph.DataSource.Clear();
    }

    private void fillDic(Dictionary<string, Vector2[]> dic, string tempString, Dropdown dropdownList)
    {
        //function variable initialization
        List<string> playerNames = new List<string>();

        //dropdown data creation
        for (int i = 0; i <= 11; ++i)
        {
            int storeIndex = 0;
            string playerName = "Player " + i + tempString;
            Vector2[] pointPos = new Vector2[19];
            for (int e = 0; e <= 90; e += 5)
            {
                storeIndex = e / 5;
                pointPos[storeIndex] = new Vector2(e, Random.value * 10f + 10f);
            }
            dic.Add(playerName, pointPos);

            playerNames.Add(playerName);
        }

        //dropdown 
        dropdownList.AddOptions(playerNames);
    }

    private void DropdownValueChanged(Dropdown change, Dictionary<string, Vector2[]> dic, ref int index, string[] graphLineStyles)
    {
        Debug.Log(index);
        //Debug.Log(change.options[change.value].text);
        string playerName = change.options[change.value].text;
        Vector2[] dropdownArray = dic[playerName];

        if (!activeGraph.Contains(playerName) && activeGraph.Count < 4)
        {
            //CREATE GRAPH IF THE GRAPH FOR PLAYER EXISTS
            graph.DataSource.StartBatch();

            graph.DataSource.AddCategory(playerName, Resources.Load(graphLineStyles[index], typeof(Material)) as Material, 1, new ChartAndGraph.MaterialTiling(true, 0.5f), null, false, Resources.Load("point", typeof(Material)) as Material, 0);
            foreach (Vector2 vec in dropdownArray)
            {
                graph.DataSource.AddPointToCategory(playerName, vec.x, vec.y);
            }
            activeGraph.Add(playerName);
            index += 1;

            graph.DataSource.EndBatch();
        }
        else if (activeGraph.Contains(playerName))
        {
            //REMOVE GRAPH IF THE GRAPH FOR PLAYER EXISTS
            graph.DataSource.StartBatch();

            graph.DataSource.RemoveCategory(playerName);
            activeGraph.Remove(playerName);
            index--;

            graph.DataSource.EndBatch();
        }
    }
}
