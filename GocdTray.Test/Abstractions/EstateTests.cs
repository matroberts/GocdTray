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

        [TestCase(StageStatus.Building,  StageStatus.Building,  EstateStatus.Building)]
        [TestCase(StageStatus.Building,  StageStatus.Failed,    EstateStatus.Building)]
        [TestCase(StageStatus.Building,  StageStatus.Cancelled, EstateStatus.Building)]
        [TestCase(StageStatus.Building,  StageStatus.Passed,    EstateStatus.Building)]
        [TestCase(StageStatus.Building,  StageStatus.Unknown,   EstateStatus.Building)]
                                                                
        [TestCase(StageStatus.Failed,    StageStatus.Building,  EstateStatus.Building)]
        [TestCase(StageStatus.Failed,    StageStatus.Failed,    EstateStatus.Failed)]
        [TestCase(StageStatus.Failed,    StageStatus.Cancelled, EstateStatus.Failed)]
        [TestCase(StageStatus.Failed,    StageStatus.Passed,    EstateStatus.Failed)]
        [TestCase(StageStatus.Failed,    StageStatus.Unknown,   EstateStatus.Failed)]
                                                                
        [TestCase(StageStatus.Cancelled, StageStatus.Building,  EstateStatus.Building)]
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

        public void IfTheResultIsValid_StateIsCalculatedFromPipelineStates_BuildingBeatsEverything_FailedBeatsPassed(StageStatus stageStatus1, StageStatus stageStatus2, EstateStatus estateStatus)
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
    }
}