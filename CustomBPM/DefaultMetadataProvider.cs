using System.Collections.Generic;

namespace CustomBPM
{
    public class DefaultMetadataProvider : IMetadataProvider
    {
        private readonly Dictionary<string, Process> _processes = new Dictionary<string, Process>()
        {
            {"test", CreateTest()},
            {"sale1", CreateSaleProcess()}
        };

        public Process GetProcess(string code)
        {
            return _processes[code];
        }

        private static Process CreateSaleProcess()
        {
            return new Process()
            {
                Code = "sale1",
                Name = "Продажа",
                Activities =
                {
                    new Activity()
                    {
                        Code = "request",
                        Name = "Обращение",
                        IsStart = true,
                        AllowRoles = new[] {"sale_manager"},
                        AllowOutputActivities =
                        {
                            new ActivityLink("inJob", true),
                            new ActivityLink("fail", false) {ConditionCode = "ToArchiveFromRequestCondition"}
                        }
                    },
                    new Activity()
                    {
                        Code = "inJob",
                        Name = "В работе",
                        AllowRoles = new[] {"sale_manager"},
                        Conditions =
                        {
                            new Condition { Key = "ToInJobCondition", Type = ConditionType.Input}
                        },

                        AllowInputActivities =
                        {
                            new ActivityLink("request", true),
                            new ActivityLink("reserve", false)
                        },
                        AllowOutputActivities =
                        {
                            new ActivityLink("reserve", true),
                            new ActivityLink("fail", false) {ConditionCode = "ToArchiveFromInjobCondition"}
                        }
                    },
                    new Activity()
                    {
                        Code = "reserve",
                        Name = "В резерве",
                        AllowRoles = new[] {"sale_manager"},
                        AllowInputActivities =
                        {
                            new ActivityLink("inJob", true)
                        },
                        AllowOutputActivities =
                        {
                            new ActivityLink("toIssue", true),
                            new ActivityLink("inJob", false),
                            new ActivityLink("fail", false) {ConditionCode = "ToArchiveFromReserveCondition"}
                        },
                        Conditions =
                        {
                            new Condition { Key = "ToReserveWithCarCheckCondition", Type = ConditionType.Input},
                            new Condition { Key = "ToReserveWithClientCheckCondition", Type = ConditionType.Input}
                        }
                    }
                    ,
                    new Activity()
                    {
                        Code = "toIssue",
                        Name = "На выдачу",
                        IsEnd = true,
                        AllowRoles = new[] {"sale_manager"},
                        AllowInputActivities =
                        {
                            new ActivityLink("reserve", true)
                        },
                        AllowOutputActivities =
                        {
                            new ActivityLink("success", false)
                        }
                    },
                    new Activity()
                    {
                        Code = "success",
                        Name = "Успех",
                        IsProcessEnd = true,
                        AllowInputActivities =
                        {
                            new ActivityLink("toIssue", false)
                        },
                        //InputAction = "successAction",
                        //InputCondition = "successCondition"
                    },
                    new Activity()
                    {
                        Code = "fail",
                        Name = "Провал",
                        IsProcessEnd = true,
                        AllowInputActivities =
                        {
                            new ActivityLink("request", false),
                            new ActivityLink("inJob", false),
                            new ActivityLink("reserve", false)
                        },
                        //InputAction = "failAction",
                        //InputCondition = "failCondition"
                    }
                }
            };
        }

        private static Process CreateTest()
        {
            return new Process()
            {
                Code = "test",
                Name = "Test process",
                Activities =
                {
                    new Activity()
                    {
                        Code = "test-activity1",
                        Name = "Activity 1"
                        ,
                        AllowOutputActivities = {new ActivityLink("test-activity2", true)}
                    },
                    new Activity()
                    {
                        Code = "test-activity2",
                        Name = "Activity 2",
                        AllowOutputActivities = {new ActivityLink("test-activity3", true)},
                        AllowInputActivities = {new ActivityLink("test-activity1", true)}
                    },
                    new Activity()
                    {
                        Code = "test-activity3",
                        Name = "Activity 3",
                        AllowInputActivities = {new ActivityLink("test-activity2", true)},
                        //InputCondition = "testCondition"
                    }
                },
            };
        }
    }
}