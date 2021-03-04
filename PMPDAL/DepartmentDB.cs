using PMPDAL.Entities;
using PMPDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PMPDAL
{
    public class DepartmentDB
    {
        private static DepartmentDB instance = null;

        public static DepartmentDB GetInstance()
        {
            if (instance == null)
                instance = new DepartmentDB();
            return instance;
        }

        public Department SaveDepartment(int _id, string _name, int _headOfDepartmentId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    if (_id == 0)
                    {
                        var Department = new Department()
                        {
                            Id = _id,
                            DepartmentName = _name,
                            Status = 1,
                            HeadOfDepartmentId = _headOfDepartmentId
                        };

                        context.Department.Add(Department);

                        int numberOfInserted = context.SaveChanges();

                        return numberOfInserted > 0 ? Department : null;
                    }
                    else
                    {
                        var Department = context.Department.FirstOrDefault(x => x.Id == _id);
                        Department.Id = _id;
                        Department.DepartmentName = _name;
                        Department.Status = 1;
                        Department.HeadOfDepartmentId = _headOfDepartmentId;

                        int numberOfUpdated = context.SaveChanges();
                        return numberOfUpdated > 0 ? Department : null;
                    }
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public List<DepartmentRepo> GetAllDepartment()
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var returnList = new List<DepartmentRepo>();

                    var list = context.Department.Where(x => x.Status > 0).ToList();


                    foreach (var item in list)
                    {
                        var head = context.Person.FirstOrDefault(x => x.Id == item.HeadOfDepartmentId);
                        returnList.Add(new DepartmentRepo()
                        {
                            HeadOfDepartmentId = item.HeadOfDepartmentId,
                            DepartmentName = item.DepartmentName,
                            Id = item.Id,
                            Status = item.Status,
                            HeadOfDepartmentName = head != null ? $"{head.Name} {head.Surname}" : "Bilinmiyor"
                        });
                    }

                    return returnList;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public Department GetDepartmentById(int _id)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var Department = context.Department.FirstOrDefault(x => x.Id == _id);
                    return Department;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public bool DeleteDepartment(int _deptId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var department = context.Department.FirstOrDefault(x => x.Id == _deptId);
                    department.Status = 0;
                    int numberOfUpdated = context.SaveChanges();
                    return numberOfUpdated > 0;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}
