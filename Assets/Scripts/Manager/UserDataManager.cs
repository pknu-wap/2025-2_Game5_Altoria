using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserDataManager
{
    List<IUserData> userDataList = new();

    public void Init()
    {
        userDataList.Add(new UserPlayerData());
        userDataList.Add(new UserLifeData());

        for(int i = 0; i < userDataList.Count; i++)
        {
            userDataList[i].LoadData();
        }
    }

    public void SetAllDefaultUserData()
    {
        for (int i = 0; i < userDataList.Count; i++)
        {
            userDataList[i].SetDefaultData();
        }
    }

    public void LoadAllUserData()
    {
        for (int i = 0; i < userDataList.Count; i++)
        {
            userDataList[i].LoadData();
        }
    }

    public void SaveAllUserData()
    {
        for (int i = 0; i < userDataList.Count; i++)
            userDataList[i].SaveData();
    }

    public T GetUserData<T>() where T : class, IUserData
    {
        return userDataList.OfType<T>().FirstOrDefault();
    }

}
