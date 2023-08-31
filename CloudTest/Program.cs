using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Client;
using PureCloudPlatform.Client.V2.Extensions;
using PureCloudPlatform.Client.V2.Model;


namespace CloudTest
{
    internal class Program
    {
        private static AuthTokenInfo accessTokenInfo;
        private static ArchitectApi architectApi;
        private static RoutingApi routingApi;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var environment = "mypurecloud.ie";
            Configuration.Default.ApiClient.setBasePath(PureCloudRegionHosts.eu_west_1);
            PureCloudRegionHosts? region = getRegion(environment);
            if (region == null)
            { //Returned in the case of default value
                PureCloudPlatform.Client.V2.Client.Configuration.Default.ApiClient.setBasePath("https://api." + environment);
            }
            else {
                PureCloudRegionHosts regionval = region.GetValueOrDefault();
                PureCloudPlatform.Client.V2.Client.Configuration.Default.ApiClient.setBasePath(regionval);
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
            var  sch = architectApi.GetArchitectSchedules();
        }

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
    }
}