using System;
using System.Linq;
using GocdTray.App.Abstractions;
using NUnit.Framework;

namespace GocdTray.Test.Abstractions
{
    [TestFixture]
    public class PipelineTests
    {
        // Collapse Stage Statuses:
        // Stage         Pipeline
        // Building   => Building
        // Failed     => Failed
        // Cancelled  => Cancelled
        // Passed     => Passed
        // Unknown    => Unknown
        //
        // When multiple stages and multiple instances:
        // if anything Building    => Building
        // else if anything Failed => Failed
        // else                    => Passed

        [Test]
        public void PipelineStatus_IfTheBuildHasNeverRun_ThatIsThereAreNoPipelineInstances_ThatCountsAsAPass()
        {
            // Arrange
            var pipeline = new Pipeline();

            // Act/Assert
            Assert.That(pipeline.Status, Is.EqualTo(PipelineStatus.Passed));
        }

        [TestCase(StageStatus.Building,  PipelineStatus.Building)]
        [TestCase(StageStatus.Failed,    PipelineStatus.Failed)]
        [TestCase(StageStatus.Cancelled, PipelineStatus.Failed)]
        [TestCase(StageStatus.Passed,    PipelineStatus.Passed)]
        [TestCase(StageStatus.Unknown,   PipelineStatus.Passed)]
        public void PipelineStatus_OneInstance_OneStage_CancelledIsFailed_UnknowenIsPassed(StageStatus stageStatus, PipelineStatus pipelineStatus)
        {
            // Arrange
            var pipeline = new Pipeline() {PipelineInstances = {new PipelineInstance() {Stages = {new Stage {Status = stageStatus}}}}};

            // Act/Assert
            Assert.That(pipeline.Status, Is.EqualTo(pipelineStatus));
        }

        [TestCase(StageStatus.Building,    StageStatus.Building,  PipelineStatus.Building)]
        [TestCase(StageStatus.Building,    StageStatus.Failed,    PipelineStatus.Building)]
        [TestCase(StageStatus.Building,    StageStatus.Cancelled, PipelineStatus.Building)]
        [TestCase(StageStatus.Building,    StageStatus.Passed,    PipelineStatus.Building)]
        [TestCase(StageStatus.Building,    StageStatus.Unknown,   PipelineStatus.Building)]
                                           
        [TestCase(StageStatus.Failed,      StageStatus.Building,  PipelineStatus.Building)]
        [TestCase(StageStatus.Failed,      StageStatus.Failed,    PipelineStatus.Failed)]
        [TestCase(StageStatus.Failed,      StageStatus.Cancelled, PipelineStatus.Failed)]
        [TestCase(StageStatus.Failed,      StageStatus.Passed,    PipelineStatus.Failed)]
        [TestCase(StageStatus.Failed,      StageStatus.Unknown,   PipelineStatus.Failed)]

        [TestCase(StageStatus.Cancelled,   StageStatus.Building,  PipelineStatus.Building)]
        [TestCase(StageStatus.Cancelled,   StageStatus.Failed,    PipelineStatus.Failed)]
        [TestCase(StageStatus.Cancelled,   StageStatus.Cancelled, PipelineStatus.Failed)]
        [TestCase(StageStatus.Cancelled,   StageStatus.Passed,    PipelineStatus.Failed)]
        [TestCase(StageStatus.Cancelled,   StageStatus.Unknown,   PipelineStatus.Failed)]

        [TestCase(StageStatus.Passed,      StageStatus.Building,  PipelineStatus.Building)]
        [TestCase(StageStatus.Passed,      StageStatus.Failed,    PipelineStatus.Failed)]
        [TestCase(StageStatus.Passed,      StageStatus.Cancelled, PipelineStatus.Failed)]
        [TestCase(StageStatus.Passed,      StageStatus.Passed,    PipelineStatus.Passed)]
        [TestCase(StageStatus.Passed,      StageStatus.Unknown,   PipelineStatus.Passed)]

        [TestCase(StageStatus.Unknown,     StageStatus.Building,  PipelineStatus.Building)]
        [TestCase(StageStatus.Unknown,     StageStatus.Failed,    PipelineStatus.Failed)]
        [TestCase(StageStatus.Unknown,     StageStatus.Cancelled, PipelineStatus.Failed)]
        [TestCase(StageStatus.Unknown,     StageStatus.Passed,    PipelineStatus.Passed)]
        [TestCase(StageStatus.Unknown,     StageStatus.Unknown,   PipelineStatus.Passed)]

        public void PipelineStatus_OneInstance_TwoStages_BuildingBeatsEverything_FailedBeatsPassed(StageStatus stageStatus1, StageStatus stageStatus2, PipelineStatus pipelineStatus)
        {
            // Arrange
            var pipeline = new Pipeline() {PipelineInstances = {new PipelineInstance() {Stages = {new Stage {Status = stageStatus1}, new Stage {Status = stageStatus2}}}}};

            // Act/Assert
            Assert.That(pipeline.Status, Is.EqualTo(pipelineStatus));
        }

        [TestCase(StageStatus.Building,    StageStatus.Building,  PipelineStatus.Building)]
        [TestCase(StageStatus.Building,    StageStatus.Failed,    PipelineStatus.Building)]
        [TestCase(StageStatus.Building,    StageStatus.Cancelled, PipelineStatus.Building)]
        [TestCase(StageStatus.Building,    StageStatus.Passed,    PipelineStatus.Building)]
        [TestCase(StageStatus.Building,    StageStatus.Unknown,   PipelineStatus.Building)]
                                           
        [TestCase(StageStatus.Failed,      StageStatus.Building,  PipelineStatus.Building)]
        [TestCase(StageStatus.Failed,      StageStatus.Failed,    PipelineStatus.Failed)]
        [TestCase(StageStatus.Failed,      StageStatus.Cancelled, PipelineStatus.Failed)]
        [TestCase(StageStatus.Failed,      StageStatus.Passed,    PipelineStatus.Failed)]
        [TestCase(StageStatus.Failed,      StageStatus.Unknown,   PipelineStatus.Failed)]

        [TestCase(StageStatus.Cancelled,   StageStatus.Building,  PipelineStatus.Building)]
        [TestCase(StageStatus.Cancelled,   StageStatus.Failed,    PipelineStatus.Failed)]
        [TestCase(StageStatus.Cancelled,   StageStatus.Cancelled, PipelineStatus.Failed)]
        [TestCase(StageStatus.Cancelled,   StageStatus.Passed,    PipelineStatus.Failed)]
        [TestCase(StageStatus.Cancelled,   StageStatus.Unknown,   PipelineStatus.Failed)]

        [TestCase(StageStatus.Passed,      StageStatus.Building,  PipelineStatus.Building)]
        [TestCase(StageStatus.Passed,      StageStatus.Failed,    PipelineStatus.Failed)]
        [TestCase(StageStatus.Passed,      StageStatus.Cancelled, PipelineStatus.Failed)]
        [TestCase(StageStatus.Passed,      StageStatus.Passed,    PipelineStatus.Passed)]
        [TestCase(StageStatus.Passed,      StageStatus.Unknown,   PipelineStatus.Passed)]

        [TestCase(StageStatus.Unknown,     StageStatus.Building,  PipelineStatus.Building)]
        [TestCase(StageStatus.Unknown,     StageStatus.Failed,    PipelineStatus.Failed)]
        [TestCase(StageStatus.Unknown,     StageStatus.Cancelled, PipelineStatus.Failed)]
        [TestCase(StageStatus.Unknown,     StageStatus.Passed,    PipelineStatus.Passed)]
        [TestCase(StageStatus.Unknown,     StageStatus.Unknown,   PipelineStatus.Passed)]

        public void PipelineStatus_TwoInstances_OneStages_BuildingBeatsEverything_FailedBeatsPassed(StageStatus stageStatus1, StageStatus stageStatus2, PipelineStatus pipelineStatus)
        {
            // Arrange
            var pipeline = new Pipeline() {PipelineInstances = {new PipelineInstance() {Stages = {new Stage {Status = stageStatus1}}}, new PipelineInstance() {Stages = {new Stage {Status = stageStatus2}}}}};

            // Act/Assert
            Assert.That(pipeline.Status, Is.EqualTo(pipelineStatus));
        }
    }
}