﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentsWebsite.Data.Entities;

namespace StudentsWebsite.WebUI.Models
{
    public class PossibleRatingViewModel
    {
        public string LecturerName { get; set; }
        public string LecturerFullName { get; set; }
        public string StudentName { get; set; }
        public string StudentFullName { get; set; }
        public int Rating { get; set; }
        public bool Selected { get; set; }

        public PossibleRatingViewModel(DbUser student, DbUser lecturer, int rating, bool selected)
        {
            StudentName = student.Email;
            StudentFullName = student.FirstName + " " + student.LastName;
            LecturerName = lecturer.Email;
            LecturerFullName = lecturer.FirstName + " " + lecturer.LastName;
            Rating = rating;
            Selected = selected;
        }
    }
}