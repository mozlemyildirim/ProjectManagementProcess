using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PMPDAL;

namespace PMPCore.Controllers
{
    public class PersonController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
        public JsonResult GetAllPerson()
        {
            try
            {
                var list = PersonDB.GetInstance().GetAllPerson();
                return Json(list);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult GetAllDepartment()
        {
            try
            {
                var list = DepartmentDB.GetInstance().GetAllDepartment();
                return Json(list);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult GetAllTeam()
        {
            try
            {
                var list = TeamDB.GetInstance().GetAllTeam();
                return Json(list);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult SavePerson(int _id, string _name, string _surname, string _mail, string _password, int _teamId, bool _isAdmin)
        {
            try
            {
                var result = PersonDB.GetInstance().SavePerson(_id, _name, _surname, _mail, _password, _teamId, _isAdmin);
                return Json(result != null);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult GetPersonById(int _personId)
        {
            try
            {
                var result = PersonDB.GetInstance().GetPersonById(_personId);
                return Json(result);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult GetDepartmentById(int _deptId)
        {
            try
            {
                var result = DepartmentDB.GetInstance().GetDepartmentById(_deptId);
                return Json(result);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult DeletePerson(int _personId)
        {
            try
            {
                var result = PersonDB.GetInstance().DeletePerson(_personId);
                return Json(result);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult DeleteDepartment(int _deptId)
        {
            try
            {
                var result = DepartmentDB.GetInstance().DeleteDepartment(_deptId);
                return Json(result);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        // TEAM 


        public JsonResult SaveTeam(int _id, string _name, int _departmentId)
        {
            try
            {
                var result = TeamDB.GetInstance().SaveTeam(_id, _name, _departmentId);
                return Json(result != null);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult SaveDepartman(int _id, string _name, int _headOfDepartment)
        {
            try
            {
                var result = DepartmentDB.GetInstance().SaveDepartment(_id, _name, _headOfDepartment);
                return Json(result != null);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult GetTeamById(int _teamId)
        {
            try
            {
                var result = TeamDB.GetInstance().GetTeamById(_teamId);
                return Json(result);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }


        public JsonResult DeleteTeam(int _teamId)
        {
            try
            {
                var result = TeamDB.GetInstance().DeleteTeam(_teamId);
                return Json(result);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }


        public JsonResult GetAllTeamSelect()
        {
            try
            {
                var list = TeamDB.GetInstance().GetAllTeamSelect();
                return Json(list);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}