using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PMPDAL;
using PMPDAL.Entities;
using System;
using System.Collections.Generic;

namespace PMPCore.Controllers
{
    public class ProjectController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
        public JsonResult GetAllProjeler()
        {
            try
            {
                var personId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;
                var projeler = ProjectDB.GetInstance().GetAllProject(personId);

                return Json(projeler);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }


        public JsonResult GetKullaniciById(int _kullaniciId)
        {
            try
            {
                var kullanici = PersonDB.GetInstance().GetPersonById(_kullaniciId);
                var returnObj = kullanici.Name + " " + kullanici.Surname;

                return Json(returnObj);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }


        public JsonResult DeleteProje(int _projeId)
        {
            try
            {
                var result = ProjectDB.GetInstance().DeleteProje(_projeId);

                if (result)
                {
                    if (HttpContext.Session.GetString("SelectedProject") != null)
                    {
                        if (_projeId == JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Id)
                            HttpContext.Session.SetString("SelectedProject", "");
                    }
                }


                return Json(result);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }


        public JsonResult GetProjeById(int _projeId)
        {
            try
            {
                var proje = ProjectDB.GetInstance().GetProjeById(_projeId);

                return Json(proje);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult GetProjectPersonsByProjectId(int _projectId)
        {
            try
            {
                var list = ProjectDB.GetInstance().GetAllProjectPersonsByProjectId(_projectId);
                return Json(list);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult SaveProject(string _titleProject, string _code, string _shortname, string _description, int _personId, int _projectId, List<int> _projeKisi)
        {
            try
            {
                if (_projectId == 0)
                {
                    var project = new Project()
                    {
                        Name = _titleProject,
                        Code = _code,
                        ShortName = _shortname,
                        Description = _description,
                        ProjectLeader = _personId,
                        Status = 1
                    };


                    var result = ProjectDB.GetInstance().SaveProject(project);

                    var currentPersonId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;

                    if (!_projeKisi.Contains(currentPersonId))
                        _projeKisi.Add(currentPersonId);

                    for (int i = 0; i < _projeKisi.Count; i++)
                    {
                        var projeKisi = new ProjectPerson()
                        {
                            ProjectId = result.Id,
                            PersonId = _projeKisi[i]
                        };

                        var ppResult = ProjectDB.GetInstance().SaveProjectPerson(projeKisi);
                    }

                    Board b = new Board()
                    {
                        Name = "YAPILACAK",
                        ProjectId = result.Id,
                        Status = 1
                    };

                    BoardDB.GetInstance().SavePano(b);

                    b = new Board()
                    {
                        Name = "YAPILIYOR",
                        ProjectId = result.Id,
                        Status = 1
                    };

                    BoardDB.GetInstance().SavePano(b);

                    b = new Board()
                    {
                        Name = "TAMAMLANDI",
                        ProjectId = result.Id,
                        Status = 1
                    };

                    BoardDB.GetInstance().SavePano(b);

                    return Json(result != null);
                }
                else
                {
                    var project = new Project()
                    {
                        Name = _titleProject,
                        Code = _code,
                        ShortName = _shortname,
                        Description = _description,
                        ProjectLeader = _personId,
                        Id = _projectId,
                        Status = 1
                    };
                    var delete = ProjectDB.GetInstance().DeleteProjectPersonByProjectId(_projectId);
                    var currentPersonId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;

                    if (!_projeKisi.Contains(currentPersonId))
                        _projeKisi.Add(currentPersonId);

                    for (int i = 0; i < _projeKisi.Count; i++)
                    {
                        var projeKisi = new ProjectPerson()
                        {
                            ProjectId = _projectId,
                            PersonId = _projeKisi[i]
                        };

                        var ppResult = ProjectDB.GetInstance().SaveProjectPerson(projeKisi);
                    }

                    var result = ProjectDB.GetInstance().UpdateProject(project);
                    return Json(result != null);
                }

            }
            catch (Exception exc)
            {

                throw exc;
            }
        }

    }
}