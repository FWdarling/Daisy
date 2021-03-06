﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DaisyDBProject.Models;
using Microsoft.AspNetCore.Authorization;

namespace DaisyDBProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NoticeController : ControllerBase
    {
        private readonly DaisyContext _context;

        public NoticeController(DaisyContext context)
        {
            _context = context;
        }

        // GET: api/Notice
        [HttpGet]
        public ActionResult<IEnumerable<Notice>> GetNotice()
        {
            return _context.Notice.ToList();
        }

        // POST: api/Notice
        [HttpPost]
        public ActionResult<Notice> PostNotice(Notice notice)
        {
            _context.Notice.Add(notice);
            try {
                _context.SaveChanges();
            }
            catch (DbUpdateException) {
                throw;
            }

            return CreatedAtAction("GetNotice", new { id = notice.NoticeId }, notice);
        }

    }
}
