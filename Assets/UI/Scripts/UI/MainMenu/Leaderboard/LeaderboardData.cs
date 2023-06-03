//using UnityEngine;
//using System.Collections;
//using System.Text;
//using System.Collections.Generic;
//
//public class LeaderboardData : MonoBehaviour 
//{
//    public string leaderboardUrl, myPlaceUrl, monthlyLeaderboardUrl, weeklyLeaderboardUrl;
//    public OnlineUserInfo[] usersInfo, weeklyUsersInfo, monthlyUsersInfo, myNeighboursInfo;
//    public string[] monthChampions, weekChampions;
//
//
//    float weeklyTime, monthlyTime, allTheTime;
//
//    int myRank = -1;
//    public int MyRank
//    {
//        get { return myRank; }
//    }
//
//
//    public void DownloadLeaderboard(LeaderboardType type, int count, System.Action<string> doneMethod)
//    {
//        switch (type)
//        {
//            case LeaderboardType.League:
//                if (usersInfo != null && count == usersInfo.Length && Time.time - allTheTime < 600)
//                {
//                    if (doneMethod != null)
//                        doneMethod("ok");
//                }
//                else
//                    StartCoroutine(_DownloadLeaderboard(leaderboardUrl, type, count, doneMethod));
//                break;
//            case LeaderboardType.Monthly:
//
//                /*if (monthlyUsersInfo == null)
//                    Debug.Log("no users");
//                else
//                    Debug.Log("time=" + Time.time + ", month time=" + monthlyTime + ", count=" + monthlyUsersInfo.Length);*/
//
//                if (monthlyUsersInfo != null /*&& count == monthlyUsersInfo.Length*/ && Time.time - monthlyTime < 600)
//                {
//                    if (doneMethod != null)
//                        doneMethod("ok");
//                }
//                else
//                    StartCoroutine(_DownloadLeaderboard(monthlyLeaderboardUrl, type, count, doneMethod));
//                break;
//            case LeaderboardType.Global:
//                if (weeklyUsersInfo != null /*&& count == weeklyUsersInfo.Length*/ && Time.time - weeklyTime < 600)
//                {
//                    if (doneMethod != null)
//                        doneMethod("ok");
//                }
//                else
//                    StartCoroutine(_DownloadLeaderboard(weeklyLeaderboardUrl, type, count, doneMethod));
//                break;
//        }
//    }
//
//    IEnumerator _DownloadLeaderboard(string url, LeaderboardType type, int count, System.Action<string> doneMethod)
//    {
//        //string format = "{count:'{0}', username:'{1}'}";
//        //string ourPostData = string.Format(format, count, Data.UserName);
//
//
//        string ourPostData = string.Concat("{count:'", count, "', username:'", Data.UserName, "'}");
//        //string ourPostData = string.Concat("{count:'", count, "', username:'miladzxx'}");
//        
//
//        Dictionary<string, string> headers = new Dictionary<string, string>();
//        headers.Add("Content-Type", "application/json");
//        headers.Add("Accept", "application/json, text/javascript, */*");
//        headers.Add("Method", "POST");
//
//        byte[] pData = Encoding.ASCII.GetBytes(ourPostData.ToCharArray());
//        WWW www = new WWW(url, pData, headers);
//        //WWW www = new WWW(leaderboardUrl, pData);
//
//        yield return www;
//
//
//        //Debug.Log(www.text);
//
//        if (!string.IsNullOrEmpty(www.error))
//        {
//            if (doneMethod != null)
//                doneMethod("systemerror:" + www.error);
//        }
//        else
//        {
//            WyrmTale.JSON json = new WyrmTale.JSON();
//            json.serialized = www.text;
//
//            string d=  json.ToString("d");
//            json.serialized = d;
//
//            string[] names = json.ToArray<string>("Name");
//            string[] scores = json.ToArray<string>("Xperience");
//
//            switch (type)
//            {
//                case LeaderboardType.League:
//                    usersInfo = new OnlineUserInfo[names.Length];
//                    for (int i = 0; i < names.Length; i++)
//                    {
//                        usersInfo[i] = new OnlineUserInfo();
//                        usersInfo[i].rank = i + 1;
//                        usersInfo[i].score = int.Parse(scores[i]);
//                        usersInfo[i].username = names[i];
//                    }
//
//                    allTheTime = Time.time;
//                    break;
//
//                case LeaderboardType.Monthly:
//                    monthlyUsersInfo = new OnlineUserInfo[names.Length];
//                    for (int i = 0; i < names.Length; i++)
//                    {
//                        monthlyUsersInfo[i] = new OnlineUserInfo();
//                        monthlyUsersInfo[i].rank = i + 1;
//                        monthlyUsersInfo[i].score = int.Parse(scores[i]);
//                        monthlyUsersInfo[i].username = names[i];
//                    }
//
//                    monthlyTime = Time.time;
//                    break;
//
//                case LeaderboardType.Global:
//                    weeklyUsersInfo = new OnlineUserInfo[names.Length];
//                    for (int i = 0; i < names.Length; i++)
//                    {
//                        weeklyUsersInfo[i] = new OnlineUserInfo();
//                        weeklyUsersInfo[i].rank = i + 1;
//                        weeklyUsersInfo[i].score = int.Parse(scores[i]);
//                        weeklyUsersInfo[i].username = names[i];
//                    }
//
//                    weeklyTime = Time.time;
//                    break;
//            }
//
//
//            
//
//            if (doneMethod != null)
//                doneMethod("ok");
//        }
//    }
//
//
//    public void DownloadMyRank(string Username, System.Action<string> doneMethod)
//    {
//        if (myRank != -1)
//        {
//            if (doneMethod != null)
//                doneMethod("ok");
//        }
//        else
//            StartCoroutine(_DownloadMyRank(Username, doneMethod));
//    }
//
//    IEnumerator _DownloadMyRank(string Username, System.Action<string> doneMethod)
//    {
//        string format = "{Username : '*'}";
//        string ourPostData = format.Replace("*", Username);
//
//        Dictionary<string, string> headers = new Dictionary<string, string>();
//        headers.Add("Content-Type", "application/json");
//        headers.Add("Accept", "application/json, text/javascript, */*");
//        headers.Add("Method", "POST");
//
//        byte[] pData = Encoding.ASCII.GetBytes(ourPostData.ToCharArray());
//
//        WWW www = new WWW(myPlaceUrl, pData, headers);
//        yield return www;
//
//        if (!string.IsNullOrEmpty(www.error))
//        {
//            if (doneMethod != null)
//                doneMethod("systemerror:" + www.error);
//        }
//        else
//        {
//            WyrmTale.JSON json = new WyrmTale.JSON();
//            json.serialized = www.text;
//
//            string d = json.ToString("d");
//            json.serialized = d;
//
//
//            myRank = json.ToInt("rank");
//            string[] upper = json.ToArray<string>("Upper");
//            string[] lower = json.ToArray<string>("Lower");
//
//            myNeighboursInfo = new OnlineUserInfo[upper.Length + lower.Length + 1];
//            for (int i = 0; i < upper.Length; i++)
//            {
//                string[] info = upper[i].Split(';');
//
//                int index = (upper.Length - 1) - i;
//                myNeighboursInfo[index] = new OnlineUserInfo();
//                myNeighboursInfo[index].rank = int.Parse(info[0]);
//                myNeighboursInfo[index].username = info[1];
//                myNeighboursInfo[index].score = int.Parse(info[2]);
//            }
//
//            int myIndex = upper.Length;
//            myNeighboursInfo[myIndex] = new OnlineUserInfo();
//            myNeighboursInfo[myIndex].rank = myRank;
//            myNeighboursInfo[myIndex].username = Username;
//            myNeighboursInfo[myIndex].score = Accounting.Instance.currentUser.TotalScore;
//
//            int lowStartIndex = myIndex + 1;
//
//            for (int i = 0; i < lower.Length; i++)
//            {
//                string[] info = lower[i].Split(';');
//                myNeighboursInfo[lowStartIndex + i] = new OnlineUserInfo();
//                myNeighboursInfo[lowStartIndex + i].rank = int.Parse(info[0]);
//                myNeighboursInfo[lowStartIndex + i].username = info[1];
//                myNeighboursInfo[lowStartIndex + i].score = int.Parse(info[2]);
//            }
//
//            for (int i = 0; i < myNeighboursInfo.Length; i++)
//            {
//                myNeighboursInfo[i].rank = myRank - (myIndex - i);
//            }
//
//            if (doneMethod != null)
//                doneMethod("ok");
//        }
//    }
//
//
//
//
//
//
//    public void GetChampionsList(System.Action<string> doneMethod)
//    {
//        if (monthChampions != null && monthChampions.Length > 0)
//        {
//            if (doneMethod != null)
//                doneMethod("ok");
//        }
//        else
//            StartCoroutine(_GetChampionsList(doneMethod));
//    }
//
//    IEnumerator _GetChampionsList(System.Action<string> doneMethod)
//    {
//        string data = "cham;";
//        //data = Accounting.Base64Encode(data); //Encoding...
//
//        string url = string.Concat(Accounting.Instance.serverUrl, data);
//        WWW www = new WWW(url);
//
//        yield return www;
//
//
//        if (string.IsNullOrEmpty(www.error))
//        {
//            data = www.text.ToLower();
//            if (data.StartsWith("ok"))
//            {
//                data = data.Substring(2);
//
//                int colonIndex = data.IndexOf(",");
//                string weeklyChampions = data.Substring(0, colonIndex);
//                string monthlyChampions = data.Substring(colonIndex + 1);
//
//                monthChampions = monthlyChampions.Split(';');
//                weekChampions = weeklyChampions.Split(';');
//
//                if (doneMethod != null)
//                    doneMethod(www.text);
//            }
//            else
//            {
//                if (doneMethod != null)
//                    doneMethod("error:" + www.text);
//            }
//        }
//        else
//        {
//            if (doneMethod != null)
//                doneMethod("systemerror:" + www.error);
//        }
//    }
//}
//
//public enum LeaderboardType
//{
//    League,
//    Monthly,
//    Global
//}