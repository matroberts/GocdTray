using System.Collections.Generic;
using GocdTray.App.Abstractions;
using NUnit.Framework;

namespace GocdTray.Test.Abstractions
{
    [TestFixture]
    public class EstateTests
    {
        [Test]
        public void IfTheResultIsInvalid_TheTheEstateIsNotConnected_AndThereAreNoPipelines()
        {
            // Arrange
            var result = Result<List<Pipeline>>.Invalid("error");

            // Act
            var estate = new Estate(result);

            // Assert
            Assert.That(estate.Status, Is.EqualTo(EstateStatus.NotConnected));
            Assert.That(estate.Pipelines, Is.Empty);
        }

        [Test]
        public void IfTheResultIsValid_ButThereAreNoPipelines_TheEstateIsPassed()
        {
            // Arrange
            var result = Result<List<Pipeline>>.Valid(new List<Pipeline>());

            // Act
            var estate = new Estate(result);

            // Assert
            Assert.That(estate.Status, Is.EqualTo(EstateStatus.Passed));
            Assert.That(estate.Pipelines, Is.Empty);
        }

        [TestCase(StageStatus.Building,  StageStatus.Building,  EstateStatus.Building)]
        [TestCase(StageStatus.Building,  StageStatus.Failed,    EstateStatus.Failed)]
        [TestCase(StageStatus.Building,  StageStatus.Cancelled, EstateStatus.Failed)]
        [TestCase(StageStatus.Building,  StageStatus.Passed,    EstateStatus.Building)]
        [TestCase(StageStatus.Building,  StageStatus.Unknown,   EstateStatus.Building)]
                                                                
        [TestCase(StageStatus.Failed,    StageStatus.Building,  EstateStatus.Failed)]
        [TestCase(StageStatus.Failed,    StageStatus.Failed,    EstateStatus.Failed)]
        [TestCase(StageStatus.Failed,    StageStatus.Cancelled, EstateStatus.Failed)]
        [TestCase(StageStatus.Failed,    StageStatus.Passed,    EstateStatus.Failed)]
        [TestCase(StageStatus.Failed,    StageStatus.Unknown,   EstateStatus.Failed)]
                                                                
        [TestCase(StageStatus.Cancelled, StageStatus.Building,  EstateStatus.Failed)]
        [TestCase(StageStatus.Cancelled, StageStatus.Failed,    EstateStatus.Failed)]
        [TestCase(StageStatus.Cancelled, StageStatus.Cancelled, EstateStatus.Failed)]
        [TestCase(StageStatus.Cancelled, StageStatus.Passed,    EstateStatus.Failed)]
        [TestCase(StageStatus.Cancelled, StageStatus.Unknown,   EstateStatus.Failed)]
                                                                
        [TestCase(StageStatus.Passed,    StageStatus.Building,  EstateStatus.Building)]
        [TestCase(StageStatus.Passed,    StageStatus.Failed,    EstateStatus.Failed)]
        [TestCase(StageStatus.Passed,    StageStatus.Cancelled, EstateStatus.Failed)]
        [TestCase(StageStatus.Passed,    StageStatus.Passed,    EstateStatus.Passed)]
        [TestCase(StageStatus.Passed,    StageStatus.Unknown,   EstateStatus.Passed)]
                                                                
        [TestCase(StageStatus.Unknown,   StageStatus.Building,  EstateStatus.Building)]
        [TestCase(StageStatus.Unknown,   StageStatus.Failed,    EstateStatus.Failed)]
        [TestCase(StageStatus.Unknown,   StageStatus.Cancelled, EstateStatus.Failed)]
        [TestCase(StageStatus.Unknown,   StageStatus.Passed,    EstateStatus.Passed)]
        [TestCase(StageStatus.Unknown,   StageStatus.Unknown,   EstateStatus.Passed)]

        public void IfTheResultIsValid_StateIsCalculatedFromPipelineStates_FailedBeatsEverything_BuildingBeatsPassed(StageStatus stageStatus1, StageStatus stageStatus2, EstateStatus estateStatus)
        {
            // Arrange
            var pipeline1 = new Pipeline() {PipelineInstances = {new PipelineInstance() {Stages = {new Stage() {Status = stageStatus1}}}}};
            var pipeline2 = new Pipeline() {PipelineInstances = {new PipelineInstance() {Stages = {new Stage() {Status = stageStatus2}}}}};
            var result = Result<List<Pipeline>>.Valid(new List<Pipeline>{pipeline1, pipeline2 });

            // Act
            var estate = new Estate(result);
            //Assert
            Assert.That(estate.Status, Is.EqualTo(estateStatus));
        }

        [TestCase(StageStatus.Building,  false, EstateStatus.Building)]
        [TestCase(StageStatus.Failed,    false, EstateStatus.Failed)]
        [TestCase(StageStatus.Cancelled, false, EstateStatus.Failed)]
        [TestCase(StageStatus.Passed,    false, EstateStatus.Passed)]
        [TestCase(StageStatus.Unknown,   false, EstateStatus.Passed)]

        [TestCase(StageStatus.Building,  true, EstateStatus.Passed)]
        [TestCase(StageStatus.Failed,    true, EstateStatus.Passed)]
        [TestCase(StageStatus.Cancelled, true, EstateStatus.Passed)]
        [TestCase(StageStatus.Passed,    true, EstateStatus.Passed)]
        [TestCase(StageStatus.Unknown,   true, EstateStatus.Passed)]
        public void PausedPipelines_AreNotIncludedInTheGlobalStatusCalculation(StageStatus stageStatus, bool isPaused, EstateStatus estateStatus)
        {
            // Arrange
            var pipeline = new Pipeline() { Paused = isPaused, PipelineInstances = { new PipelineInstance() { Stages = { new Stage() { Status = stageStatus } } } } };
            var result = Result<List<Pipeline>>.Valid(new List<Pipeline> { pipeline });

            // Act
            var estate = new Estate(result);
            //Assert
            Assert.That(estate.Status, Is.EqualTo(estateStatus));
        }
    }
}