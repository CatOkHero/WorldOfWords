using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WorldOfWords.API.Models;
using WorldOfWords.Domain.Models;
using WorldOfWords.Infrastructure.Data.EF;
using System.Data.Entity.Migrations;

namespace WorldOfWords.Domain.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupMapper _mapper;

        public GroupService(IGroupMapper mapper)
        {
            _mapper = mapper;
        }

        public List<Group> GetAll(int userId)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                return context.Groups.Where(g => g.OwnerId == userId).Include(group => group.Course).ToList();
            }
        }

        public bool CheckIfGroupNameExists(GroupModel groupModel)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                var group = context.Groups.FirstOrDefault(g => g.Name == groupModel.Name);
                return group != null;
            }
        }

        public bool Add(GroupModel groupModel)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                var newGroup = _mapper.MapToGroup(groupModel);
                context.Groups.Add(newGroup);
                context.SaveChanges();
                return true;
            }
        }

        public Group GetById(int groupId, int userId)
        {
            using (var context = new WorldOfWordsDatabaseContext())
            {
                return context.Groups.Where(g => g.OwnerId == userId).Include(g => g.Course).FirstOrDefault(g => g.Id == groupId);
            }
        }

        public bool DeleteById(int groupId)
        {
            try
            {
                using (var context = new WorldOfWordsDatabaseContext())
                {
                    context.Enrollments.RemoveRange(context.Enrollments.Where(e => e.GroupId == groupId));
                    context.Groups.Remove(context.Groups.FirstOrDefault(g => g.Id == groupId));
                    context.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}