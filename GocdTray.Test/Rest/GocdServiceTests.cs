using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using GocdTray.App;
using GocdTray.App.Abstractions;
using GocdTray.Rest;
using GocdTray.Rest.Dto;
using NUnit.Framework;

namespace GocdTray.Test.Rest
{
    [TestFixture]
    public class GocdServiceTests
    {

        [Test, Ignore("")]
        public void GetPipelines_RealCall()
        {
            var restClient = new RestClient("https://buildserver:8154", "username", "password", true);
            var service = new GocdService(restClient);
            var result = service.GetPipelines();
            
            Assert.That(result.IsValid, result.ToString());
            Console.WriteLine(result.Data);
        }

        [Test]
        public void GetPipelines_DeserialisesPipelineAndPipelineGroupName_Correctly()
        {
            // Arrange
            #region jsonResult

            string jsonResult = @"{
  ""_embedded"": {
    ""pipeline_groups"": [
      {
        ""name"": ""Build"",
        ""_embedded"": {
          ""pipelines"": [
            {
              ""name"": ""DirectaTrunk"",
              ""locked"": false,
              ""pause_info"": {
                ""paused"": false,
                ""paused_by"": null,
                ""pause_reason"": null
              },
              ""_embedded"": {
                ""instances"": [
                  {
                    ""label"": ""1768"",
                    ""schedule_at"": ""2018-07-06T11:39:03.000Z"",
                    ""triggered_by"": ""changes"",
                    ""_embedded"": {
                      ""stages"": [
                        {
                          ""name"": ""BuildStage"",
                          ""status"": ""Passed""
                        }
                      ]
                    }
                  }
                ]
              }
            },
            {
              ""name"": ""DirectaTrunk-Msi"",
              ""locked"": false,
              ""pause_info"": {
                ""paused"": false,
                ""paused_by"": null,
                ""pause_reason"": null
              },
              ""_embedded"": {
                ""instances"": [
                  {
                    ""label"": ""1488"",
                    ""schedule_at"": ""2018-07-06T11:59:15.000Z"",
                    ""triggered_by"": ""changes"",
                    ""_embedded"": {
                      ""stages"": [
                        {
                          ""name"": ""BuildStage"",
                          ""status"": ""Passed""
                        }
                      ]
                    }
                  }
                ]
              }
            }
          ]
        }
      },
      {
        ""name"": ""Test"",
        ""_embedded"": {
          ""pipelines"": [
          ]
        }
      }
    ]
  }
}";
            

            #endregion
            var httpClientHandler = new HttpClientHandlerFake { HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(jsonResult) } };

            // Act
            Result<List<Pipeline>> result;
            using (var service = new GocdService(new RestClient("https://buildserver:8154", "username", "password", true, httpClientHandler)))
            {
                result = service.GetPipelines();
            }

            // Assert
            Assert.That(httpClientHandler.RequestUri.ToString(), Does.EndWith("/go/api/dashboard"));
            Assert.That(httpClientHandler.AcceptHeaders.Count, Is.EqualTo(1));
            Assert.That(httpClientHandler.AcceptHeaders.Single().ToString(), Is.EqualTo("application/vnd.go.cd.v1+json"));

            Assert.That(result.IsValid, result.ToString());
            Assert.That(result.Data.Count, Is.EqualTo(2));

            Assert.That(result.Data[0].PipelineGroupName, Is.EqualTo("Build"));
            Assert.That(result.Data[0].Name, Is.EqualTo("DirectaTrunk"));

            Assert.That(result.Data[1].PipelineGroupName, Is.EqualTo("Build"));
            Assert.That(result.Data[1].Name, Is.EqualTo("DirectaTrunk-Msi"));
        }

        [Test]
        public void GetPipelines_DeserialisesThePipelineStage_AndItsPassedStatus_Correctly()
        {
            // Arrange
            #region jsonResult

            string jsonResult = @"
{
  ""_embedded"": {
    ""pipeline_groups"": [
      {
        ""name"": ""Build"",
        ""_embedded"": {
          ""pipelines"": [
            {
              ""name"": ""MsBuild"",
              ""locked"": false,
              ""pause_info"": {
                ""paused"": false,
                ""paused_by"": null,
                ""pause_reason"": null
              },
              ""_embedded"": {
                ""instances"": [
                  {
                    ""label"": ""57"",
                    ""schedule_at"": ""2018-06-20T16:00:00.000Z"",
                    ""triggered_by"": ""changes"",
                    ""_embedded"": {
                      ""stages"": [
                        {
                          ""name"": ""BuildStage"",
                          ""status"": ""Passed""
                        }
                      ]
                    }
                  }
                ]
              }
            }
          ]
        }
      }
    ]
  }
}
";
            #endregion
            var httpClientHandler = new HttpClientHandlerFake { HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(jsonResult) } };

            // Act
            Result<List<Pipeline>> result;
            using (var service = new GocdService(new RestClient("https://buildserver:8154", "username", "password", true, httpClientHandler)))
            {
                result = service.GetPipelines();
            }

            // Assert
            Assert.That(result.IsValid, result.ToString());
            Assert.That(result.Data.Count, Is.EqualTo(1));

            Assert.That(result.Data[0].PipelineGroupName, Is.EqualTo("Build"));
            Assert.That(result.Data[0].Name, Is.EqualTo("MsBuild"));

            Assert.That(result.Data[0].PipelineInstances.Count, Is.EqualTo(1));
            Assert.That(result.Data[0].PipelineInstances[0].Label, Is.EqualTo("57"));
            Assert.That(result.Data[0].PipelineInstances[0].ScheduledAt, Is.EqualTo(new DateTime(2018, 6, 20, 16, 0, 0, 0, DateTimeKind.Utc))); //2018-06-20T16:00:00.000Z
            Assert.That(result.Data[0].PipelineInstances[0].TriggeredBy, Is.EqualTo("changes"));

            Assert.That(result.Data[0].PipelineInstances[0].Stages.Count, Is.EqualTo(1));
            Assert.That(result.Data[0].PipelineInstances[0].Stages[0].Name, Is.EqualTo("BuildStage"));
            Assert.That(result.Data[0].PipelineInstances[0].Stages[0].Status, Is.EqualTo(StageStatus.Passed));
            Assert.That(result.Data[0].PipelineInstances[0].Stages[0].PreviousStatus, Is.Null);
        }

        [Test]
        public void GetPipelines_DeserialisesThePipelineStage_AndItsBuildingStatus_Correctly()
        {
            // Arrange
            #region jsonResult

            string jsonResult = @"
{
  ""_embedded"": {
    ""pipeline_groups"": [
      {
        ""name"": ""Build"",
        ""_embedded"": {
          ""pipelines"": [
            {
              ""name"": ""MsBuild"",
              ""locked"": false,
              ""pause_info"": {
                ""paused"": false,
                ""paused_by"": null,
                ""pause_reason"": null
              },
              ""_embedded"": {
                ""instances"": [
                  {
                    ""label"": ""58"",
                    ""schedule_at"": ""2018-07-11T14:19:05.000Z"",
                    ""triggered_by"": ""changes"",
                    ""_embedded"": {
                      ""stages"": [
                        {
                          ""name"": ""BuildStage"",
                          ""status"": ""Building"",
                          ""previous_stage"": {
                            ""name"": ""BuildStage"",
                            ""status"": ""Passed""
                          }
                        }
                      ]
                    }
                  }
                ]
              }
            }
          ]
        }
      }
    ]
  }
}

";
            #endregion
            var httpClientHandler = new HttpClientHandlerFake { HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(jsonResult) } };

            // Act
            Result<List<Pipeline>> result;
            using (var service = new GocdService(new RestClient("https://buildserver:8154", "username", "password", true, httpClientHandler)))
            {
                result = service.GetPipelines();
            }

            // Assert
            Assert.That(result.Data[0].PipelineInstances[0].Stages.Count, Is.EqualTo(1));
            Assert.That(result.Data[0].PipelineInstances[0].Stages[0].Name, Is.EqualTo("BuildStage"));
            Assert.That(result.Data[0].PipelineInstances[0].Stages[0].Status, Is.EqualTo(StageStatus.Building));
            Assert.That(result.Data[0].PipelineInstances[0].Stages[0].PreviousStatus.Value, Is.EqualTo(StageStatus.Passed));
        }

        [Test]
        public void GetPipelines_DeserialisesThePipelineStage_AndItsFailedStatus_Correctly()
        {
            // Arrange
            #region jsonResult

            string jsonResult = @"
{
  ""_embedded"": {
    ""pipeline_groups"": [
      {
        ""name"": ""Build"",
        ""_embedded"": {
          ""pipelines"": [
            {
              ""name"": ""MsBuild"",
              ""locked"": false,
              ""pause_info"": {
                ""paused"": false,
                ""paused_by"": null,
                ""pause_reason"": null
              },
              ""_embedded"": {
                ""instances"": [
                  {
                    ""label"": ""58"",
                    ""schedule_at"": ""2018-07-11T14:19:05.000Z"",
                    ""triggered_by"": ""changes"",
                    ""_embedded"": {
                      ""stages"": [
                        {
                          ""name"": ""BuildStage"",
                          ""status"": ""Failed""
                        }
                      ]
                    }
                  }
                ]
              }
            }
          ]
        }
      }
    ]
  }
}

";
            #endregion
            var httpClientHandler = new HttpClientHandlerFake { HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(jsonResult) } };

            // Act
            Result<List<Pipeline>> result;
            using (var service = new GocdService(new RestClient("https://buildserver:8154", "username", "password", true, httpClientHandler)))
            {
                result = service.GetPipelines();
            }

            // Assert
            Assert.That(result.Data[0].PipelineInstances[0].Stages.Count, Is.EqualTo(1));
            Assert.That(result.Data[0].PipelineInstances[0].Stages[0].Name, Is.EqualTo("BuildStage"));
            Assert.That(result.Data[0].PipelineInstances[0].Stages[0].Status, Is.EqualTo(StageStatus.Failed));
            Assert.That(result.Data[0].PipelineInstances[0].Stages[0].PreviousStatus, Is.Null);
        }

        [Test]
        public void GetPipelines_DeserialisesThePipelineStage_AndItsCancelledStatus_Correctly()
        {
            // Arrange
            #region jsonResult

            string jsonResult = @"
{
  ""_embedded"": {
    ""pipeline_groups"": [
      {
        ""name"": ""Build"",
        ""_embedded"": {
          ""pipelines"": [
            {
              ""name"": ""MsBuild"",
              ""locked"": false,
              ""pause_info"": {
                ""paused"": false,
                ""paused_by"": null,
                ""pause_reason"": null
              },
              ""_embedded"": {
                ""instances"": [
                  {
                    ""label"": ""59"",
                    ""schedule_at"": ""2018-07-11T16:40:16.000Z"",
                    ""triggered_by"": ""Mat.Roberts"",
                    ""_embedded"": {
                      ""stages"": [
                        {
                          ""name"": ""BuildStage"",
                          ""status"": ""Cancelled""
                        }
                      ]
                    }
                  }
                ]
              }
            }
          ]
        }
      }
    ]
  }
}

";
            #endregion
            var httpClientHandler = new HttpClientHandlerFake { HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(jsonResult) } };

            // Act
            Result<List<Pipeline>> result;
            using (var service = new GocdService(new RestClient("https://buildserver:8154", "username", "password", true, httpClientHandler)))
            {
                result = service.GetPipelines();
            }

            // Assert
            Assert.That(result.Data[0].PipelineInstances[0].Stages.Count, Is.EqualTo(1));
            Assert.That(result.Data[0].PipelineInstances[0].Stages[0].Name, Is.EqualTo("BuildStage"));
            Assert.That(result.Data[0].PipelineInstances[0].Stages[0].Status, Is.EqualTo(StageStatus.Cancelled));
            Assert.That(result.Data[0].PipelineInstances[0].Stages[0].PreviousStatus, Is.Null);
        }

        [Test]
        public void GetPipelines_DeserialisesPausedAndLocked_Correctly()
        {
            // Arrange
            #region jsonResult

            string jsonResult = @"
{
  ""_embedded"": {
    ""pipeline_groups"": [
      {
        ""name"": ""Build"",
        ""_embedded"": {
          ""pipelines"": [
            {
              ""name"": ""MsBuild"",
              ""locked"": true,
              ""pause_info"": {
                ""paused"": true,
                ""paused_by"": ""mat.roberts"",
                ""pause_reason"": ""editing""
              },
              ""_embedded"": {
                ""instances"": [
                  {
                    ""label"": ""59"",
                    ""schedule_at"": ""2018-07-11T16:40:16.000Z"",
                    ""triggered_by"": ""Mat.Roberts"",
                    ""_embedded"": {
                      ""stages"": [
                        {
                          ""name"": ""BuildStage"",
                          ""status"": ""Cancelled""
                        }
                      ]
                    }
                  }
                ]
              }
            }
          ]
        }
      }
    ]
  }
}
";
            #endregion
            var httpClientHandler = new HttpClientHandlerFake { HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(jsonResult) } };

            // Act
            Result<List<Pipeline>> result;
            using (var service = new GocdService(new RestClient("https://buildserver:8154", "username", "password", true, httpClientHandler)))
            {
                result = service.GetPipelines();
            }

            // Assert
            Assert.That(result.Data[0].Name, Is.EqualTo("MsBuild"));
            Assert.That(result.Data[0].Locked, Is.True);
            Assert.That(result.Data[0].Paused, Is.True);
            Assert.That(result.Data[0].PausedBy, Is.EqualTo("mat.roberts"));
            Assert.That(result.Data[0].PausedReason, Is.EqualTo("editing"));
        }

        [Test]
        public void GetPipelines_ANewPipeline_WhichHasNeverBeenRun_DeserialisesCorrectly()
        {
            // Arrange
            #region jsonResult

            string jsonResult = @"
{
  ""_embedded"": {
    ""pipeline_groups"": [
      {
        ""name"": ""Admin"",
        ""_embedded"": {
          ""pipelines"": [
            {
              ""name"": ""NewPipeline"",
              ""locked"": false,
              ""pause_info"": {
                ""paused"": true,
                ""paused_by"": ""mat.roberts"",
                ""pause_reason"": ""Under construction""
              },
              ""_embedded"": {
                ""instances"": [

                ]
              }
            }
          ]
        }
      }
    ]
  }
}
";
            #endregion
            var httpClientHandler = new HttpClientHandlerFake { HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(jsonResult) } };

            // Act
            Result<List<Pipeline>> result;
            using (var service = new GocdService(new RestClient("https://buildserver:8154", "username", "password", true, httpClientHandler)))
            {
                result = service.GetPipelines();
            }

            // Assert
            Assert.That(result.Data[0].Name, Is.EqualTo("NewPipeline"));
            Assert.That(result.Data[0].PipelineGroupName, Is.EqualTo("Admin"));
            Assert.That(result.Data[0].Locked, Is.False);
            Assert.That(result.Data[0].Paused, Is.True);
            Assert.That(result.Data[0].PausedBy, Is.EqualTo("mat.roberts"));
            Assert.That(result.Data[0].PausedReason, Is.EqualTo("Under construction"));
            Assert.That(result.Data[0].PipelineInstances.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetPipelines_WhenPipelineHasTwoStages_DeserialisesCorrectly()
        {
            // Arrange
            #region jsonResult

            string jsonResult = @"

{
  ""_embedded"": {
    ""pipeline_groups"": [
      {
        ""name"": ""Branch"",
        ""_embedded"": {
          ""pipelines"": [
            {
              ""name"": ""NetBackupAdapter9r1v8"",
              ""locked"": false,
              ""pause_info"": {
                ""paused"": false,
                ""paused_by"": null,
                ""pause_reason"": null
              },
              ""_embedded"": {
                ""instances"": [
                  {
                    ""label"": ""68"",
                    ""schedule_at"": ""2018-07-12T09:33:15.000Z"",
                    ""triggered_by"": ""changes"",
                    ""_embedded"": {
                      ""stages"": [
                        {
                          ""name"": ""BuildStage"",
                          ""status"": ""Passed""
                        },
                        {
                          ""name"": ""TestStage"",
                          ""status"": ""Building"",
                          ""previous_stage"": {
                            ""name"": ""TestStage"",
                            ""status"": ""Passed""
                          }
                        }
                      ]
                    }
                  }
                ]
              }
            }
          ]
        }
      }
    ]
  }
}

";
            #endregion
            var httpClientHandler = new HttpClientHandlerFake { HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(jsonResult) } };

            // Act
            Result<List<Pipeline>> result;
            using (var service = new GocdService(new RestClient("https://buildserver:8154", "username", "password", true, httpClientHandler)))
            {
                result = service.GetPipelines();
            }

            // Assert
            Assert.That(result.Data[0].Name, Is.EqualTo("NetBackupAdapter9r1v8"));
            Assert.That(result.Data[0].PipelineInstances.Count, Is.EqualTo(1));

            Assert.That(result.Data[0].PipelineInstances[0].Stages.Count, Is.EqualTo(2));
            Assert.That(result.Data[0].PipelineInstances[0].Stages[0].Name, Is.EqualTo("BuildStage"));
            Assert.That(result.Data[0].PipelineInstances[0].Stages[0].Status, Is.EqualTo(StageStatus.Passed));
            Assert.That(result.Data[0].PipelineInstances[0].Stages[0].PreviousStatus, Is.Null);
            Assert.That(result.Data[0].PipelineInstances[0].Stages[1].Name, Is.EqualTo("TestStage"));
            Assert.That(result.Data[0].PipelineInstances[0].Stages[1].Status, Is.EqualTo(StageStatus.Building));
            Assert.That(result.Data[0].PipelineInstances[0].Stages[1].PreviousStatus, Is.EqualTo(StageStatus.Passed));
        }

        [Test]
        public void GetPipelines_WhenPipelineHasTwoInstances_DeserialisesCorrectly()
        {
            // Arrange
            #region jsonResult

            string jsonResult = @"

{
  ""_embedded"": {
    ""pipeline_groups"": [
      {
        ""name"": ""Branch"",
        ""_embedded"": {
          ""pipelines"": [
            {
              ""name"": ""NetBackupAdapter9r1v8"",
              ""locked"": false,
              ""pause_info"": {
                ""paused"": false,
                ""paused_by"": null,
                ""pause_reason"": null
              },
              ""_embedded"": {
                ""instances"": [
                  {
                    ""label"": ""69"",
                    ""schedule_at"": ""2018-07-12T09:48:15.000Z"",
                    ""triggered_by"": ""changes"",
                    ""_embedded"": {
                      ""stages"": [
                        {
                          ""name"": ""BuildStage"",
                          ""status"": ""Building"",
                          ""previous_stage"": {
                            ""name"": ""BuildStage"",
                            ""status"": ""Passed""
                          }
                        },
                        {
                          ""name"": ""TestStage"",
                          ""status"": ""Unknown""
                        }
                      ]
                    }
                  },
                  {
                    ""label"": ""68"",
                    ""schedule_at"": ""2018-07-12T09:33:15.000Z"",
                    ""triggered_by"": ""changes"",
                    ""_embedded"": {
                      ""stages"": [
                        {
                          ""name"": ""BuildStage"",
                          ""status"": ""Passed""
                        },
                        {
                          ""name"": ""TestStage"",
                          ""status"": ""Building"",
                          ""previous_stage"": {
                            ""name"": ""TestStage"",
                            ""status"": ""Passed""
                          }
                        }
                      ]
                    }
                  }
                ]
              }
            }
          ]
        }
      }
    ]
  }
}
";
            #endregion
            var httpClientHandler = new HttpClientHandlerFake { HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(jsonResult) } };

            // Act
            Result<List<Pipeline>> result;
            using (var service = new GocdService(new RestClient("https://buildserver:8154", "username", "password", true, httpClientHandler)))
            {
                result = service.GetPipelines();
            }

            // Assert
            Assert.That(result.Data[0].Name, Is.EqualTo("NetBackupAdapter9r1v8"));
            Assert.That(result.Data[0].PipelineInstances.Count, Is.EqualTo(2));

            Assert.That(result.Data[0].PipelineInstances[0].Label, Is.EqualTo("69"));
            Assert.That(result.Data[0].PipelineInstances[0].Stages.Count, Is.EqualTo(2));
            Assert.That(result.Data[0].PipelineInstances[0].Stages[0].Name, Is.EqualTo("BuildStage"));
            Assert.That(result.Data[0].PipelineInstances[0].Stages[0].Status, Is.EqualTo(StageStatus.Building));
            Assert.That(result.Data[0].PipelineInstances[0].Stages[0].PreviousStatus, Is.EqualTo(StageStatus.Passed));
            Assert.That(result.Data[0].PipelineInstances[0].Stages[1].Name, Is.EqualTo("TestStage"));
            Assert.That(result.Data[0].PipelineInstances[0].Stages[1].Status, Is.EqualTo(StageStatus.Unknown));
            Assert.That(result.Data[0].PipelineInstances[0].Stages[1].PreviousStatus, Is.Null);

            Assert.That(result.Data[0].PipelineInstances[1].Label, Is.EqualTo("68"));
            Assert.That(result.Data[0].PipelineInstances[1].Stages.Count, Is.EqualTo(2));
            Assert.That(result.Data[0].PipelineInstances[1].Stages[0].Name, Is.EqualTo("BuildStage"));
            Assert.That(result.Data[0].PipelineInstances[1].Stages[0].Status, Is.EqualTo(StageStatus.Passed));
            Assert.That(result.Data[0].PipelineInstances[1].Stages[0].PreviousStatus, Is.Null);
            Assert.That(result.Data[0].PipelineInstances[1].Stages[1].Name, Is.EqualTo("TestStage"));
            Assert.That(result.Data[0].PipelineInstances[1].Stages[1].Status, Is.EqualTo(StageStatus.Building));
            Assert.That(result.Data[0].PipelineInstances[1].Stages[1].PreviousStatus, Is.EqualTo(StageStatus.Passed));
        }

        // How deal with failure to connect, or interuption?
        // Not doing it now but may want to connect to more than one go.cd
        // async



    }
}