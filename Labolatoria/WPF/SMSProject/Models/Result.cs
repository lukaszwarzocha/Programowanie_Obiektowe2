using System;

public class Result
{
    public int ResultId { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public decimal Grade { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;

    public Student Student { get; set; } = new();
    public Course Course { get; set; } = new();
}
