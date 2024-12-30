namespace TeisterMask.DataProcessor.ExportDto
{
    public  class ExportEmployeesDto
    {
        public string Username { get; set; } = null!;

        public List<ExportTaskDto> Tasks { get; set; }=new List<ExportTaskDto>();
    }
}
