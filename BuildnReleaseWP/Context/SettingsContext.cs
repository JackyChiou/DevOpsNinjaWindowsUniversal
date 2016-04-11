using System;
using System.Collections.Generic;
using System.Text;

namespace BuildnReleaseWP.Context
{
    //class SettingsContext
    //{
    //    private string str_isProjectContextEnabled = "SC_IPCE";
    //    private string str_isShowDataToggleEnabled = "SC_ISDTE";
    //    private string str_showMyData = "SC_ShOW_MY_DATA";
    //    private string str_myName = "SC_MY_NAME";
    //    private string str_myEmailId = "SC_MY_EMAIL_ID";
    //    private string str_numDaysOfDataToShow = "SC_NDODTS";

    //    Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

    //    private static SettingsContext settingsContext = new SettingsContext();

    //    public void RestoreSavedSettings()
    //    {
    //        _myName = restoreSavedStrSettingsFor(str_myName);
    //        _myEmailId = restoreSavedStrSettingsFor(str_myEmailId);

    //        string restoredVal = restoreSavedStrSettingsFor(str_isProjectContextEnabled);
    //        _isProjectContextEnabled = restoredVal.Equals(String.Empty) ? true : bool.Parse(restoredVal);

    //        restoredVal = restoreSavedStrSettingsFor(str_isShowDataToggleEnabled);
    //        _isShowDataToggleEnabled = restoredVal.Equals(String.Empty) ? false : bool.Parse(restoredVal);

    //        restoredVal = restoreSavedStrSettingsFor(str_showMyData);
    //        _showMyData = restoredVal.Equals(String.Empty) ? true : bool.Parse(restoredVal);

    //        restoredVal = restoreSavedStrSettingsFor(str_numDaysOfDataToShow);
    //        _numDaysOfDataToShow = restoredVal.Equals(String.Empty) ? 0 : int.Parse(restoredVal);
    //    }

    //    private string restoreSavedStrSettingsFor(string setting)
    //    {
    //        if (localSettings.Values.ContainsKey(setting))
    //        {
    //            return localSettings.Values[setting] as string;
    //        }
    //        return String.Empty;
    //    }

    //    public static SettingsContext GetSettingsContext()
    //    {
    //        return settingsContext;
    //    }

    //    private bool _isProjectContextEnabled;

    //    public bool IsProjectContextEnabled
    //    {
    //        get { return _isProjectContextEnabled; }
    //        set
    //        {
    //            _isProjectContextEnabled = value;
    //            localSettings.Values[str_isProjectContextEnabled] = _isProjectContextEnabled.ToString();

    //        }
    //    }

    //    private bool _isShowDataToggleEnabled;

    //    public bool IsShowDataToggleEnabled
    //    {
    //        get { return _isShowDataToggleEnabled; }
    //        set
    //        {
    //            _isShowDataToggleEnabled = value;
    //            localSettings.Values[str_isShowDataToggleEnabled] = _isShowDataToggleEnabled.ToString();

    //        }
    //    }
    //    private bool _showMyData;

    //    public bool ShowMyData
    //    {
    //        get { return _showMyData; }
    //        set
    //        {
    //            _showMyData = value;
    //            localSettings.Values[str_showMyData] = _showMyData.ToString();
    //        }
    //    }
    //    private string _myName;

    //    public string MyName
    //    {
    //        get { return _myName; }
    //        set
    //        {
    //            _myName = value;
    //            localSettings.Values[str_myName] = _myName;

    //        }
    //    }

    //    private string _myEmailId;

    //    public string MyEmailId
    //    {
    //        get { return _myEmailId; }
    //        set
    //        {
    //            _myEmailId = value;
    //            localSettings.Values[str_myEmailId] = _myEmailId;
    //        }
    //    }

    //    private int _numDaysOfDataToShow;

    //    public int NumDaysToShowData
    //    {
    //        get { return _numDaysOfDataToShow; }
    //        set
    //        {
    //            _numDaysOfDataToShow = value;
    //            localSettings.Values[str_numDaysOfDataToShow] = _numDaysOfDataToShow.ToString();
    //        }
    //    }


    //    public void Reset()
    //    {
    //        _isProjectContextEnabled = true;
    //        _isShowDataToggleEnabled = false;
    //        _showMyData = false;
    //        _myName = String.Empty;
    //        _myEmailId = String.Empty;
    //        _numDaysOfDataToShow = 0;
    //    }


    //}
}
