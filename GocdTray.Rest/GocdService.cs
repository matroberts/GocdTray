using System;
using System.Collections.Generic;
using System.Linq;
using GocdTray.App.Abstractions;
using GocdTray.Rest.Dto;

namespace GocdTray.Rest
{
    public class GocdService : IDisposable
    {
        private readonly IRestClient restClient;

        public GocdService(IRestClient restClient)
        {
            this.restClient = restClient;
        }

        public Result<List<Pipeline>> GetPipelines()
        {
            var result = restClient.Get<DtoEmbedded<DtoPipelineGroupsList>>("/go/api/dashboard", "application/vnd.go.cd.v1+json");
            if (result.IsValid)
            {
                var pipelines = new List<Pipeline>();
                foreach (var dtoPipelineGroup in result.Data._embedded.PipelineGroups)
                {
                    var pipelineGroupName = dtoPipelineGroup.Name;
                    foreach (var dtoPipeline in dtoPipelineGroup._embedded.pipelines)
                    {
                        var pipeline = new Pipeline()
                        {
                            PipelineGroupName = pipelineGroupName,
                            Name = dtoPipeline.Name,
                            Locked = dtoPipeline.locked,
                            Paused = dtoPipeline.pause_info.paused,
                            PausedBy = dtoPipeline.pause_info.paused_by,
                            PausedReason = dtoPipeline.pause_info.pause_reason
                        };
                        pipelines.Add(pipeline);
                        foreach (var dtoInstance in dtoPipeline._embedded.instances)
                        {
                            var instance = new PipelineInstance
                            {
                                Label = dtoInstance.label,
                                TriggeredBy = dtoInstance.triggered_by,
                                ScheduledAt = dtoInstance.schedule_at
                            };
                            pipeline.PipelineInstances.Add(instance);
                            foreach (var dtoStage in dtoInstance._embedded.stages)
                            {
                                var stage = new Stage
                                {
                                    Name = dtoStage.name,
                                    Status = dtoStage.status.ToEnum<StageStatus>(),
                                    PreviousStatus = dtoStage?.previous_stage?.status.ToEnumNullable<StageStatus>(),
                                };
                                instance.Stages.Add(stage);
                            }
                        }    
                    }
                }
                return Result<List<Pipeline>>.Valid(pipelines);
            }
            else
            {
                return Result<List<Pipeline>>.Invalid(result.ErrorMessage);
            }
        }

        public void Dispose()
        {
            restClient?.Dispose();
        }
    }
}