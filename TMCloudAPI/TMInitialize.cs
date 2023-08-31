
using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Client;
using PureCloudPlatform.Client.V2.Extensions;
using PureCloudPlatform.Client.V2.Model;
using System;
using System.Linq;

namespace TMCloudAPI
{
    public static class TMInitialize
    {
        static AuthTokenInfo accessTokenInfo;
        static ArchitectApi architectApi;

        public static RoutingApi routingApi { get; private set; }
        public static ScheduleGroupEntityListing ScheduleGroups { get; private set; }
        public static ScheduleEntityListing Schedules { get; private set; }

        private static List<Schedule> ScheduleEntities;
        private static List<string> queues;
        private static List<string> schedules;
        private static List<Queue> QueueEntityListing;

        public static Nullable<PureCloudRegionHosts> getRegion(String str = "http://api.mypurecloud.com")
        {
            switch (str)
            {
                case "mypurecloud.com":
                    return PureCloudRegionHosts.us_east_1;
                case "mypurecloud.ie":
                    return PureCloudRegionHosts.eu_west_1;
                case "mypurecloud.de":
                    return PureCloudRegionHosts.eu_central_1;
                case "mypurecloud.jp":
                    return PureCloudRegionHosts.ap_northeast_1;
                case "mypurecloud.com.au":
                    return PureCloudRegionHosts.ap_southeast_1;
                case "usw2.pure.cloud":
                    return PureCloudRegionHosts.us_west_2;
                case "cac1.pure.cloud":
                    return PureCloudRegionHosts.ca_central_1;
                case "apne2.pure.cloud":
                    return PureCloudRegionHosts.ap_northeast_2;
                case "euw2.pure.cloud":
                    return PureCloudRegionHosts.eu_west_2;
                case "aps1.pure.cloud":
                    return PureCloudRegionHosts.ap_south_1;
                case "use2.us-gov-pure.cloud":
                    return PureCloudRegionHosts.us_east_2;
                default:
                    Console.WriteLine("Value does not exist in enum using default val");
                    return null;
            }
        }
        static TMInitialize()
        {
            Intialize();
        }
        public static void Intialize()
        {

            var environment = "mypurecloud.ie";
            Configuration.Default.ApiClient.setBasePath(PureCloudRegionHosts.eu_west_1);
            PureCloudRegionHosts? region = getRegion(environment);
            if (region == null)
            { //Returned in the case of default value
                PureCloudPlatform.Client.V2.Client.Configuration.Default.ApiClient.setBasePath("https://api." + environment);
            }
            accessTokenInfo = Configuration.Default.ApiClient.PostToken("3f51f7bc-52bc-426b-b49f-5efd9f463f23", "fifd81W_dm1KBqTRjVjCUEX0RqBb9Bgm3sMdDl59HnM");

            var retryConfig = new ApiClient.RetryConfiguration
            {
                MaxRetryTimeSec = 10,
                RetryMax = 5
            };

            Configuration.Default.ApiClient.RetryConfig = retryConfig;

            architectApi = new ArchitectApi();
            routingApi = new RoutingApi();

            var Queues = routingApi.GetRoutingQueues();
            queues = new List<string>();
            queues.AddRange(Queues.Entities.Select(x => x.Name));

            QueueEntityListing = Queues.Entities.ToList();


            while (Queues.NextUri != null)
            {
                Queues = routingApi.GetRoutingQueues(pageNumber: Queues.PageNumber + 1);
                QueueEntityListing.AddRange(Queues.Entities);
                queues.AddRange(Queues.Entities.Select(x => x.Name));
            }

            Schedules = architectApi.GetArchitectSchedules();
            ScheduleEntities  = Schedules.Entities.ToList();

            schedules = Schedules.Entities.Select(s => s.Name).ToList();

            while (Schedules.NextUri != null)
            {
                Schedules = architectApi.GetArchitectSchedules(pageNumber: Schedules.PageNumber + 1);
                schedules.AddRange(Schedules.Entities.Select(x => x.Name));
                ScheduleEntities.AddRange(Schedules.Entities);
            }
        }

        public static void GetSchedulesGroups()
        {
            ScheduleGroups = architectApi.GetArchitectSchedulegroups();
        }

        public static void GetSchedules()
        {
            Schedules = architectApi.GetArchitectSchedules();

        }
        public static List<string> GetQueues(string Queue)
        {


            var tempQueue = queues.Intersect(schedules).ToList();
            var queueItem = QueueEntityListing.FirstOrDefault(f=> f.Id.Equals( Queue, StringComparison.InvariantCultureIgnoreCase));

            if(queueItem != null)  tempQueue.Remove(queueItem.Name);

            var tempSchedules = ScheduleEntities.Where(w => tempQueue.ToArray().Any(a => a == w.Name)).ToList();

            tempSchedules = tempSchedules.Where(w => w.Rrule.Contains(DateTime.Now.DayOfWeek.ToString().Substring(0, 2), StringComparison.InvariantCultureIgnoreCase)).ToList();

            return tempQueue;
        }
    }
}
