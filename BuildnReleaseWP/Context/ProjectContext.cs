using BuildnReleaseWP.Service;
using HandyVS.com.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildnReleaseWP.Context
{
    class ProjectContext
    {
        private string str_project = "PC_PROJECT";
        private string str_team = "PC_TEAM";
        private string str_repositoryId = "PC_REPOSITORY_ID";
        private string str_repositoryType = "PC_REPOSITORY_TYPE";
        private string str_repositoryName = "PC_REPOSITORY_NAME";
        private string str_projectTemplate = "PC_PROJECT_TEMPLATE";
        private string str_featureTypeName = "PC_FEATURE_TYPE_NAME";
        private string str_taskTypeName = "PC_TASK_TYPE_NAME";
        private string str_requirementTypeName = "PC_REQUIREMENT_TYPE";
        private string str_bugTypeName = "PC_BUG_TYPE_NAME";

        private const string WI_FEATURE_CATEGORY = "Microsoft.FeatureCategory";
        private const string WI_REQUIREMENT_CATEGORY = "Microsoft.RequirementCategory";
        private const string WI_TASK_CATEGORY = "Microsoft.TaskCategory";
        private const string WI_BUG_CATEGORY = "Microsoft.BugCategory";
        private string _bugTypeName;

        public string BugTypeName
        {
            get { return _bugTypeName; }
            set
            {
                _bugTypeName = value;
                localSettings.Values[str_bugTypeName] = _bugTypeName;
            }
        }


        private string _taskTypeName;

        public string TaskTypeName
        {
            get { return _taskTypeName; }
            set
            {
                _taskTypeName = value;
                localSettings.Values[str_taskTypeName] = _taskTypeName;
            }
        }


        private string _requirementTypeName;

        public string RequirementTypeName
        {
            get { return _requirementTypeName; }
            set
            {
                _requirementTypeName = value;
                localSettings.Values[str_requirementTypeName] = _requirementTypeName;
            }
        }

        private string _featureTypeName;

        public string FeatureTypeName
        {
            get { return _featureTypeName; }
            set
            {
                _featureTypeName = value;
                localSettings.Values[str_featureTypeName] = _featureTypeName;
            }
        }

        private string _projectTemplate;

        public string ProjectTemplate
        {
            get { return _projectTemplate; }
            set
            {
                _projectTemplate = value;
                localSettings.Values[str_projectTemplate] = _projectTemplate;
            }
        }


        private string _repositoryType;

        public string RepositoryType
        {
            get { return _repositoryType; }
            set
            {
                _repositoryType = value;
                localSettings.Values[str_repositoryType] = _repositoryType;
            }
        }

        private string _repositoryName;

        public string RepositoryName
        {
            get { return _repositoryName; }
            set
            {
                _repositoryName = value;
                localSettings.Values[str_repositoryName] = _repositoryName;
            }
        }


        private string _repositoryId;

        public string RepositoryId
        {
            get { return _repositoryId; }
            set
            {
                _repositoryId = value;
                localSettings.Values[str_repositoryId] = value;
            }
        }

        private string _project;
        public string Project
        {
            get { return _project; }
            set
            {
                getAndSetProjectContext(value);
                //localSettings.Values[str_project] = _project;
            }
        }

        private void getAndSetProjectContext(string value)
        {
            if (!value.Equals(_project))
            {
                Reset();
                _project = value;
                //anangaur: added recently
                localSettings.Values[str_project] = _project;
                //GetAndSetProjectDetails();
            }
            //GetAndSetWorkItemCategories();

        }

        LoginContext lc = LoginContext.GetLoginContext();
        public void GetAndSetWorkItemCategories()
        {
            string api = String.Format(Constants.API_GET_WI_CATEGORIES, lc.VSTSAccountUrl, Project);

            string responseBody = VSTSService.GetResponseFromGetRESTUri(api);

            if (responseBody != null)
            {
                try
                {
                    dynamic json = JsonConvert.DeserializeObject(responseBody);


                    foreach (var v in json.value)
                    {
                        string s = v.referenceName;
                        if (s != null && !s.Equals(String.Empty))
                        {
                            switch (s)
                            {
                                case WI_FEATURE_CATEGORY:
                                    FeatureTypeName = v.defaultWorkItemType.name;
                                    break;
                                case WI_REQUIREMENT_CATEGORY:
                                    RequirementTypeName = v.defaultWorkItemType.name;
                                    break;
                                case WI_TASK_CATEGORY:
                                    TaskTypeName = v.defaultWorkItemType.name;
                                    break;
                                case WI_BUG_CATEGORY:
                                    BugTypeName = v.defaultWorkItemType.name;
                                    break;
                            }
                        }
                    }
                }
                catch
                {
                    //
                }
            }
        }

        public void GetAndSetProjectDetails()
        {

            LoginContext lc = LoginContext.GetLoginContext();
            string apiUrl = String.Format(Constants.GET_API_TEAM_PROJECT_WITH_CAPABILITIES, lc.VSTSAccountUrl, Project);

            string responseBody = VSTSService.GetResponseFromGetRESTUri(apiUrl);

            if (responseBody != null)
            {
                try
                {
                    dynamic json = JsonConvert.DeserializeObject(responseBody);
                    RepositoryType = json.capabilities.versioncontrol.sourceControlType;
                    ProjectTemplate = json.capabilities.processTemplate.templateName;

                    localSettings.Values[str_project] = _project;
                    localSettings.Values[str_repositoryType] = _repositoryType;
                    localSettings.Values[str_projectTemplate] = _projectTemplate;
                }
                catch { }
            }

        }

        private string _team;

        public string Team
        {
            get { return _team; }
            set
            {
                _team = value;
                localSettings.Values[str_team] = _team;
            }
        }


        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        private static ProjectContext projectContext = new ProjectContext();

        public static ProjectContext GetProjectContext()
        {
            return projectContext;
        }
        public void ResetProject()
        {
            localSettings.Values[str_project] = String.Empty;
        }

        public bool RestoreSavedSettings()
        {
            _project = restoreSavedStrSettingsFor(str_project);

            if(!String.IsNullOrWhiteSpace(_project))
            {
                return true;
            }

            return false;
            //_team = restoreSavedStrSettingsFor(str_team);
            //_projectTemplate = restoreSavedStrSettingsFor(str_projectTemplate);
            //_repositoryId = restoreSavedStrSettingsFor(str_repositoryId);
            //_repositoryType = restoreSavedStrSettingsFor(str_repositoryType);
            //_repositoryName = restoreSavedStrSettingsFor(str_repositoryName);
            //_featureTypeName = restoreSavedStrSettingsFor(str_featureTypeName);
            //_requirementTypeName = restoreSavedStrSettingsFor(str_requirementTypeName);
            //_taskTypeName = restoreSavedStrSettingsFor(str_taskTypeName);
            //_bugTypeName = restoreSavedStrSettingsFor(str_bugTypeName);
        }

        private string restoreSavedStrSettingsFor(string setting)
        {
            if (localSettings.Values.ContainsKey(setting))
            {
                return localSettings.Values[setting] as string;
            }
            return String.Empty;
        }

        public void Reset()
        {
            _project = String.Empty;
            _team = String.Empty;
            _repositoryId = String.Empty;
            _repositoryType = String.Empty;
            _repositoryName = String.Empty;
            _projectTemplate = String.Empty;
            _featureTypeName = String.Empty;
            _taskTypeName = String.Empty;
            _requirementTypeName = String.Empty;
            _bugTypeName = String.Empty;
        }

    }
}
