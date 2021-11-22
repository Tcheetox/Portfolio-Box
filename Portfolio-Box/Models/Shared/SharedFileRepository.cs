﻿using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Portfolio_Box.Models.Shared
{
    public class SharedFileRepository : PageModel, ISharedFileRepository
    { 
        private readonly AppDBContext _appDBContext;
        private readonly User.User _user;

        public SharedFileRepository(AppDBContext appDBContext, User.User user)
        {
            _appDBContext = appDBContext;
            _user = user;
        }

        public IEnumerable<SharedFile> AllFiles => from f in _appDBContext.Files
                                                   where f.UserId == _user.Id
                                                   select f;

        public SharedFile GetSharedFileById(int id)
        {
            return (from f in _appDBContext.Files
                    where f.Id == id && f.UserId == _user.Id
                    select f)
                    .Include(c => c.Links)
                    .FirstOrDefault();
        }

        public void SaveFile(SharedFile sharedFile)
        {
            _appDBContext.Files.Add(sharedFile);
            _appDBContext.SaveChanges();
        }
    }
}
