using System;
using System.Collections.Generic;
using System.Text;

namespace HandyVS.com.Services
{
    public static class Constants
    {
        public static string QUERY_ALL_US = "{0}/DefaultCollection/7d7c091b-1f03-4010-84e5-336f0e6ea9f2/_apis/wit/wiql/d335369d-fa77-4e9b-806f-7b22d7890759";
        public static string QUERY_ALL_US_DETAILS = "{0}/DefaultCollection/_apis/wit/Workitems?ids={1}&api-version=1.0";
        public static string QUERY_ALL_BUGS = "https://hackathon2015.visualstudio.com/DefaultCollection/_apis/wit/Workitems?ids=6&api-version=1.0";
        public static string QUERY_ALL_WI = "";
        public static string QUERY_COMPLETED_US = "";
        public static string QUERY_RESOLVED_BUGS = "";
        public static string QUERY_COMPLETED_BUGS = "";

        // Colors
        public static string COLOR_FEATURE_DARK = "#773B93";
        public static string COLOR_USER_STORY_DARK = "#009CCC";
        public static string COLOR_TASK_DARK = "#F2CB1D";
        public static string COLOR_BUG_DARK = "#CC293D";

        public static string COLOR_FEATURE_LIGHT = "#F9F2FB";
        public static string COLOR_USER_STORY_LIGHT = "#F0F7F9";
        public static string COLOR_TASK_LIGHT = "#FDF8F0";
        public static string COLOR_BUG_LIGHT = "#F9EEF0";

        // Queries
        
        public static string API_GET_A_WI_DETAILS = "{0}/DefaultCollection/_apis/wit/workitems/{1}?api-version=1.0";
        public static string API_GET_WI_CATEGORIES = "{0}/defaultcollection/{1}/_apis/wit/workitemtypecategories?api-version=1.0";
        public static string API_GET_WI_QUERIES = "{0}/DefaultCollection/{1}/_apis/wit/queries/{2}?$depth=1&api-version=1.0";
        public static string API_GET_WI_DETAILS_FOR_IDS = "{0}/DefaultCollection/_apis/wit/workitems?ids={1}&api-version=1.0";

        public static string API_POST_WI_OPEN_ALL = "";
        public static string API_JSON_WI_OPEN_ALL = "{{\"query\": \"Select [System.WorkItemType],[System.Title],[System.State], [System.Id], [System.AssignedTo] From WorkItems Where [System.State] <> 'Closed' AND [System.State] <> 'Removed' AND [System.State] <> 'Resolved' AND [System.TeamProject] = '{0}' order by [Microsoft.VSTS.Common.Priority] asc, [System.CreatedDate] desc\"}}";

        public static string STR_PROJECT_REPO_TYPE_GIT = "git";
        public static string STR_PROJECT_REPO_TYPE_TFVC = "tfvc";
        public static string STR_PROJECT_TEMPLATE_TYPE;

        public static string GET_API_GIT_REPOS = "{0}/DefaultCollection/_apis/git/repositories?api-version=1.0";
        public static string GET_API_GIT_PULL_REQUESTS_ALL = "{0}/Defaultcollection/_apis/git/repositories/{1}/pullrequests?api-version=1.0";
        //public static string GET_API_GIT_COMMITS_ALL = "{0}/Defaultcollection/_apis/git/repositories/{1}/commits?api-version=1.0&fromdate={2}";
        public static string GET_API_GIT_COMMITS_ALL = "{0}/Defaultcollection/_apis/git/repositories/{1}/commits?api-version=1.0";

        public static string GET_API_TEAM_PROJECT_WITH_CAPABILITIES = "{0}/DefaultCollection/_apis/projects/{1}?includeCapabilities=true&api-version=1.0";


        public static string API_GET_BUILD_DEFINITIONS_ALL = "{0}/DefaultCollection/{1}/_apis/build/definitions?api-version=2.0";

        public static string API_GET_BUILDS_COMPLETED = "{0}/DefaultCollection/{1}/_apis/build/builds?api-version=2.0&%24top=25&statusFilter=2&type=2";
        public static string API_GET_BUILDS_QUEUED = "{0}/DefaultCollection/{1}/_apis/build/builds?api-version=2.0&%24top=100&statusFilter=41&type=2";

        public static string API_GET_BUILDS_COMPLETED_FOR_BD = "{0}/DefaultCollection/{1}/_apis/build/builds?api-version=2.0&%24top=25&statusFilter=2&type=2&definitions={2}";
        public static string API_GET_BUILDS_QUEUED_FOR_BD = "{0}/DefaultCollection/{1}/_apis/build/builds?api-version=2.0&%24top=100&statusFilter=41&type=2&definitions={2}";

        public static string API_GET_BUILDS_FOR_A_DEFINITION = "{0}/DefaultCollection/{1}/_apis/build/builds?api-version=1.0&definitionid={2}";
        public static string API_POST_BUILD_QUEUE_A_BUILD_URL = "{0}/DefaultCollection/{1}/_apis/build/builds?api-version=2.0";
        public static string API_POST_BODY_BUILD_QUEUE_A_BUILD_JSON_BODY = "{{\"definition\":{{\"id\": {0}}},\"sourceBranch\":\"{1}\"}}";

        //Release APIs
        public static string API_GET_RELEASE_DEFINITIONS_ALL = "{0}/DefaultCollection/{1}/_apis/release/definitions";
        public static string API_GET_A_RELEASE_DEFINITION = "{0}/DefaultCollection/{1}/_apis/release/definitions/{2}";
        public static string API_GET_RELEASES_ALL = "{0}/DefaultCollection/{1}/_apis/release/releases?$expand=environments&$top=25";
        public static string API_GET_A_RELEASE = "{0}/DefaultCollection/{1}/_apis/release/releases/{2}";
        public static string API_GET_RELEASES_FOR_RD = "{0}/DefaultCollection/{1}/_apis/release/releases?$expand=environments&definitionId={2}&$top=25";
        public static string API_GET_APPROVALS_ALL = "{0}/DefaultCollection/{1}/_apis/release/approvals?assignedToFilter=00000000-0000-0000-0000-000000000000";
        public static string API_GET_MY_APPROVALS = "{0}/DefaultCollection/{1}/_apis/release/approvals";
        public static string API_GET_APPROVALS_FOR_RELEASE = "{0}/DefaultCollection/{1}/_apis/release/approvals?releaseidsfilter={2}";
        public static string API_GET_APPROVALS_FOR_RD = "{0}/DefaultCollection/{1}/_apis/release/approvals?releaseidsfilter={2}";
        public static string API_PATCH_APPROVAL = "{0}/DefaultCollection/{1}/_apis/release/approvals/{2}?api-version=2.2-preview.1";
        public static string API_POST_ARTIFACT_VERSIONS_INTERNAL = "{0}/DefaultCollection/{1}/_apis/Release/artifacts/versions?api-version=2.2-preview.1";
        public static string API_POST_CREATE_A_RELEASE = "{0}/DefaultCollection/{1}/_apis/release/releases?api-version=2.2-preview.1";

        public static string API_POST_BODY_ARTIFACT_VERSIONS_INTERNAL = "[{0}]";
        public static string API_POST_BODY_CREATE_A_RELEASE = "{{ \"definitionId\":{0}, \"description\":\"{1}\", \"artifacts\":[ {2} ] }}";
        public static string API_POST_BODY_CREATE_RELEASE_ARTIFACT_BODY = "{{\"alias\":\"{0}\",\"instanceReference\":{{\"name\":\"{1}\",\"id\":\"{2}\",\"sourceBranch\":\"{3}\"}} }}";
        public static string API_POST_BODY_CREATE_A_RELEASE_SINGLE_ARTIFACT = "{{\"id\":{0},\"type\":\"{1}\",\"alias\":\"{2}\",\"definitionReference\":{{\"definition\":{{\"id\":\"{3}\",\"name\":\"{4}\"}},\"project\":{{\"id\":\"{5}\",\"name\":\"{6}\"}} }} }}";


        public static string API_POST_BODY_APPROVALS = "{{\"status\":\"{0}\",\"comments\":\"{1}\"}}";


        public static string WI_TYPE_BUG = "Bug";
        public static string WI_TYPE_USER_STORY = "User Story";
        public static string QUERY_WIT = "{0}/DefaultCollection/_apis/wit/wiql?api-version=1.0";
        public static string SAVE_WIT = "{0}/DefaultCollection/_apis/wit/workitems/{1}?api-version=1.0";
        public static string WIT_POST_DATA_BUG_WEEKLY = "{{\"query\": \"Select [System.Id], [System.Title], [System.State] From WorkItems Where [System.WorkItemType] = 'Bug' and  [System.CreatedDate] >= '{0}' and [System.TeamProject] = '{1}' order by [Microsoft.VSTS.Common.Priority] asc, [System.CreatedDate] desc\"}}";
        public static string WIT_POST_DATA_USERSTORY_DATEFILTER = "{{\"query\": \"Select [System.Id], [System.Title], [System.State] From WorkItems Where [System.WorkItemType] = 'User Story' and [System.CreatedDate] >= '{0}' and [System.TeamProject] = '{1}' order by [Microsoft.VSTS.Common.Priority] asc, [System.CreatedDate] desc\"}}";

        public static string WIT_ME_BUG_LAST30DAYS = "{{\"query\": \"SELECT [System.Id] FROM WorkItems WHERE [System.TeamProject] = '{0}'  AND  [System.CreatedDate] >= @Today - 30  AND  [System.WorkItemType] = 'Bug'  AND  [System.AssignedTo] = @Me ORDER BY [System.Id] DESC \"}}";
        public static string WIT_ME_FEATURES = "{{\"query\": \"SELECT [System.Id] FROM WorkItems WHERE [System.WorkItemType] IN GROUP 'Microsoft.FeatureCategory'  AND  [System.IterationPath] UNDER '{0}'  AND  [System.AreaPath] UNDER '{0}'  AND  [System.CreatedDate] >= @Today - 30  AND  [System.AssignedTo] = @Me ORDER BY [Microsoft.VSTS.Common.StackRank], [System.Id] DESC \"}}";
        public static string WIT_ME_SCENARIOS = "{{\"query\": \"SELECT [System.Id] FROM WorkItems WHERE [System.WorkItemType] IN GROUP 'Microsoft.ScenarioCategory'  AND  [System.IterationPath] UNDER '{0}'  AND  [System.AreaPath] UNDER '{0}'  AND  [System.AssignedTo] = @Me ORDER BY [Microsoft.VSTS.Common.StackRank], [System.Id] DESC \"}}";
        public static string WIT_ME_TASK_LAST30DAYS = "{{\"query\": \"SELECT [System.Id] FROM WorkItems WHERE [System.TeamProject] = '{0}'  AND  [System.CreatedDate] >= @Today - 30  AND  [System.WorkItemType] = 'Task'  AND  [System.AssignedTo] = @Me ORDER BY [System.Id] DESC \"}}";
        public static string WIT_ME_USERSTORIES_LAST30DAYS = "{{\"query\": \"SELECT [System.Id] FROM WorkItems WHERE [System.TeamProject] = '{0}'  AND  [System.CreatedDate] >= @Today - 30  AND  [System.WorkItemType] = 'User Story'  AND  [System.AssignedTo] = @Me ORDER BY [System.Id] DESC \"}}";
        public static string WIT_PROJECT_BUG_LAST30DAYS = "{{\"query\": \"SELECT [System.Id] FROM WorkItems WHERE [System.TeamProject] = '{0}'  AND  [System.CreatedDate] >= @Today - 30  AND  [System.WorkItemType] = 'Bug' ORDER BY [System.Id] DESC \"}}";
        public static string WIT_PROJECT_FEATURES_LAST30DAYS = "{{\"query\": \"SELECT [System.Id] FROM WorkItems WHERE [System.WorkItemType] IN GROUP 'Microsoft.FeatureCategory'  AND  [System.IterationPath] UNDER '{0}'  AND  [System.AreaPath] UNDER '{0}'  AND  [System.CreatedDate] >= @Today - 30 ORDER BY [Microsoft.VSTS.Common.StackRank], [System.Id] DESC \"}}";
        public static string WIT_PROJECT_SCENARIOS = "{{\"query\": \"SELECT [System.Id] FROM WorkItems WHERE [System.WorkItemType] IN GROUP 'Microsoft.ScenarioCategory'  AND  [System.IterationPath] UNDER '{0}'  AND  [System.AreaPath] UNDER '{0}' ORDER BY [Microsoft.VSTS.Common.StackRank], [System.Id] DESC \"}}";
        public static string WIT_PROJECT_TASK_LAST30DAYS = "{{\"query\": \"SELECT [System.Id] FROM WorkItems WHERE [System.TeamProject] = '{0}'  AND  [System.CreatedDate] >= @Today - 30  AND  [System.WorkItemType] = 'Task' ORDER BY [System.Id] DESC \"}}";
        public static string WIT_PROJECT_USERSTORIES_LAST30DAYS = "{{\"query\": \"SELECT [System.Id] FROM WorkItems WHERE [System.TeamProject] = '{0}'  AND  [System.CreatedDate] >= @Today - 30  AND  [System.WorkItemType] = 'User Story' ORDER BY [System.Id] DESC \"}}";

        public static string WIT_PROJECT_FEATURES_CLOSED_30DAYS = "{{\"query\": \"SELECT [System.Id] FROM WorkItems WHERE [System.WorkItemType] IN GROUP 'Microsoft.FeatureCategory' AND [System.TeamProject] = '{0}'  AND  ([System.State] = 'Closed'  OR  [System.State] = 'Completed'  OR  [System.State] = 'Done') AND ( [Microsoft.VSTS.Common.ResolvedDate] > @Today - 30  OR  [Microsoft.VSTS.Common.ClosedDate] > @Today - 30  OR  [Microsoft.VSTS.Scheduling.FinishDate] > @Today - 30 ) ORDER BY [System.Id] DESC \"}}";
        public static string WIT_PROJECT_USERSTORIES_CLOSED_30DAYS = "{{\"query\": \"SELECT [System.Id] FROM WorkItems WHERE [System.WorkItemType] IN GROUP 'Microsoft.RequirementCategory' AND [System.TeamProject] = '{0}'  AND  ([System.State] = 'Closed'  OR  [System.State] = 'Completed'  OR  [System.State] = 'Done') AND ( [Microsoft.VSTS.Common.ResolvedDate] > @Today - 30  OR  [Microsoft.VSTS.Common.ClosedDate] > @Today - 30  OR  [Microsoft.VSTS.Scheduling.FinishDate] > @Today - 30 ) ORDER BY [System.Id] DESC \"}}";
        public static string WIT_PROJECT_BUGS_CLOSED_30DAYS = "{{\"query\": \"SELECT [System.Id] FROM WorkItems WHERE [System.WorkItemType] IN GROUP 'Microsoft.BugCategory' AND [System.TeamProject] = '{0}'  AND  ([System.State] = 'Closed'  OR  [System.State] = 'Completed'  OR  [System.State] = 'Done') AND ( [Microsoft.VSTS.Common.ResolvedDate] > @Today - 30  OR  [Microsoft.VSTS.Common.ClosedDate] > @Today - 30  OR  [Microsoft.VSTS.Scheduling.FinishDate] > @Today - 30 ) ORDER BY [System.Id] DESC \"}}";
        public static string WIT_PROJECT_TASKS_CLOSED_30DAYS = "{{\"query\": \"SELECT [System.Id] FROM WorkItems WHERE [System.WorkItemType] IN GROUP 'Microsoft.TaskCategory' AND [System.TeamProject] = '{0}'  AND  ([System.State] = 'Closed'  OR  [System.State] = 'Completed'  OR  [System.State] = 'Done') AND ( [Microsoft.VSTS.Common.ResolvedDate] > @Today - 30  OR  [Microsoft.VSTS.Common.ClosedDate] > @Today - 30  OR  [Microsoft.VSTS.Scheduling.FinishDate] > @Today - 30 ) ORDER BY [System.Id] DESC \"}}";
        public static string WIT_MY_FEATURES_CLOSED_30DAYS = "{{\"query\": \"SELECT [System.Id] FROM WorkItems WHERE [System.WorkItemType] IN GROUP 'Microsoft.FeatureCategory' AND [System.TeamProject] = '{0}'  AND  [System.AssignedTo] = @Me  AND  ([System.State] = 'Closed'  OR  [System.State] = 'Completed'  OR  [System.State] = 'Done') AND ( [Microsoft.VSTS.Common.ResolvedDate] > @Today - 30  OR  [Microsoft.VSTS.Common.ClosedDate] > @Today - 30  OR  [Microsoft.VSTS.Scheduling.FinishDate] > @Today - 30 ) ORDER BY [System.Id] DESC \"}}";
        public static string WIT_MY_USERSTORIES_CLOSED_30DAYS = "{{\"query\": \"SELECT [System.Id] FROM WorkItems WHERE [System.WorkItemType] IN GROUP 'Microsoft.RequirementCategory' AND [System.TeamProject] = '{0}'  AND  [System.AssignedTo] = @Me  AND  ([System.State] = 'Closed'  OR  [System.State] = 'Completed'  OR  [System.State] = 'Done') AND ( [Microsoft.VSTS.Common.ResolvedDate] > @Today - 30  OR  [Microsoft.VSTS.Common.ClosedDate] > @Today - 30  OR  [Microsoft.VSTS.Scheduling.FinishDate] > @Today - 30 ) ORDER BY [System.Id] DESC \"}}";
        public static string WIT_MY_BUGS_CLOSED_30DAYS = "{{\"query\": \"SELECT [System.Id] FROM WorkItems WHERE [System.WorkItemType] IN GROUP 'Microsoft.BugCategory' AND [System.TeamProject] = '{0}'  AND  [System.AssignedTo] = @Me  AND  ([System.State] = 'Closed'  OR  [System.State] = 'Completed'  OR  [System.State] = 'Done') AND ( [Microsoft.VSTS.Common.ResolvedDate] > @Today - 30  OR  [Microsoft.VSTS.Common.ClosedDate] > @Today - 30  OR  [Microsoft.VSTS.Scheduling.FinishDate] > @Today - 30 ) ORDER BY [System.Id] DESC \"}}";
        public static string WIT_MY_TASKS_CLOSED_30DAYS = "{{\"query\": \"SELECT [System.Id] FROM WorkItems WHERE [System.WorkItemType] IN GROUP 'Microsoft.TaskCategory' AND [System.TeamProject] = '{0}'  AND  [System.AssignedTo] = @Me  AND  ([System.State] = 'Closed'  OR  [System.State] = 'Completed'  OR  [System.State] = 'Done') AND ( [Microsoft.VSTS.Common.ResolvedDate] > @Today - 30  OR  [Microsoft.VSTS.Common.ClosedDate] > @Today - 30  OR  [Microsoft.VSTS.Scheduling.FinishDate] > @Today - 30 ) ORDER BY [System.Id] DESC \"}}";

        public static string WIT_PROJECT_FEATURES_OPEN = "{{\"query\": \"SELECT [System.Id] FROM WorkItems WHERE [System.WorkItemType] IN GROUP 'Microsoft.FeatureCategory' AND [System.TeamProject] = '{0}'  AND  [System.State] <> 'Cancelled'  AND  [System.State] <> 'Closed'  AND  [System.State] <> 'Cut'  AND  [System.State] <> 'Completed'  AND  [System.State] <> 'Done' ORDER BY [System.Id] DESC \"}}";
        public static string WIT_PROJECT_USERSTORIES_OPEN = "{{\"query\": \"SELECT [System.Id] FROM WorkItems WHERE [System.WorkItemType] IN GROUP 'Microsoft.RequirementCategory' AND [System.TeamProject] = '{0}'  AND  [System.State] <> 'Cancelled'  AND  [System.State] <> 'Closed'  AND  [System.State] <> 'Cut'  AND  [System.State] <> 'Completed'  AND  [System.State] <> 'Done'  ORDER BY [System.Id] DESC \"}}";
        public static string WIT_PROJECT_TASKS_OPEN = "{{\"query\": \"SELECT [System.Id] FROM WorkItems WHERE [System.WorkItemType] IN GROUP 'Microsoft.TaskCategory' AND [System.TeamProject] = '{0}'  AND  [System.State] <> 'Cancelled'  AND  [System.State] <> 'Closed'  AND  [System.State] <> 'Cut'  AND  [System.State] <> 'Completed'  AND  [System.State] <> 'Done'  ORDER BY [System.Id] DESC \"}}";
        public static string WIT_PROJECT_BUGS_OPEN = "{{\"query\": \"SELECT [System.Id] FROM WorkItems WHERE [System.WorkItemType] IN GROUP 'Microsoft.BugCategory' AND [System.TeamProject] = '{0}'  AND  [System.State] <> 'Cancelled'  AND  [System.State] <> 'Closed'  AND  [System.State] <> 'Cut'  AND  [System.State] <> 'Completed'  AND  [System.State] <> 'Done'  ORDER BY [System.Id] DESC \"}}";

        public static string WIT_ME_FEATURES_OPEN = "{{\"query\": \"SELECT [System.Id] FROM WorkItems WHERE [System.WorkItemType] IN GROUP 'Microsoft.FeatureCategory' AND [System.TeamProject] = '{0}'  AND  [System.State] <> 'Cancelled'  AND  [System.State] <> 'Closed'  AND  [System.State] <> 'Cut'  AND  [System.State] <> 'Completed'  AND  [System.State] <> 'Done' AND  [System.AssignedTo] = @Me ORDER BY [System.Id] DESC \"}}";
        public static string WIT_ME_USERSTORIES_OPEN = "{{\"query\": \"SELECT [System.Id] FROM WorkItems WHERE [System.WorkItemType] IN GROUP 'Microsoft.RequirementCategory' AND [System.TeamProject] = '{0}'  AND  [System.State] <> 'Cancelled'  AND  [System.State] <> 'Closed'  AND  [System.State] <> 'Cut'  AND  [System.State] <> 'Completed'  AND  [System.State] <> 'Done'  AND  [System.AssignedTo] = @Me ORDER BY [System.Id] DESC \"}}";
        public static string WIT_ME_TASKS_OPEN = "{{\"query\": \"SELECT [System.Id] FROM WorkItems WHERE [System.WorkItemType] IN GROUP 'Microsoft.TaskCategory' AND [System.TeamProject] = '{0}'  AND  [System.State] <> 'Cancelled'  AND  [System.State] <> 'Closed'  AND  [System.State] <> 'Cut'  AND  [System.State] <> 'Completed'  AND  [System.State] <> 'Done'  AND  [System.AssignedTo] = @Me ORDER BY [System.Id] DESC \"}}";
        public static string WIT_ME_BUGS_OPEN = "{{\"query\": \"SELECT [System.Id] FROM WorkItems WHERE [System.WorkItemType] IN GROUP 'Microsoft.BugCategory' AND [System.TeamProject] = '{0}'  AND  [System.State] <> 'Cancelled'  AND  [System.State] <> 'Closed'  AND  [System.State] <> 'Cut'  AND  [System.State] <> 'Completed'  AND  [System.State] <> 'Done'  AND  [System.AssignedTo] = @Me ORDER BY [System.Id] DESC \"}}";

        public static string WIT_PROJECT_TEAM_ALL_OPEN = "{{\"query\": \"SELECT [System.Id], [System.Title], [System.State], [System.WorkItemType] FROM WorkItems WHERE [System.TeamProject] = '{0}'  AND  [System. \"}}";

        public static string GET_PROFILE_API_URL = "https://app.vssps.visualstudio.com/_apis/profile/profiles/me?api-version=1.0";

        public static string SAVE_WITSTATE = @"[{{
        ""op"": ""add"",
        ""path"": ""/fields/System.State"",
        ""value"": ""{0}""
      }}]";
        
        public static string ABB_MINE = "mine";
        public static string ABB_PROJECT = "project";

        public static string []WITType ={"User Story","Bug"};

        public static string VAL_NUM_DAYS_DATA_TO_FETCH = "5";

        public static string STG_PC_PROJECT = "PC_PROJECT";
        public static string STG_PC_TEAM = "PC_TEAM";
        public static string STG_PC_REPO_TYPE = "PC_REPO_TYPE";
        public static string STG_PC_REPO_ID = "PC_REPO_ID";
        public static string STG_PC_PROJECT_TEMPLATE = "PC_PROJ_TEMPLATE";

        public static string STG_STR_UNAME = "USERNAME";
        public static string STG_STR_PWD = "PASSWORD";
        public static string STG_STR_VSO_URL = "VSO_URL";
        public static string STG_BOOL_SHOW_MY_DATA = "SHOW_MY_DATA";
        public static string STG_STR_MY_EMAIL_ID = "MY_EMAIL_ID";
        public static string STG_STR_MY_NAME = "MY_NAME";
        public static string STG_BOOL_ENABLE_MY_DATA_TOGGLE = "ENABLE_MY_DATA_TOGGLE";
        public static string STG_BOOL_REMEMBER_PROJ_CONTEXT = "PROJ_CONTEXT";
        public static string STG_INT_NUM_DAYS_DATA_TO_FETCH = "NUM_DAYS_DATA_TO_FETCH";


        public static string MSG_BUILD_QUEUED_SUCCESSFUL = "Build queued successfully!";
        public static string MSG_SHOW_MY_DATA_NOT_ENABLED = "You need to enable this option from settings. Click on \"...\" from the bottom app bar and select \"Settings\".";
        public static string MSG_BUILD_QUEUE_FAIL_CHK_PERMS = "There was a problem while queuing a new build. Check if you have right permissions";
        public static int WitFeaturesPageIndex = 0;
        public static int WitUserStoriesPageIndex = 1;
        public static int WitBugsPageIndex = 2;
        public static int WitTasksPageIndex = 3;

        public static string STG_PC_REPO_NAME = "PC_REPO_NAME";
        public static string STR_QUERIES_MY_QUERIES = "My Queries";
        public static string STR_QUERIES_SHARED_QUERIES = "Shared Queries";

        public static string API_POST_RELEASE_BODY = "";
    }

    
}
