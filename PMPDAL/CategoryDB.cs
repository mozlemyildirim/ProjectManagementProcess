using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PMPDAL.Entities;
using PMPDAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using Category = PMPDAL.Entities.Category;

namespace PMPDAL
{
    public class CategoryDB
    {
        private static CategoryDB instance = null;

        public static CategoryDB GetInstance()
        {
            if (instance == null)
                instance = new CategoryDB();
            return instance;
        }
        public List<Category> GetCategoryById(int _CategoryId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var returnList = new List<Category>();
                    var list = context.Category.FirstOrDefault(x => x.Status > 0 && x.Id == _CategoryId);
                        returnList.Add(new Category()
                        {
                            Name = list.Name,
                            Id = list.Id,
                            ProjectId = list.ProjectId
                        });
                    return returnList;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public List<Category> GetAllCategoryByProjectId(int _projectId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var returnList = new List<Category>();
                    var list = context.Category.Where(x => x.Status > 0 && x.ProjectId == _projectId).ToList();
                    foreach(var item in list)
                    {
                        returnList.Add(new Category()
                        {
                            Name = item.Name,
                            Id = item.Id,
                            ProjectId = item.ProjectId
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
        public List<Category> GetAllCategory()
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var returnList = new List<Category>();
                    var list = context.Category.Where(x => x.Status > 0 ).ToList();
                    foreach (var item in list)
                    {
                        returnList.Add(new Category()
                        {
                            Name = item.Name,
                            Id = item.Id,
                            ProjectId = item.ProjectId
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
        public Category SaveCategory(Category _Category)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    context.Category.Add(_Category);
                    int numberOfInserted = context.SaveChanges();
                    return numberOfInserted > 0 ? _Category : null;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public Category UpdateCategory(Category _s)
        {
            try
            {
                var numberOfUpdated = 0;
                using (var context = new ProjectManagementEntities())
                {
                    var Category = context.Category.FirstOrDefault(x => x.Id == _s.Id);

                    if (Category != null)
                    {
                        Category.Name = _s.Name;
                        Category.ProjectId = _s.ProjectId;
                        Category.Status = _s.Status;
                        numberOfUpdated = context.SaveChanges();
                        return numberOfUpdated > 0 ? _s : null;
                    }
                    return null;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public bool DeleteCategory(int _CategoryId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var Category = context.Category.FirstOrDefault(x => x.Id == _CategoryId);

                    if (Category != null)
                    {
                        Category.Status = 0;
                        int numberOfDeleted = context.SaveChanges();

                        return numberOfDeleted > 0;
                    }

                }
                return false;

            }
            catch (System.Exception exc)
            {

                throw exc;
            }
        }
    }
}

