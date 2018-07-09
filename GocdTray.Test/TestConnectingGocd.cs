using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using GocdTray.App;
using GocdTray.Rest;
using NUnit.Framework;

namespace GocdTray.Test
{
    [TestFixture]
    public class TestConnectingGocd
    {
        // connect to go.cd and get the data back wot i want in an object

        [Test]
        public void RealConnection()
        {
            var restClient = new RestClient(AppConfig.GocdApiUri, AppConfig.Username, AppConfig.Password, AppConfig.IgnoreCertificateErrors);
            var gocdServer = new GocdServer(restClient);
            var result = gocdServer.GetDashboard();
            
            Assert.That(result.HasData, result.ToString());
            Console.WriteLine(result.Data);

        }

        [Test]
        public void GetDashboard_ResultIsDeserialisedCorrectly()
        {
            string json = @"{
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

            var result = json.FromJson<GoEmbedded<GoPipelineGroupsList>>();

            Assert.That(result._embedded.PipelineGroups.Count, Is.EqualTo(2));

            Assert.That(result._embedded.PipelineGroups[0].Name, Is.EqualTo("Build"));
            Assert.That(result._embedded.PipelineGroups[0]._embedded.pipelines.Count, Is.EqualTo(2));
            Assert.That(result._embedded.PipelineGroups[0]._embedded.pipelines[0].Name, Is.EqualTo("DirectaTrunk"));
            Assert.That(result._embedded.PipelineGroups[0]._embedded.pipelines[1].Name, Is.EqualTo("DirectaTrunk-Msi"));

            Assert.That(result._embedded.PipelineGroups[1].Name, Is.EqualTo("Test"));
            Assert.That(result._embedded.PipelineGroups[1]._embedded.pipelines.Count, Is.EqualTo(0));

        }

        // Post process object to get what i want
        // Sort out http shite to make it testable
        // Reorg project

        // invliad urls
        // if it throws exceptions (e.g. cert not vliad)

        // Http client setup
        // Need to vary accept header on call by call basis
        // How deal with failure to connect, or interuption?
        // Not doing it now but may want to connect to more than one go.cd
        // Need to process the return data, and test the processing
        // async
        // request errors - catach exceptions
        // response errors - validation object
    }
}