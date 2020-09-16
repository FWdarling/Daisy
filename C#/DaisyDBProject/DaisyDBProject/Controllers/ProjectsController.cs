﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DaisyDBProject.Models;
using Microsoft.CodeAnalysis.Operations;

namespace DaisyDBProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly DaisyContext _context;

        public ProjectsController(DaisyContext context)
        {
            _context = context;
        }
        // GET: api/Projects
        [HttpGet]
        public ActionResult<IEnumerable<Project>> GetProject()
        {
            var query = from project in _context.Set<Project>()
                        select project;
            return query.OrderBy(q => q.StartTime).ToList();
        }
        // GET: api/Projects/5
        [HttpGet,Route("search")]
        public ActionResult<IEnumerable<Project>> SearchProject(string name)
        {
            var query = from project in _context.Set<Project>()
                        where project.Name == name
                        select project;
            return query.ToList();
        }
        [HttpGet("{id}")]
        public ActionResult<Project> GetProject(int id)
        {
            var project = _context.Project.Find(id);

            if (project == null)
            {
                return NotFound();
            }
            return project;
        }

        [HttpGet,Route("random")]
        public ActionResult<IEnumerable<Project>> GetRandomProjects()
        {
            var query = from project in _context.Set<Project>()
                        select project;
            var result=query.OrderBy(q=>Guid.NewGuid()).Take(15);
            return query.ToList();
        }
        // PUT: api/Projects/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public ActionResult<Project> PutProject(int id, Project project)
        {
            if (id != project.ProjectId)
            {
                return BadRequest();
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // POST: api/Projects
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult<Project> PostProject(Project project)
        {
            _context.Project.Add(project);
            try
            {
                _context.SaveChanges();
            }
            catch(DbUpdateException)
            {
                throw;
            }
            return CreatedAtAction("GetProject", new { id = project.ProjectId }, project);
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public ActionResult<Project> DeleteProject(int id)
        {
            var project = _context.Project.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Project.Remove(project);
            _context.SaveChanges();

            return project;
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.ProjectId == id);
        }
    }
}
