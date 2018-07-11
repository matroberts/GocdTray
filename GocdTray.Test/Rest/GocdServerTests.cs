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
    public class GocdServerTests
    {

        [Test]
        public void RealConnection()
        {
            var restClient = new RestClient(AppConfig.GocdApiUri, AppConfig.Username, AppConfig.Password, AppConfig.IgnoreCertificateErrors);
            var gocdServer = new GocdServer(restClient);
            var result = gocdServer.GetPipelines();
            
            Assert.That(result.HasData, result.ToString());
            Console.WriteLine(result.Data);
        }

        [Test]
        public void GetPipelines_DeserialisesThePipelineAndPipelineGroupName_Correctly()
        {
            // Arrange
            #region jsonResult

            string jsonResult = @"{
  ""_links"": {
    ""self"": {
      ""href"": ""https://devbuild03:8154/go/api/dashboard""
    },
    ""doc"": {
      ""href"": ""https://api.gocd.io/#dashboard""
    }
  },
  ""_embedded"": {
    ""pipeline_groups"": [
      {
        ""_links"": {
          ""self"": {
            ""href"": ""https://devbuild03:8154/go/api/config/pipeline_groups""
          },
          ""doc"": {
            ""href"": ""https://api.gocd.io/#pipeline-groups""
          }
        },
        ""name"": ""Build"",
        ""_embedded"": {
          ""pipelines"": [
            {
              ""_links"": {
                ""self"": {
                  ""href"": ""https://devbuild03:8154/go/api/pipelines/DirectaTrunk/history""
                },
                ""doc"": {
                  ""href"": ""https://api.gocd.io/#pipelines""
                },
                ""settings_path"": {
                  ""href"": ""https://devbuild03:8154/go/admin/pipelines/DirectaTrunk/general""
                },
                ""trigger"": {
                  ""href"": ""https://devbuild03:8154/go/api/pipelines/DirectaTrunk/schedule""
                },
                ""trigger_with_options"": {
                  ""href"": ""https://devbuild03:8154/go/api/pipelines/DirectaTrunk/schedule""
                },
                ""pause"": {
                  ""href"": ""https://devbuild03:8154/go/api/pipelines/DirectaTrunk/pause""
                },
                ""unpause"": {
                  ""href"": ""https://devbuild03:8154/go/api/pipelines/DirectaTrunk/unpause""
                }
              },
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
                    ""_links"": {
                      ""self"": {
                        ""href"": ""https://devbuild03:8154/go/api/pipelines/DirectaTrunk/instance/1768""
                      },
                      ""doc"": {
                        ""href"": ""https://api.gocd.io/#get-pipeline-instance""
                      },
                      ""history_url"": {
                        ""href"": ""https://devbuild03:8154/go/api/pipelines/DirectaTrunk/history""
                      },
                      ""vsm_url"": {
                        ""href"": ""https://devbuild03:8154/go/pipelines/value_stream_map/DirectaTrunk/1768""
                      },
                      ""compare_url"": {
                        ""href"": ""https://devbuild03:8154/go/compare/DirectaTrunk/1767/with/1768""
                      },
                      ""build_cause_url"": {
                        ""href"": ""https://devbuild03:8154/go/pipelines/DirectaTrunk/1768/build_cause""
                      }
                    },
                    ""label"": ""1768"",
                    ""schedule_at"": ""2018-07-06T11:39:03.000Z"",
                    ""triggered_by"": ""changes"",
                    ""_embedded"": {
                      ""stages"": [
                        {
                          ""_links"": {
                            ""self"": {
                              ""href"": ""https://devbuild03:8154/go/api/stages/DirectaTrunk/1768/BuildStage/1""
                            },
                            ""doc"": {
                              ""href"": ""https://api.gocd.io/#get-stage-instance""
                            }
                          },
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
              ""_links"": {
                ""self"": {
                  ""href"": ""https://devbuild03:8154/go/api/pipelines/DirectaTrunk-Msi/history""
                },
                ""doc"": {
                  ""href"": ""https://api.gocd.io/#pipelines""
                },
                ""settings_path"": {
                  ""href"": ""https://devbuild03:8154/go/admin/pipelines/DirectaTrunk-Msi/general""
                },
                ""trigger"": {
                  ""href"": ""https://devbuild03:8154/go/api/pipelines/DirectaTrunk-Msi/schedule""
                },
                ""trigger_with_options"": {
                  ""href"": ""https://devbuild03:8154/go/api/pipelines/DirectaTrunk-Msi/schedule""
                },
                ""pause"": {
                  ""href"": ""https://devbuild03:8154/go/api/pipelines/DirectaTrunk-Msi/pause""
                },
                ""unpause"": {
                  ""href"": ""https://devbuild03:8154/go/api/pipelines/DirectaTrunk-Msi/unpause""
                }
              },
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
                    ""_links"": {
                      ""self"": {
                        ""href"": ""https://devbuild03:8154/go/api/pipelines/DirectaTrunk-Msi/instance/1488""
                      },
                      ""doc"": {
                        ""href"": ""https://api.gocd.io/#get-pipeline-instance""
                      },
                      ""history_url"": {
                        ""href"": ""https://devbuild03:8154/go/api/pipelines/DirectaTrunk-Msi/history""
                      },
                      ""vsm_url"": {
                        ""href"": ""https://devbuild03:8154/go/pipelines/value_stream_map/DirectaTrunk-Msi/1488""
                      },
                      ""compare_url"": {
                        ""href"": ""https://devbuild03:8154/go/compare/DirectaTrunk-Msi/1487/with/1488""
                      },
                      ""build_cause_url"": {
                        ""href"": ""https://devbuild03:8154/go/pipelines/DirectaTrunk-Msi/1488/build_cause""
                      }
                    },
                    ""label"": ""1488"",
                    ""schedule_at"": ""2018-07-06T11:59:15.000Z"",
                    ""triggered_by"": ""changes"",
                    ""_embedded"": {
                      ""stages"": [
                        {
                          ""_links"": {
                            ""self"": {
                              ""href"": ""https://devbuild03:8154/go/api/stages/DirectaTrunk-Msi/1488/BuildStage/1""
                            },
                            ""doc"": {
                              ""href"": ""https://api.gocd.io/#get-stage-instance""
                            }
                          },
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
        ""_links"": {
          ""self"": {
            ""href"": ""https://devbuild03:8154/go/api/config/pipeline_groups""
          },
          ""doc"": {
            ""href"": ""https://api.gocd.io/#pipeline-groups""
          }
        },
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
            RestResult<List<Pipeline>> result;
            using (var gocdServer = new GocdServer(new RestClient(AppConfig.GocdApiUri, AppConfig.Username, AppConfig.Password, AppConfig.IgnoreCertificateErrors, httpClientHandler)))
            {
                result = gocdServer.GetPipelines();
            }

            // Assert
            Assert.That(httpClientHandler.RequestUri.ToString(), Does.EndWith("/go/api/dashboard"));
            Assert.That(httpClientHandler.AcceptHeaders.Count, Is.EqualTo(1));
            Assert.That(httpClientHandler.AcceptHeaders.Single().ToString(), Is.EqualTo("application/vnd.go.cd.v1+json"));

            Assert.That(result.HasData, result.ToString());
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
  ""_links"": {
    ""self"": {
      ""href"": ""https://devbuild03:8154/go/api/dashboard""
    },
    ""doc"": {
      ""href"": ""https://api.gocd.io/#dashboard""
    }
  },
  ""_embedded"": {
    ""pipeline_groups"": [
      {
        ""_links"": {
          ""self"": {
            ""href"": ""https://devbuild03:8154/go/api/config/pipeline_groups""
          },
          ""doc"": {
            ""href"": ""https://api.gocd.io/#pipeline-groups""
          }
        },
        ""name"": ""Build"",
        ""_embedded"": {
          ""pipelines"": [
            {
              ""_links"": {
                ""self"": {
                  ""href"": ""https://devbuild03:8154/go/api/pipelines/MsBuild/history""
                },
                ""doc"": {
                  ""href"": ""https://api.gocd.io/#pipelines""
                },
                ""settings_path"": {
                  ""href"": ""https://devbuild03:8154/go/admin/pipelines/MsBuild/general""
                },
                ""trigger"": {
                  ""href"": ""https://devbuild03:8154/go/api/pipelines/MsBuild/schedule""
                },
                ""trigger_with_options"": {
                  ""href"": ""https://devbuild03:8154/go/api/pipelines/MsBuild/schedule""
                },
                ""pause"": {
                  ""href"": ""https://devbuild03:8154/go/api/pipelines/MsBuild/pause""
                },
                ""unpause"": {
                  ""href"": ""https://devbuild03:8154/go/api/pipelines/MsBuild/unpause""
                }
              },
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
                    ""_links"": {
                      ""self"": {
                        ""href"": ""https://devbuild03:8154/go/api/pipelines/MsBuild/instance/57""
                      },
                      ""doc"": {
                        ""href"": ""https://api.gocd.io/#get-pipeline-instance""
                      },
                      ""history_url"": {
                        ""href"": ""https://devbuild03:8154/go/api/pipelines/MsBuild/history""
                      },
                      ""vsm_url"": {
                        ""href"": ""https://devbuild03:8154/go/pipelines/value_stream_map/MsBuild/57""
                      },
                      ""compare_url"": {
                        ""href"": ""https://devbuild03:8154/go/compare/MsBuild/56/with/57""
                      },
                      ""build_cause_url"": {
                        ""href"": ""https://devbuild03:8154/go/pipelines/MsBuild/57/build_cause""
                      }
                    },
                    ""label"": ""57"",
                    ""schedule_at"": ""2018-06-20T16:00:00.000Z"",
                    ""triggered_by"": ""changes"",
                    ""_embedded"": {
                      ""stages"": [
                        {
                          ""_links"": {
                            ""self"": {
                              ""href"": ""https://devbuild03:8154/go/api/stages/MsBuild/57/BuildStage/1""
                            },
                            ""doc"": {
                              ""href"": ""https://api.gocd.io/#get-stage-instance""
                            }
                          },
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
            RestResult<List<Pipeline>> result;
            using (var gocdServer = new GocdServer(new RestClient(AppConfig.GocdApiUri, AppConfig.Username, AppConfig.Password, AppConfig.IgnoreCertificateErrors, httpClientHandler)))
            {
                result = gocdServer.GetPipelines();
            }

            // Assert
            Assert.That(result.HasData, result.ToString());
            Assert.That(result.Data.Count, Is.EqualTo(1));

            Assert.That(result.Data[0].PipelineGroupName, Is.EqualTo("Build"));
            Assert.That(result.Data[0].Name, Is.EqualTo("MsBuild"));

            Assert.That(result.Data[0].PipelineInstances.Count, Is.EqualTo(1));
            Assert.That(result.Data[0].PipelineInstances[0].Label, Is.EqualTo("57"));
            Assert.That(result.Data[0].PipelineInstances[0].ScheduledAt, Is.EqualTo(new DateTime(2018, 6, 20, 16, 0, 0, 0, DateTimeKind.Utc))); //2018-06-20T16:00:00.000Z
            Assert.That(result.Data[0].PipelineInstances[0].TriggeredBy, Is.EqualTo("changes"));

            Assert.That(result.Data[0].PipelineInstances[0].Stages.Count, Is.EqualTo(1));
            Assert.That(result.Data[0].PipelineInstances[0].Stages[0].Name, Is.EqualTo("BuildStage"));
            Assert.That(result.Data[0].PipelineInstances[0].Stages[0].Status, Is.EqualTo(BuildStatus.Passed));
        }

        [Test]
        public void GetPipelines_DeserialisesThePipelineStage_AndItsBuildingStatus_Correctly()
        {
            // Arrange
            #region jsonResult

            string jsonResult = @"
{
  ""_links"": {
    ""self"": {
      ""href"": ""https://devbuild03:8154/go/api/dashboard""
    },
    ""doc"": {
      ""href"": ""https://api.gocd.io/#dashboard""
    }
  },
  ""_embedded"": {
    ""pipeline_groups"": [
      {
        ""_links"": {
          ""self"": {
            ""href"": ""https://devbuild03:8154/go/api/config/pipeline_groups""
          },
          ""doc"": {
            ""href"": ""https://api.gocd.io/#pipeline-groups""
          }
        },
        ""name"": ""Build"",
        ""_embedded"": {
          ""pipelines"": [
            {
              ""_links"": {
                ""self"": {
                  ""href"": ""https://devbuild03:8154/go/api/pipelines/MsBuild/history""
                },
                ""doc"": {
                  ""href"": ""https://api.gocd.io/#pipelines""
                },
                ""settings_path"": {
                  ""href"": ""https://devbuild03:8154/go/admin/pipelines/MsBuild/general""
                },
                ""trigger"": {
                  ""href"": ""https://devbuild03:8154/go/api/pipelines/MsBuild/schedule""
                },
                ""trigger_with_options"": {
                  ""href"": ""https://devbuild03:8154/go/api/pipelines/MsBuild/schedule""
                },
                ""pause"": {
                  ""href"": ""https://devbuild03:8154/go/api/pipelines/MsBuild/pause""
                },
                ""unpause"": {
                  ""href"": ""https://devbuild03:8154/go/api/pipelines/MsBuild/unpause""
                }
              },
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
                    ""_links"": {
                      ""self"": {
                        ""href"": ""https://devbuild03:8154/go/api/pipelines/MsBuild/instance/58""
                      },
                      ""doc"": {
                        ""href"": ""https://api.gocd.io/#get-pipeline-instance""
                      },
                      ""history_url"": {
                        ""href"": ""https://devbuild03:8154/go/api/pipelines/MsBuild/history""
                      },
                      ""vsm_url"": {
                        ""href"": ""https://devbuild03:8154/go/pipelines/value_stream_map/MsBuild/58""
                      },
                      ""compare_url"": {
                        ""href"": ""https://devbuild03:8154/go/compare/MsBuild/57/with/58""
                      },
                      ""build_cause_url"": {
                        ""href"": ""https://devbuild03:8154/go/pipelines/MsBuild/58/build_cause""
                      }
                    },
                    ""label"": ""58"",
                    ""schedule_at"": ""2018-07-11T14:19:05.000Z"",
                    ""triggered_by"": ""changes"",
                    ""_embedded"": {
                      ""stages"": [
                        {
                          ""_links"": {
                            ""self"": {
                              ""href"": ""https://devbuild03:8154/go/api/stages/MsBuild/58/BuildStage/1""
                            },
                            ""doc"": {
                              ""href"": ""https://api.gocd.io/#get-stage-instance""
                            }
                          },
                          ""name"": ""BuildStage"",
                          ""status"": ""Building"",
                          ""previous_stage"": {
                            ""_links"": {
                              ""self"": {
                                ""href"": ""https://devbuild03:8154/go/api/stages/MsBuild/57/BuildStage/1""
                              },
                              ""doc"": {
                                ""href"": ""https://api.gocd.io/#get-stage-instance""
                              }
                            },
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
            RestResult<List<Pipeline>> result;
            using (var gocdServer = new GocdServer(new RestClient(AppConfig.GocdApiUri, AppConfig.Username, AppConfig.Password, AppConfig.IgnoreCertificateErrors, httpClientHandler)))
            {
                result = gocdServer.GetPipelines();
            }

            // Assert
            Assert.That(result.Data[0].PipelineInstances[0].Stages.Count, Is.EqualTo(1));
            Assert.That(result.Data[0].PipelineInstances[0].Stages[0].Name, Is.EqualTo("BuildStage"));
            Assert.That(result.Data[0].PipelineInstances[0].Stages[0].Status, Is.EqualTo(BuildStatus.Building));
        }

        [Test]
        public void GetPipelines_DeserialisesThePipelineStage_AndItsFailedStatus_Correctly()
        {
            // Arrange
            #region jsonResult

            string jsonResult = @"
{
  ""_links"": {
    ""self"": {
      ""href"": ""https://devbuild03:8154/go/api/dashboard""
    },
    ""doc"": {
      ""href"": ""https://api.gocd.io/#dashboard""
    }
  },
  ""_embedded"": {
    ""pipeline_groups"": [
      {
        ""_links"": {
          ""self"": {
            ""href"": ""https://devbuild03:8154/go/api/config/pipeline_groups""
          },
          ""doc"": {
            ""href"": ""https://api.gocd.io/#pipeline-groups""
          }
        },
        ""name"": ""Build"",
        ""_embedded"": {
          ""pipelines"": [
            {
              ""_links"": {
                ""self"": {
                  ""href"": ""https://devbuild03:8154/go/api/pipelines/MsBuild/history""
                },
                ""doc"": {
                  ""href"": ""https://api.gocd.io/#pipelines""
                },
                ""settings_path"": {
                  ""href"": ""https://devbuild03:8154/go/admin/pipelines/MsBuild/general""
                },
                ""trigger"": {
                  ""href"": ""https://devbuild03:8154/go/api/pipelines/MsBuild/schedule""
                },
                ""trigger_with_options"": {
                  ""href"": ""https://devbuild03:8154/go/api/pipelines/MsBuild/schedule""
                },
                ""pause"": {
                  ""href"": ""https://devbuild03:8154/go/api/pipelines/MsBuild/pause""
                },
                ""unpause"": {
                  ""href"": ""https://devbuild03:8154/go/api/pipelines/MsBuild/unpause""
                }
              },
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
                    ""_links"": {
                      ""self"": {
                        ""href"": ""https://devbuild03:8154/go/api/pipelines/MsBuild/instance/58""
                      },
                      ""doc"": {
                        ""href"": ""https://api.gocd.io/#get-pipeline-instance""
                      },
                      ""history_url"": {
                        ""href"": ""https://devbuild03:8154/go/api/pipelines/MsBuild/history""
                      },
                      ""vsm_url"": {
                        ""href"": ""https://devbuild03:8154/go/pipelines/value_stream_map/MsBuild/58""
                      },
                      ""compare_url"": {
                        ""href"": ""https://devbuild03:8154/go/compare/MsBuild/57/with/58""
                      },
                      ""build_cause_url"": {
                        ""href"": ""https://devbuild03:8154/go/pipelines/MsBuild/58/build_cause""
                      }
                    },
                    ""label"": ""58"",
                    ""schedule_at"": ""2018-07-11T14:19:05.000Z"",
                    ""triggered_by"": ""changes"",
                    ""_embedded"": {
                      ""stages"": [
                        {
                          ""_links"": {
                            ""self"": {
                              ""href"": ""https://devbuild03:8154/go/api/stages/MsBuild/58/BuildStage/1""
                            },
                            ""doc"": {
                              ""href"": ""https://api.gocd.io/#get-stage-instance""
                            }
                          },
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
            RestResult<List<Pipeline>> result;
            using (var gocdServer = new GocdServer(new RestClient(AppConfig.GocdApiUri, AppConfig.Username, AppConfig.Password, AppConfig.IgnoreCertificateErrors, httpClientHandler)))
            {
                result = gocdServer.GetPipelines();
            }

            // Assert
            Assert.That(result.Data[0].PipelineInstances[0].Stages.Count, Is.EqualTo(1));
            Assert.That(result.Data[0].PipelineInstances[0].Stages[0].Name, Is.EqualTo("BuildStage"));
            Assert.That(result.Data[0].PipelineInstances[0].Stages[0].Status, Is.EqualTo(BuildStatus.Failed));
        }

        // How deal with failure to connect, or interuption?
        // Not doing it now but may want to connect to more than one go.cd
        // async


        // pipeline
        // - locked
        // - paused (and by and reason)
        //   instances
        //   - label
        //   - scheduled at
        //   - triggered by
        //     stages
        //     - name
        //     - status
        //     - previous stage????

    }
}