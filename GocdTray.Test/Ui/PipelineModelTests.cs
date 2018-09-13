using System;
using System.Linq;
using GocdTray.App;
using GocdTray.App.Abstractions;
using GocdTray.Test.App;
using GocdTray.Ui.Code;
using GocdTray.Ui.ViewModel;
using NUnit.Framework;

namespace GocdTray.Test.Ui
{
    [TestFixture]
    public class PipelineModelTests
    {
        [TestCase("http://gocdserver/", "http://gocdserver/go/pipelines/Pipeline/23/Stage/3")]
        [TestCase("http://gocdserver", "http://gocdserver/go/pipelines/Pipeline/23/Stage/3")]
        public void PipelineViewModel_ShouldFormTheWebsiteUrlCorrectly_IndependentlyOfTrailingSlashes(string baseUri, string result)
        {
            // Arrange
            GocdTray.App.Properties.Settings.Default.GocdWebUri = baseUri;
            var pipelineViewModel = new PipelineViewModel(new ServiceManager(new GocdServiceFactoryFake()));

            // Act
            var pipeline = new Pipeline { Name = "Pipeline", PipelineInstances = { new PipelineInstance() { Label = "23", Stages = { new Stage() { Name = "Stage", Run = 3 } } } } };
            var websiteUrl = pipelineViewModel.GetPipelineUrl(new UiPipeline(pipeline));

            // Assert
            Assert.That(websiteUrl, Is.EqualTo(result));
        }

        // a "base" url of http://gocdserver/subdomain produces a result of http://gocdserver/go/pipelines/Pipeline/23/Stage/3 but decided this was fine
    }
}