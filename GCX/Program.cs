// See https://aka.ms/new-console-template for more information


using PureCloudPlatform.Client.V2.Model;

Console.WriteLine("Hello, World!");

PureCloudPlatform.Client.V2.Api.ArchitectApi api = new PureCloudPlatform.Client.V2.Api.ArchitectApi();
Flow f = new Flow();
ScheduleGroup sc = new ScheduleGroup();
IVR i = new IVR();
i.