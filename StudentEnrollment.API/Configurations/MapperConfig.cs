﻿using AutoMapper;

namespace StudentEnrollment.API.Configurations;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<Course, CourseDto>().ReverseMap();
        CreateMap<Course, CreateCourseDto>().ReverseMap();
        CreateMap<Course, CourseDetailsDto>().ForMember(q => q.Students, x => x.MapFrom(course => course.Enrollments.Select(s => s.Student)));

        CreateMap<Student, StudentDto>().ReverseMap();
        CreateMap<Student, CreateStudentDto>().ReverseMap();
        CreateMap<Student, StudentDetailsDto>()
                .ForMember(q => q.Courses, x => x.MapFrom(stu => stu.Enrollments.Select(c => c.Course)));

        CreateMap<Enrollment, EnrollmentDto>().ReverseMap();
        CreateMap<Enrollment, CreateEnrollmentDto>().ReverseMap();

        //CreateMap<Register, SchoolUser>().ReverseMap();
    }
}
