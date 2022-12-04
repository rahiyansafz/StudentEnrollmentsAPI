﻿using StudentEnrollment.Data.Models.Course;
using StudentEnrollment.Data.Models.Student;

namespace StudentEnrollment.Data.Models.Enrollment;
public class EnrollmentDto
{
    public int CourseId { get; set; }
    public int StudentId { get; set; }
    public virtual CourseDto? Course { get; set; }
    public virtual StudentDto? Student { get; set; }
}