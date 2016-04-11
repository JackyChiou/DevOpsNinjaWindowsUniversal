using BuildnReleaseWP.Context;
using BuildnReleaseWP.Model;
using HandyVS.com.Services;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace BuildnReleaseWP.Service
{
    static class VSTSService
    {
        public static Profile GetProfileDetails()
        {
            Profile profile = null;
            using (HttpResponseMessage response = LoginService.VSTSHttpClient.GetAsync(Constants.GET_PROFILE_API_URL).Result)
            {
                try
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    profile = getProfileDetailsFromJsonBody(responseBody);
                }
                catch
                {
                    //msg
                }
            }

            return profile;
        }

        public static string GetResponseFromPostRESTUri(string apiUrl, string content)
        {
            string responseBody = null;
            try
            {
                var requestContent = new System.Net.Http.StringContent(content, Encoding.UTF8, "application/json");
                var resultWit = LoginService.VSTSHttpClient.PostAsync(new Uri(apiUrl), requestContent).Result;
                resultWit.EnsureSuccessStatusCode();
                responseBody = resultWit.Content.ReadAsStringAsync().Result;
            }
            catch
            {
                //msg
            }
            return responseBody;
        }

        public static string GetResponseFromGetRESTUri(string apiUrl)
        {
            string responseBody = null;
            try
            {
                HttpResponseMessage response = LoginService.VSTSHttpClient.GetAsync(apiUrl).Result;
                response.EnsureSuccessStatusCode();

                responseBody = response.Content.ReadAsStringAsync().Result;
            }
            catch
            {
                //msg
            }
            return responseBody;
        }

        internal static Approval PatchApproval(Approval approval, string status, string comments)
        {
            Approval resultApproval = new Approval();
            var releaseServiceUrl = getReleaseServiceUrl();
            var query = string.Format(Constants.API_PATCH_APPROVAL, releaseServiceUrl, ProjectContext.GetProjectContext().Project, approval.Id);
            var jsonDataForPostCall = string.Format(Constants.API_POST_BODY_APPROVALS, status, comments);
            try
            {
                var requestContent = new StringContent(jsonDataForPostCall, Encoding.UTF8, "application/json");

                
                var result = LoginService.VSTSHttpClient.PatchAsync(new Uri(query),requestContent).Result;
                result.EnsureSuccessStatusCode();

                var responseWit = result.Content.ReadAsStringAsync().Result;

                dynamic json = JsonConvert.DeserializeObject(responseWit);

                resultApproval = getAnApprovalFromJson(json);
            }
            catch
            {
                resultApproval = null;
            }
            return resultApproval;
        }

        internal static Release CreateARelease(string definitionId, string description, List<ReleaseArtifactWithVersions> releaseArtifactsWithVersionsList)
        {
            Release release = null;
            string postBodyForArtifacts = getCreateReleasePostBodyForArtifacts(releaseArtifactsWithVersionsList);

            string postBodyStr = String.Format(Constants.API_POST_BODY_CREATE_A_RELEASE, definitionId, description, postBodyForArtifacts);

            string releaseUrl = getReleaseServiceUrl();

            string url = String.Format(Constants.API_POST_CREATE_A_RELEASE, releaseUrl, ProjectContext.GetProjectContext().Project);

            try
            {
                var postCallContent = new StringContent(postBodyStr, Encoding.UTF8, "application/json");
                var result = LoginService.VSTSHttpClient.PostAsync(new Uri(url), postCallContent).Result;
                result.EnsureSuccessStatusCode();
                var responseBody = result.Content.ReadAsStringAsync().Result;

                //dynamic json = JsonConvert.DeserializeObject(responseWit);

                release = getAReleaseFromResponseBody(responseBody);
            }
            catch
            {
                release = null;
            }
            return release;
        }

        private static string getCreateReleasePostBodyForArtifacts(List<ReleaseArtifactWithVersions> releaseArtifactsWithVersionsList)
        {
            string postBodyStr = "";

            foreach(ReleaseArtifactWithVersions rav in releaseArtifactsWithVersionsList)
            {
                //"{{\"alias\":\"{0}\",\"instanceReference\":{{\"name\":\"{1}\",\"id\":\"{2}\",\"sourceBranch\":\"{3}\"}} }}";

                string s = String.Format(Constants.API_POST_BODY_CREATE_RELEASE_ARTIFACT_BODY, rav.Artifact.Alias, rav.SelectedArtifactVersion.Name, rav.SelectedArtifactVersion.Id, rav.SelectedArtifactVersion.SourceBranch);
                postBodyStr = postBodyStr + s + ",";
            }

            postBodyStr = postBodyStr.Substring(0, postBodyStr.Length - 1);

            return postBodyStr;
        }

        internal static List<ReleaseArtifactWithVersions> GetReleaseArtifactsVersions(List<ReleaseDefinition.RDArtifact> artifacts, string postBodyForGettingArtifactVersions)
        {
            List<ReleaseArtifactWithVersions> artifactWithVersionsList = new List<ReleaseArtifactWithVersions>();
            string releaseUrl = getReleaseServiceUrl();

            string url = String.Format(Constants.API_POST_ARTIFACT_VERSIONS_INTERNAL, releaseUrl, ProjectContext.GetProjectContext().Project);
            
            try
            {
                var postCallContent = new StringContent(postBodyForGettingArtifactVersions, Encoding.UTF8, "application/json");
                var resultWit = LoginService.VSTSHttpClient.PostAsync(new Uri(url), postCallContent).Result;
                resultWit.EnsureSuccessStatusCode();
                var responseWit = resultWit.Content.ReadAsStringAsync().Result;

                dynamic json = JsonConvert.DeserializeObject(responseWit);   
                
                foreach(var av in json.artifactVersions)
                {
                    ReleaseArtifactWithVersions releaseArtifactWithVersions = new ReleaseArtifactWithVersions();
                    string artifactSourceId = av.artifactSourceId;

                    releaseArtifactWithVersions.Artifact = artifacts.FirstOrDefault(a => a.Id == artifactSourceId);
                    releaseArtifactWithVersions.ArtifactVersions = getReleaseArtifactVersionsFromJson(av.versions, artifactSourceId);

                    artifactWithVersionsList.Add(releaseArtifactWithVersions);
                }             
            }
            catch
            {
               
            }
            return artifactWithVersionsList;
        }

        private static List<ReleaseArtifactVersion> getReleaseArtifactVersionsFromJson(dynamic av, string id)
        {
            List<ReleaseArtifactVersion> releaseArtifactVersionList = new List<ReleaseArtifactVersion>();
            foreach(var a in av)
            {
                ReleaseArtifactVersion rav = new ReleaseArtifactVersion
                {
                    ArtifactSourceId = id,
                    Id = a.id,
                    Name = a.name,
                    SourceBranch = a.sourceBranch
                };

                releaseArtifactVersionList.Add(rav);
            }

            return releaseArtifactVersionList;
        }

        private static Profile getProfileDetailsFromJsonBody(string responseBody)
        {
            throw new NotImplementedException();
        }

        public static bool VSOLogin(string username, string passwd, string vsoUrl)
        {
            bool loginSuccess = false;

            if (String.IsNullOrEmpty(username) ||
                String.IsNullOrEmpty(passwd) ||
                String.IsNullOrEmpty(vsoUrl))
            {
                return false;
            }

            //LogInDetails.userName = username;
            //LogInDetails.password = passwd;
            //LoginContext.GetLoginContext().VsoAccountUrl = vsoUrl;

            //LoginService.VSTSHttpClient = LoginService.Connect(username, passwd);

            string tfsURL = string.Format("{0}/DefaultCollection", vsoUrl);

            using (HttpResponseMessage response = LoginService.VSTSHttpClient.GetAsync(
                                tfsURL).Result)
            {
                try
                {
                    response.EnsureSuccessStatusCode();
                    loginSuccess = true;
                }
                catch
                {
                    loginSuccess = false;
                }

            }
            return loginSuccess;
        }


        public static List<Project> GetVSTSProjects()
        {
            string tfsURL = string.Format("{0}/DefaultCollection/_apis/Projects?api-version=1.0&stateFilter=WellFormed", LoginContext.GetLoginContext().VSTSAccountUrl);

            using (HttpResponseMessage response = LoginService.VSTSHttpClient.GetAsync(tfsURL).Result)
            {
                try
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    var unsortedProjectsList = JsonConvert.DeserializeObject<ProjectsList>(responseBody).value;
                    return new List<Project>(unsortedProjectsList.OrderBy(s => s.Name,
                                  StringComparer.CurrentCultureIgnoreCase));
                }
                catch
                {
                    return new List<Project> { new Project { Name = "Unable to fetch Projects list." } };
                }
            }
        }
        
        internal static List<BuildDefinition> GetBuildDefinitions()
        {
            string buildAPIUrl = String.Format(Constants.API_GET_BUILD_DEFINITIONS_ALL, LoginContext.GetLoginContext().VSTSAccountUrl, ProjectContext.GetProjectContext().Project);
            HttpResponseMessage response = LoginService.VSTSHttpClient.GetAsync(buildAPIUrl).Result;
            response.EnsureSuccessStatusCode();

            string responseBody = response.Content.ReadAsStringAsync().Result;

            List<BuildDefinition> buildDefList = new List<BuildDefinition>();
            if (responseBody != null)
            {
                buildDefList = getBuildDefinitionListFromResponseBody(responseBody);
            }
            buildDefList.Sort((a, b) => a.Name.CompareTo(b.Name));

            return buildDefList;
        }

        internal static List<TimelineRecord> GetBuildTimeLineRecords(string v)
        {
            HttpResponseMessage response = LoginService.VSTSHttpClient.GetAsync(v).Result;
            response.EnsureSuccessStatusCode();

            string responseBody = response.Content.ReadAsStringAsync().Result;

            List<TimelineRecord> timelineRecords = new List<TimelineRecord>();

            if(responseBody != null)
            {
                timelineRecords = getBuildTimeLineRecordsFromResponseBody(responseBody);
            }
            timelineRecords.Sort((a, b) => a.Order.CompareTo(b.Order));

            return timelineRecords;
        }

        internal static List<Artifact> GetBuildArtifactss(string v)
        {
            HttpResponseMessage response = LoginService.VSTSHttpClient.GetAsync(v).Result;
            response.EnsureSuccessStatusCode();

            string responseBody = response.Content.ReadAsStringAsync().Result;

            List<Artifact> timelineRecords = new List<Artifact>();

            if (responseBody != null)
            {
                timelineRecords = getBuildArtifactssFromResponseBody(responseBody);
            }
            //timelineRecords.Sort((a, b) => a.Order.CompareTo(b.Order));

            return timelineRecords;
        }

        private static List<Artifact> getBuildArtifactssFromResponseBody(string responseBody)
        {
            List<Artifact> artifactsList = new List<Artifact>();

            dynamic json = JsonConvert.DeserializeObject(responseBody);

            if(json != null)
            {
                foreach(var a in json.value)
                {
                    Artifact artifact = new Artifact
                    {
                        Id = a.id,
                        Name = a.name
                    };

                    if(a.resource != null)
                    {
                        artifact.Url = a.resource.url;
                        artifact.DownloadUrl = a.resource.downloadUrl;
                    }

                    artifactsList.Add(artifact);
                }
            }

            return artifactsList;
        }

        private static List<TimelineRecord> getBuildTimeLineRecordsFromResponseBody(string responseBody)
        {
            List<TimelineRecord> timelineRecords = new List<TimelineRecord>();
            dynamic json = JsonConvert.DeserializeObject(responseBody);

            foreach(var tr in json.records)
            {
                TimelineRecord t = new TimelineRecord
                {
                    Name = tr.name,
                    Order = tr.order,
                    State = tr.state,
                    Result = tr.result,
                    WorkerName = tr.workerName,
                    Type = tr.type,
                    ParentId = tr.parentId
                };

                if (tr.startTime != null)
                {
                    t.StartTime = tr.startTime;
                }

                if (tr.finishTime != null)
                {
                    t.FinishTime = tr.finishTime;
                }

                if(t.StartTime != null && t.FinishTime != null)
                {
                    t.Duration = t.FinishTime.Subtract(t.StartTime).TotalMinutes;
                }

                if (tr.log != null)
                {
                    t.LogUrl = tr.log.url;
                }
                timelineRecords.Add(t);
            }
            return timelineRecords;
        }

        internal static List<QueuedBuild> GetQueuedBuilds()
        {
            string buildAPIUrl = String.Format(Constants.API_GET_BUILDS_QUEUED, LoginContext.GetLoginContext().VSTSAccountUrl, ProjectContext.GetProjectContext().Project);

            string responseBody = GetResponseFromGetRESTUri(buildAPIUrl);

            List<QueuedBuild> queuedBuildsList = new List<QueuedBuild>();

            if (responseBody != null)
            {
                queuedBuildsList = getQueuedBuildsFromJson(responseBody);
            }


            queuedBuildsList.Sort((a, b) => b.QueuedDate.CompareTo(a.QueuedDate));
            return queuedBuildsList;
        }

        internal static List<QueuedBuild> GetQueuedBuilds(BuildDefinition bd)
        {
            string buildAPIUrl = String.Format(Constants.API_GET_BUILDS_QUEUED_FOR_BD, LoginContext.GetLoginContext().VSTSAccountUrl, ProjectContext.GetProjectContext().Project, bd.Id);

            string responseBody = GetResponseFromGetRESTUri(buildAPIUrl);

            List<QueuedBuild> queuedBuildsList = new List<QueuedBuild>();

            if (responseBody != null)
            {
                queuedBuildsList = getQueuedBuildsFromJson(responseBody);
            }


            queuedBuildsList.Sort((a, b) => b.QueuedDate.CompareTo(a.QueuedDate));
            return queuedBuildsList;
        }

        private static List<QueuedBuild> getQueuedBuildsFromJson(string responseBody)
        {
            List<QueuedBuild> queuedBuildsList = new List<QueuedBuild>();

            dynamic json = JsonConvert.DeserializeObject(responseBody);

            foreach (var qb in json.value)
            {
                

                queuedBuildsList.Add(getQueuedBuildFromJson(qb));
            }
            return queuedBuildsList;
        }

        private static QueuedBuild getQueuedBuildFromJson(dynamic qb)
        {
            QueuedBuild build = new QueuedBuild
            {
                Id = qb.id,
                BuildNumber = qb.buildNumber,
                DefinitionName = qb.definition.name,
                Url = qb.url,
                QueuedDate = qb.queueTime,

                RequestedForName = qb.requestedFor.displayName,
                RequestedForUName = qb.requestedFor.uniqueName,
                Priotiy = qb.priority,
                Controller = qb.queue.name,
                Status = qb.status,
                SourceBranch = qb.sourceBranch

            };
            

            return build;
        }

        //internal static object CreateARelease(ReleaseDefinition releaseDefinition)
        //{
        //    bool success = true;
        //    var query = string.Format(Constants.API_POST_BUILD_QUEUE_A_BUILD_URL, LoginContext.GetLoginContext().VSTSAccountUrl, ProjectContext.GetProjectContext().Project);
        //    var jsonDataForPostCall = string.Format(Constants.API_POST_RELEASE_BODY);
        //    try
        //    {
        //        var responseContent = new System.Net.Http.StringContent(jsonDataForPostCall, Encoding.UTF8, "application/json");
        //        var resultWit = LoginService.VSTSHttpClient.PostAsync(new Uri(query), responseContent).Result;
        //        resultWit.EnsureSuccessStatusCode();
        //        var responseWit = resultWit.Content.ReadAsStringAsync().Result;

        //        dynamic json = JsonConvert.DeserializeObject(responseWit);

        //        newBuild = getQueuedBuildFromJson(json);
        //    }
        //    catch
        //    {
        //        success = false;
        //        newBuild = new QueuedBuild();
        //    }
        //    return success;
        //}

        internal static ReleaseDefinition GetAReleaseDefinition(string id)
        {
            string releaseServiceUrl = getReleaseServiceUrl();

            var url = string.Format(Constants.API_GET_A_RELEASE_DEFINITION, releaseServiceUrl, ProjectContext.GetProjectContext().Project, id);
            HttpResponseMessage response = LoginService.VSTSHttpClient.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();

            string responseBody = response.Content.ReadAsStringAsync().Result;

            var json = JsonConvert.DeserializeObject(responseBody);

            ReleaseDefinition rd = getAReleaseDefinitionFromJson(json, false);

            return rd;

        }

        internal static string GetBuildTimelineRecordLogs(string logUrl)
        {
            string logStr = "";
            try
            {
                string responseBody = GetResponseFromGetRESTUri(logUrl);

                dynamic json = JsonConvert.DeserializeObject(responseBody);
                int i = 0;

                //string[] logs = (string[])json.value;

                //logStr = logs.ToString();
                foreach (var line in json.value)
                {
                    logStr += (string)line;
                    logStr += "\n";
                    ++i;
                    if (i > 300)
                    {
                        logStr += "SKIPPING REST OF THE DATA ( > 300 lines)...\n";
                        break;
                    }
                }
            }
            catch
            {
                logStr = "Unable to get logs at this moment! The logs may be too huge to download. \nPlease try again later";
            }
            return logStr;
        }
        internal static string GetReleaseLogs(string logUrl)
        {
            string responseBody = "Unable to get logs at this moment! The logs may be too huge to download. \nPlease try again later";
            try
            {
                responseBody = GetResponseFromGetRESTUri(logUrl);

                //dynamic json = JsonConvert.DeserializeObject(responseBody);

                //string logStr = "";
                //foreach (var line in json.value)
                //{
                //    logStr += (string)line;
                //    logStr += "\n";
                //}
            }
            catch
            {
                //logStr = "Unable to get logs at this moment! The logs may be too huge to download. \nPlease try again later";
            }
            return responseBody;
        }

        internal static Build GetABuild(string url)
        {
            string responseBody = GetResponseFromGetRESTUri(url);

            dynamic json = JsonConvert.DeserializeObject(responseBody);
            Build b = getABuildFromJson(json);

            return b;
        }

        internal static List<Build> GetBuilds(BuildDefinition bd)
        {
            //int numDays = -1 * SettingsContext.GetSettingsContext().NumDaysToShowData;
            //buildAPIUrl = String.Format(Constants.API_BUILDS_QUEUED, LoginContext.GetLoginContext().VsoAccountUrl, ProjectContext.GetProjectContext().Project);
            string buildAPIUrl = String.Format(Constants.API_GET_BUILDS_COMPLETED_FOR_BD, LoginContext.GetLoginContext().VSTSAccountUrl, ProjectContext.GetProjectContext().Project, bd.Id);

            string responseBody = GetResponseFromGetRESTUri(buildAPIUrl);

            List<Build> buildsList = new List<Build>();
            if (responseBody != null)
            {
                buildsList = getBuildsFromJson(responseBody);
            }
            buildsList.Sort((a, b) => b.FinishTime.CompareTo(a.FinishTime));
            return buildsList;
        }

        internal static List<Build> GetBuilds()
        {

            //buildAPIUrl = String.Format(Constants.API_BUILDS_QUEUED, LoginContext.GetLoginContext().VsoAccountUrl, ProjectContext.GetProjectContext().Project);
            //int numDays = -1 * SettingsContext.GetSettingsContext().NumDaysToShowData;
            string buildAPIUrl = String.Format(Constants.API_GET_BUILDS_COMPLETED, LoginContext.GetLoginContext().VSTSAccountUrl, ProjectContext.GetProjectContext().Project);

            string responseBody = GetResponseFromGetRESTUri(buildAPIUrl);

            List<Build> buildsList = new List<Build>();
            if (responseBody != null)
            {
                buildsList = getBuildsFromJson(responseBody);
            }
            buildsList.Sort((a, b) => b.FinishTime.CompareTo(a.FinishTime));
            return buildsList;
        }

        private static List<Build> getBuildsFromJson(string responseBody)
        {
            List<Build> buildsList = new List<Build>();

            dynamic json = JsonConvert.DeserializeObject(responseBody);

            foreach (var bBdoy in json.value)
            {
                buildsList.Add(getABuildFromJson(bBdoy));
            }

            return buildsList;
        }

        private static Build getABuildFromJson(dynamic bb)
        {
            Build build = new Build
            {
                Id = bb.id,
                BuildNumber = bb.buildNumber,
                Status = bb.status,
                Result = bb.result,
                Url = bb.url,
                QueueTime = bb.queueTime,
                FinishTime = bb.finishTime,
                StartTime = bb.startTime,
                SourceBranch = bb.sourceBranch,
                SourceVersion = bb.sourceVersion,
                Priority = bb.priority,
                Reason = bb.reason
            };
            
            dynamic queueJson = bb.queue;

            if(queueJson != null)
            {
                build.Queue = new Queue
                {
                    Id = queueJson.id,
                    Name = queueJson.name
                };
            }

            dynamic identityJson = bb.requestedFor;
            if(identityJson != null)
            {
                build.RequestedFor = getIdentityFronJson(identityJson);
            }

            identityJson = bb.requestedBy;
            if (identityJson != null)
            {
                build.RequestedBy = getIdentityFronJson(identityJson);
            }

            dynamic defJson = bb.definition;
            if(defJson != null)
            {
                build.Definition = getABuildDefinitionFromJson(defJson);
            }

            build.LogsUrl = bb.logs == null ? "" : bb.logs.url;
            
            return build;
        }

        //Release API calls

        internal static List<ReleaseDefinition> GetReleaseDefinitions()
        {
            List<ReleaseDefinition> releaseDefList = new List<ReleaseDefinition>();
            try
            {
                string releaseServiceUrl = getReleaseServiceUrl();
                string releaseAPIUrl = String.Format(Constants.API_GET_RELEASE_DEFINITIONS_ALL, releaseServiceUrl, ProjectContext.GetProjectContext().Project);
                //HttpResponseMessage response = LoginService.VSTSHttpClient.GetAsync(releaseAPIUrl).Result;
                //response.EnsureSuccessStatusCode();

                string responseBody = GetResponseFromGetRESTUri(releaseAPIUrl);
                if (responseBody != null)
                {
                    releaseDefList = getReleaseDefinitionListFromJson(responseBody);
                }
                //releaseDefList.Sort((a, b) => a.Name.CompareTo(b.Name));
            }
            catch
            {
                releaseDefList = null;
            }
            return releaseDefList;
        }

        public static string getReleaseServiceUrl()
        {
            string accountUrl = LoginContext.GetLoginContext().VSTSAccountUrl.ToLower();
            string accountNamePart = accountUrl.Split('.')[0];
            string releaseServiceUrl = accountNamePart + ".vsrm.visualstudio.com";
            return releaseServiceUrl;
        }

        private static List<ReleaseDefinition> getReleaseDefinitionListFromJson(string responseBody, bool shallowReference = true)
        {
            List<ReleaseDefinition> rdList = new List<ReleaseDefinition>();

            dynamic json = JsonConvert.DeserializeObject(responseBody);

            foreach (var rdb in json.value)
            {
                ReleaseDefinition rd = getAReleaseDefinitionFromJson(rdb, shallowReference);

                rdList.Add(rd);
            }
            return rdList;
        }

        private static ReleaseDefinition getAReleaseDefinitionFromJson(dynamic rdb, bool shallowReference = true)
        {
            ReleaseDefinition rd = new ReleaseDefinition
            {
                Id = rdb.id,
                Name = rdb.name,
                Url = rdb.url,
                ReleaseNameFormat = rdb.releaseNameFormat
            };

            if(rdb.createdBy != null)
            {
                rd.CreatedBy = getIdentityFronJson(rdb.createdBy);
            }

            if(shallowReference == false)
            {
                if(rdb.artifacts != null)
                {
                    rd.Artifacts = getArtifactsFromRDJson(rdb.artifacts);
                }

                if(rdb.environments != null)
                {
                    rd.Environments = getEnvironmentsFromRDJson(rdb.environments);
                }
            }

            return rd;
        }

        private static List<ReleaseDefinition.RDEnvironment> getEnvironmentsFromRDJson(dynamic environments)
        {
            List<ReleaseDefinition.RDEnvironment> environmentsList = new List<ReleaseDefinition.RDEnvironment>();

            if(environments != null)
            {
                foreach(var e in environments)
                {
                    ReleaseDefinition.RDEnvironment re = new ReleaseDefinition.RDEnvironment();
                    re.Name = e.name;
                    environmentsList.Add(re);
                }
            }

            return environmentsList;
        }

        private static List<ReleaseDefinition.RDArtifact> getArtifactsFromRDJson(dynamic artifacts)
        {
            List<ReleaseDefinition.RDArtifact> artifactsList = new List<ReleaseDefinition.RDArtifact>();
            if(artifacts != null)
            {
                foreach(var a in artifacts)
                {
                    ReleaseDefinition.RDArtifact artifact = new ReleaseDefinition.RDArtifact
                    {
                        Id = a.id,
                        Alias = a.alias,
                        Type = a.type
                    };

                    var adr = a.definitionReference;

                    artifact.DefinitionReference = new ReleaseDefinition.RDArtifact.DefinitionRef();
                    if (adr != null)
                    {
                        if (adr.definition != null)
                        {
                            ItemDetails definition = new ItemDetails();

                            definition.Id = adr.definition.id;
                            definition.Name = adr.definition.name;

                            artifact.DefinitionReference.Definition = definition;
                        }

                        if(adr.project != null)
                        {
                            ItemDetails project = new ItemDetails();
                            project.Id = adr.project.id;
                            project.Name = adr.project.name;
                            artifact.DefinitionReference.Project = project;
                        }
                    }

                    artifactsList.Add(artifact);
                }
            }

            return artifactsList;
        }

        internal static List<Release> GetReleases(ReleaseDefinition rd)
        {
            //int numDays = -1 * SettingsContext.GetSettingsContext().NumDaysToShowData;


            string releaseServiceUrl = getReleaseServiceUrl();

            string releaseAPIUrl = String.Format(Constants.API_GET_RELEASES_FOR_RD, releaseServiceUrl, ProjectContext.GetProjectContext().Project, rd.Id);

            string responseBody = GetResponseFromGetRESTUri(releaseAPIUrl);
            return getReleasesListFromResponseBody(responseBody);
        }
        
        internal static List<Release> GetReleases()
        {

            //buildAPIUrl = String.Format(Constants.API_BUILDS_QUEUED, LoginContext.GetLoginContext().VsoAccountUrl, ProjectContext.GetProjectContext().Project);
            //int numDays = -1 * SettingsContext.GetSettingsContext().NumDaysToShowData;

            string releaseServiceUrl = getReleaseServiceUrl();

            string releaseAPIUrl = String.Format(Constants.API_GET_RELEASES_ALL, releaseServiceUrl, ProjectContext.GetProjectContext().Project);

            string responseBody = GetResponseFromGetRESTUri(releaseAPIUrl);

            return getReleasesListFromResponseBody(responseBody);
        }

        public static Release GetARelease(int id)
        {
            string releaseServiceUrl = getReleaseServiceUrl();

            string releaseAPIUrl = String.Format(Constants.API_GET_A_RELEASE, releaseServiceUrl, ProjectContext.GetProjectContext().Project, id);

            string responseBody = GetResponseFromGetRESTUri(releaseAPIUrl);

            return getAReleaseFromResponseBody(responseBody, false);
        }

        private static List<Release> getReleasesListFromResponseBody(string responseBody)
        {
            List<Release> releasesList = new List<Release>();
            if (responseBody != null)
            {
                releasesList = getReleasesListFromJson(responseBody);
            }
            releasesList.Sort((a, b) => b.ModifiedOn.CompareTo(a.ModifiedOn));
            return releasesList;
        }

        private static Release getAReleaseFromResponseBody(string responseBody, bool shallowreference = true)
        {
            Release release = null;
            if (responseBody != null)
            {
                dynamic json = JsonConvert.DeserializeObject(responseBody);
                release = getAReleaseFromJsonValue(json, shallowreference);
            }

            return release;
        }

        private static List<Release> getReleasesListFromJson(string responseBody)
        {
            List<Release> releasesList = new List<Release>();

            dynamic json = JsonConvert.DeserializeObject(responseBody);

            foreach (var rB in json.value)
            {
                Release r = getAReleaseFromJsonValue(rB);

                releasesList.Add(r);
            }

            return releasesList;
        }

        private static Release getAReleaseFromJsonValue(dynamic rB, bool shallowReference = true)
        {
            Release r = new Release
            {
                Id = rB.id,
                Name = rB.name,
                Url = rB.url,
                Status = rB.status,
                Reason = rB.reason,
                Description = rB.description
            };

            if (rB.modifiedOn != null)
                r.ModifiedOn = rB.modifiedOn;

            if (rB.createdOn != null)
                r.CreatedOn = rB.createdOn;

            if (rB.createdBy != null)
                r.CreatedBy = getIdentityFronJson(rB.createdBy);

            if (rB.modifiedBy != null)
                r.ModifiedBy = getIdentityFronJson(rB.modifiedBy);

            if (rB.releaseDefinition != null)
            {
                r.ReleaseDefinition = getAReleaseDefinitionFromJson(rB.releaseDefinition);
            }
                       
            dynamic eJson = rB.environments;
            if (eJson != null)
            {
                r.Environments = getAReleaseEnvironments(eJson, shallowReference);
            }

            if (!shallowReference)
            {
                dynamic aJson = rB.artifacts;

                if (aJson != null)
                {
                    r.Artifacts = getReleaseArtifacts(aJson);
                }
            }

            return r;
        }

        private static List<Release.REnvironment> getAReleaseEnvironments(dynamic eJson, bool shallowReference = true)
        {
            List<Release.REnvironment> reList = new List<Release.REnvironment>();
            foreach (var eB in eJson)
            {
                Release.REnvironment re = getAREnvironment(eB, shallowReference);
                reList.Add(re);
            }


            reList.Sort((a, b) => a.Rank.CompareTo(b.Rank));
            return reList;
        }

        private static Release.REnvironment getAREnvironment(dynamic eB, bool shallowReference = true)
        {
            Release.REnvironment e = new Release.REnvironment();
            e.Id = eB.id;
            e.Name = eB.name;
            e.Rank = eB.rank;
            e.Status = eB.status;


            if (shallowReference == false)
            {
                if(eB.createdOn != null)
                e.CreatedOn = eB.createdOn;
                // Get Pre-Deployment Approvals
                e.PreDeployApprovals = getAReleaseEnvironmentApprovals(eB.preDeployApprovals);
                e.PostDeployApprovals = getAReleaseEnvironmentApprovals(eB.postDeployApprovals);
                //Get Post-Deployment Approvals
                // Get deploy Step
                e.DeploySteps = getAREnvironmentDeployStep(eB);
            }

            return e;
        }

        private static List<Approval> getAReleaseEnvironmentApprovals(dynamic approvals)
        {
            List<Approval> approvalsList = new List<Approval>();
            if(approvals != null)
            {
                foreach(var approval in approvals)
                {
                    Approval a = getAnApprovalFromJson(approval);

                    approvalsList.Add(a);
                }
            }

            approvalsList.Sort((a, b) => a.Rank.CompareTo(b.Rank));
            return approvalsList;
        }

        private static Release.DeployStep getAREnvironmentDeployStep(dynamic eB)
        {
            Release.DeployStep ds = new Release.DeployStep();

            try
            {
                if (eB != null && eB.deploySteps != null)
                {
                    var deploySteps =  eB.deploySteps;

                    
                    foreach (var deployStep in deploySteps)
                    {
                        dynamic json = deployStep.tasks;
                        ds.Tasks = getARDeployStepTasksListFromJson(json);
                        ds.Job = getARDeployStepJobDetailsFromJson(deployStep.job);
                        break;
                    }
                    
                }
            }
            catch (RuntimeBinderException)
            {

            }

            return ds;
        }

        private static Release.DJob getARDeployStepJobDetailsFromJson(dynamic jobJson)
        {
            if(jobJson != null)
            {
                Release.DJob job = new Release.DJob
                {
                    Status = jobJson.status
                };

                return job;
            }

            return null;
        }

        private static List<Release.DTask> getARDeployStepTasksListFromJson(dynamic json)
        {
            List<Release.DTask> tasksList = new List<Release.DTask>();

            foreach (var t in json)
            {
                Release.DTask task = getARDeployStepSingleTaskFromJson(t);
                tasksList.Add(task);            
            }
            
            tasksList.Sort((a, b) => a.Rank.CompareTo(b.Rank));
            return tasksList;
        }

        private static Release.DTask getARDeployStepSingleTaskFromJson(dynamic t)
        {
            Release.DTask task = new Release.DTask
            {
                Id = t.id,
                Name = t.name,
                Rank = t.rank,
                AgentName = t.agentName,
                Status = t.status
            };

            if (t.dateStarted != null)
            {
                task.DateStarted = t.dateStarted;
            }
            if (t.dateEnded != null)
            {
                task.DateEnded = t.dateEnded;
            }
            if (!task.DateStarted.Equals(DateTime.MinValue) && !task.DateEnded.Equals(DateTime.MinValue))
            {
                task.Duration = task.DateEnded.Subtract(task.DateStarted).TotalMinutes;
            }
            else
            {
                task.Duration = -1;
            }

            return task;
        }

        private static List<Release.RArtifact> getReleaseArtifacts(dynamic aJson)
        {
            List<Release.RArtifact> aList = new List<Release.RArtifact>();
            foreach (var aB in aJson)
            {
                Release.RArtifact a = new Release.RArtifact();          
                a.Id = aB.id;
                a.Alias = aB.alias;
                a.Type = aB.type;
                if (aB.definitionReference != null)
                {
                    a.DefinitionReference = new Release.RArtifact.ArtifactDefinitionReference();
                    if(aB.definitionReference.definition != null)
                    {
                        a.DefinitionReference.DefintionName = aB.definitionReference.definition.name;
                    }

                    if (aB.definitionReference.version != null)
                    {
                        a.DefinitionReference.VersionName = aB.definitionReference.version.name;
                    }

                    if (aB.definitionReference.branch != null)
                    {
                        a.DefinitionReference.BranchName = aB.definitionReference.branch.name;
                    }
                }
                aList.Add(a);
            }

            return aList;
        }

        internal static List<Approval> GetApprovals()
        {
            string releaseServiceUrl = getReleaseServiceUrl();

            string releaseAPIUrl = String.Format(Constants.API_GET_APPROVALS_ALL, releaseServiceUrl, ProjectContext.GetProjectContext().Project);

            string responseBody = GetResponseFromGetRESTUri(releaseAPIUrl);

            return getApprovalsFromResponseBody(responseBody);
        }
        internal static List<Approval> GetMyApprovals()
        {
            string releaseServiceUrl = getReleaseServiceUrl();

            string releaseAPIUrl = String.Format(Constants.API_GET_MY_APPROVALS, releaseServiceUrl, ProjectContext.GetProjectContext().Project);

            string responseBody = GetResponseFromGetRESTUri(releaseAPIUrl);

            return getApprovalsFromResponseBody(responseBody);
        }

        internal static List<Approval> GetApprovals(Release release)
        {

            //buildAPIUrl = String.Format(Constants.API_BUILDS_QUEUED, LoginContext.GetLoginContext().VsoAccountUrl, ProjectContext.GetProjectContext().Project);
            //int numDays = -1 * SettingsContext.GetSettingsContext().NumDaysToShowData;

            string releaseServiceUrl = getReleaseServiceUrl();

            string releaseAPIUrl = String.Format(Constants.API_GET_APPROVALS_FOR_RELEASE, releaseServiceUrl, ProjectContext.GetProjectContext().Project, release.Id);

            string responseBody = GetResponseFromGetRESTUri(releaseAPIUrl);

            return getApprovalsFromResponseBody(responseBody);
        }

        internal static List<Approval> GetApprovals(List<Release> releasesList)
        {

            //buildAPIUrl = String.Format(Constants.API_BUILDS_QUEUED, LoginContext.GetLoginContext().VsoAccountUrl, ProjectContext.GetProjectContext().Project);
            //int numDays = -1 * SettingsContext.GetSettingsContext().NumDaysToShowData;

            string releaseServiceUrl = getReleaseServiceUrl();

            string releaseIdsFilter = "";
            foreach(Release r in releasesList)
            {
                releaseIdsFilter += (r.Id + ",");
            }
            releaseIdsFilter = releaseIdsFilter.Substring(0, releaseIdsFilter.Length - 1);
            string releaseAPIUrl = String.Format(Constants.API_GET_APPROVALS_FOR_RELEASE, releaseServiceUrl, ProjectContext.GetProjectContext().Project, releaseIdsFilter);

            string responseBody = GetResponseFromGetRESTUri(releaseAPIUrl);

            return getApprovalsFromResponseBody(responseBody);
        }

        internal static List<Approval> GetApprovals(ReleaseDefinition rd)
        {

            //buildAPIUrl = String.Format(Constants.API_BUILDS_QUEUED, LoginContext.GetLoginContext().VsoAccountUrl, ProjectContext.GetProjectContext().Project);
            //int numDays = -1 * SettingsContext.GetSettingsContext().NumDaysToShowData;

            string releaseServiceUrl = getReleaseServiceUrl();

            string releaseAPIUrl = String.Format(Constants.API_GET_APPROVALS_FOR_RD, releaseServiceUrl, ProjectContext.GetProjectContext().Project);

            string responseBody = GetResponseFromGetRESTUri(releaseAPIUrl);

            return getApprovalsFromResponseBody(responseBody);
        }

        private static List<Approval> getApprovalsFromResponseBody(string responseBody)
        {
            List<Approval> releasesList = new List<Approval>();
            if (responseBody != null)
            {
                releasesList = getApprovalsFromJson(responseBody);
            }
            releasesList.Sort((a, b) => b.ModifiedOn.CompareTo(a.ModifiedOn));
            return releasesList;
        }

        private static List<Approval> getApprovalsFromJson(string responseBody)
        {
            List<Approval> approvalsList = new List<Approval>();

            dynamic json = JsonConvert.DeserializeObject(responseBody);

            foreach (var approvalBody in json.value)
            {
                Approval approval = getAnApprovalFromJson(approvalBody);

                approvalsList.Add(approval);
            }

            return approvalsList;
        }

        private static Approval getAnApprovalFromJson(dynamic approvalBody)
        {
            Approval approval = new Approval
            {
                Id = approvalBody.id,
                ApprovalType = approvalBody.approvalType,
                Status = approvalBody.status,
                Comments = approvalBody.comments
            };

            if (approvalBody.approver != null)
            {
                approval.Approver = getIdentityFronJson(approvalBody.approver);
            }

            if (approvalBody.createdOn != null)
            {
                approval.CreatedOn = approvalBody.createdOn;
            }

            if (approvalBody.modifiedOn != null)
            {
                approval.ModifiedOn = approvalBody.modifiedOn;
            }


            if (approvalBody.releaseDefinition != null)
            {
                approval.ReleaseDefinition = new ReleaseDefinition
                {
                    Id = approvalBody.releaseDefinition.id,
                    Name = approvalBody.releaseDefinition.name
                };
            }

            if (approvalBody.release != null)
            {
                approval.Release = new Release
                {
                    Id = approvalBody.release.id,
                    Name = approvalBody.release.name
                };
            }

            if (approvalBody.releaseEnvironment != null)
            {
                approval.ReleaseEnvironment = new Release.REnvironment
                {
                    Id = approvalBody.releaseEnvironment.id,
                    Name = approvalBody.releaseEnvironment.name
                };
            }

            return approval;
        }


        //Build APIs
        private static List<BuildDefinition> getBuildDefinitionListFromResponseBody(string responseBody, bool shallowReference = true)
        {
            List<BuildDefinition> buildDefList = new List<BuildDefinition>();

            dynamic json = JsonConvert.DeserializeObject(responseBody);

            foreach (var bdBody in json.value)
            {
                BuildDefinition bd = getABuildDefinitionFromJson(bdBody, shallowReference);
                buildDefList.Add(bd);
            }

            return buildDefList;
        }

        private static BuildDefinition getABuildDefinitionFromJson(dynamic bdBody, bool shallowReference = true)
        {
            if (bdBody == null) return null;
            BuildDefinition bd = new BuildDefinition
            {
                Id = bdBody.id,
                Name = bdBody.name,
                Url = bdBody.url,
                Type = bdBody.type,
            };

            if (!shallowReference)
            {
                bd.BuildNumberFormat = bdBody.buildNumberFormat;

                if (bdBody.createdDate != null)
                {
                    bd.CreatedDate = bdBody.createdDate;
                }

                dynamic identityJson = bdBody.authoredBy;
                if (identityJson != null)
                {
                    bd.AuthoredBy = getIdentityFronJson(identityJson);
                }

                dynamic repoJson = bdBody.repository;
                if (repoJson != null)
                {
                    bd.Repository = getRepoFromJson(repoJson);
                }

                dynamic queue = bdBody.queue;

                if (queue != null)
                {
                    bd.Queue = new Queue
                    {
                        Id = queue.id,
                        Name = queue.name
                    };
                }
            }
            return bd;
        }

    

        private static Identity getIdentityFronJson(dynamic identityJson)
        {
            if (identityJson == null) return null;
            return new Identity
            {
                Id = identityJson.id,
                DisplayName = identityJson.displayName,
                UniqueName = identityJson.uniqueName,
                ImageUrl = identityJson.imageUrl,
                Url = identityJson.url
            };
        }

        public static BuildDefinition GetABuildDefinition(BuildDefinition bdRef)
        {
            if (bdRef == null || bdRef.Url == null || bdRef.Url.Equals(String.Empty))
            {
                return null;
            }

            string responseBody = GetResponseFromGetRESTUri(bdRef.Url);

            dynamic json = JsonConvert.DeserializeObject(responseBody);

            return getABuildDefinitionFromJson(json, false);
        }

       
        private static string GetLongFormattedDateStr(DateTime dt)
        {

            if (dt == null || dt.Equals(DateTime.MinValue)) return "";
            return dt.ToString("ddd dd MMM yyyy");
        }

        public static List<Repository> GetRepos()
        {
            string repoAPIUrl = String.Format(Constants.GET_API_GIT_REPOS, LoginContext.GetLoginContext().VSTSAccountUrl);

            string responseBody = GetResponseFromGetRESTUri(repoAPIUrl);

            List<Repository> repoList = new List<Repository>();
            if (responseBody != null && !responseBody.Equals(String.Empty))
            {
                dynamic json = JsonConvert.DeserializeObject(responseBody);

                foreach (var r in json.value)
                {
                    string projName = r.project.name;

                    if (projName.ToLower().Equals(ProjectContext.GetProjectContext().Project.ToLower()))
                    {
                        Repository repo = getRepoFromJson(r);

                        //Debug.WriteLine("name = " + r.name + "  proj = " + r.project.id + ": " + r.project.name);
                        repoList.Add(repo);
                    }
                }

                repoList.Sort((a, b) => b.Name.CompareTo(a.Name));
            }

            return repoList;
        }

        private static Repository getRepoFromJson(dynamic r)
        {
            return new Repository
            {
                Id = r.id,
                Name = r.name,
                Url = r.url,
                DefaultBranch = r.defaultBranch,
                Type = r.type
            };
        }

        internal static bool QueueBuild(string buildDefId, string branch, string description, out QueuedBuild newBuild)
        {
            bool success = true;
            var query = string.Format(Constants.API_POST_BUILD_QUEUE_A_BUILD_URL, LoginContext.GetLoginContext().VSTSAccountUrl, ProjectContext.GetProjectContext().Project);
            var jsonDataForPostCall = string.Format(Constants.API_POST_BODY_BUILD_QUEUE_A_BUILD_JSON_BODY, buildDefId, branch);
            try
            {
                var requestPostData = new StringContent(jsonDataForPostCall, Encoding.UTF8, "application/json");
                var responseBody = LoginService.VSTSHttpClient.PostAsync(new Uri(query), requestPostData).Result;
                responseBody.EnsureSuccessStatusCode();
                var responseWit = responseBody.Content.ReadAsStringAsync().Result;

                dynamic json = JsonConvert.DeserializeObject(responseWit);

                newBuild = getQueuedBuildFromJson(json);
            }
            catch
            {
                success = false;
                newBuild = new QueuedBuild();
            }
            return success;
        }
    }
}
