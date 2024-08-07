﻿using AutoMapper;
using ConstructionApplication.DataModels.AttendanceDetails;
using ConstructionApplication.DataModels.CostMaster;
using ConstructionApplication.Repositories;
using ConstructionApplication.ViewModels.AttendanceDetailsVm;
using ConstructionApplication.ViewModels.CostMasterVm;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionApplication.Controllers
{
    public class AttendanceDetailsController : Controller
    {
        IAttendanceDetailsRepository _attendanceDetailsRepository;
        IMapper _imapper;

        public AttendanceDetailsController(IAttendanceDetailsRepository attendanceDetailsRepository)
        {
            _attendanceDetailsRepository = attendanceDetailsRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
            });

            _imapper = configuration.CreateMapper();
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AttendanceDetailsVm attendanceDetailsVm)
        {
            attendanceDetailsVm.AttendanceId = Convert.ToInt32(HttpContext.Session.GetInt32("AttendanceId"));

            AttendanceDetails attendanceDetails = _imapper.Map<AttendanceDetailsVm, AttendanceDetails>(attendanceDetailsVm);
            int affectedRowCount = _attendanceDetailsRepository.Create(attendanceDetails);
            if (affectedRowCount > 0)
            {
                ViewBag.successMessage = "Add New Attendance Details Successful";
            }
            return View();
        }
    }
}
