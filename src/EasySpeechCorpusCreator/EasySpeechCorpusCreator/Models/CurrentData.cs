namespace EasySpeechCorpusCreator.Models
{
    public class CurrentData
    {
        public string Project { get; set; }

        public CurrentData(string project = "")
        {
            this.Project = project;
        }
    }
}
