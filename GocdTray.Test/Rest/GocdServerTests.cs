using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using GocdTray.App;
using GocdTray.Rest;
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
        public void GetPipelines_DeserialisesTheDataCorrectly()
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
            var restClient = new RestClient(AppConfig.GocdApiUri, AppConfig.Username, AppConfig.Password, AppConfig.IgnoreCertificateErrors, httpClientHandler);

            // Act
            var gocdServer = new GocdServer(restClient);
            var result = gocdServer.GetPipelines();

            // Assert
            Assert.That(httpClientHandler.RequestUri.ToString(), Does.EndWith("/go/api/dashboard"));
            Assert.That(httpClientHandler.AcceptHeaders.Count, Is.EqualTo(1));
            Assert.That(httpClientHandler.AcceptHeaders.Single().ToString(), Is.EqualTo("application/vnd.go.cd.v1+json"));

            Assert.That(result.HasData, result.ToString());
            Assert.That(result.Data._embedded.PipelineGroups.Count, Is.EqualTo(2));

            Assert.That(result.Data._embedded.PipelineGroups[0].Name, Is.EqualTo("Build"));
            Assert.That(result.Data._embedded.PipelineGroups[0]._embedded.pipelines.Count, Is.EqualTo(2));
            Assert.That(result.Data._embedded.PipelineGroups[0]._embedded.pipelines[0].Name, Is.EqualTo("DirectaTrunk"));
            Assert.That(result.Data._embedded.PipelineGroups[0]._embedded.pipelines[1].Name, Is.EqualTo("DirectaTrunk-Msi"));

            Assert.That(result.Data._embedded.PipelineGroups[1].Name, Is.EqualTo("Test"));
            Assert.That(result.Data._embedded.PipelineGroups[1]._embedded.pipelines.Count, Is.EqualTo(0));
        }

        // connect to go.cd and get the data back wot i want in an object

        // Post process object to get what i want


    }
}