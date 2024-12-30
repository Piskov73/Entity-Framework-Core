namespace TeisterMask.DataProcessor.ExportDto
{
    public class ExportTaskDto
    {
        //"TaskName": "Pointed Gourd",
        public string TaskName { get; set; } = null!;

        //"OpenDate": "10/08/2018",
        public string OpenDate { get; set; } = null!;

        //"DueDate": "10/24/2019",
        public string DueDate { get; set; } = null!;

        //"LabelType": "Priority",
        public string LabelType { get; set; } = null!;

        //"ExecutionType": "ProductBacklog"
        public string ExecutionType { get; set; } = null!;

    }
}