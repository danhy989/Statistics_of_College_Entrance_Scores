using Crawl_College_Entrance_Scores.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crawl_College_Entrance_Scores.Repository
{
	public interface IUserRepository
	{
		Task<IEnumerable<User>> GetAll();
		Task<User> GetUserAsync(string username);
		Task<User> GetUserAsync(string username,string password);
	}
	public class UserRepository : IUserRepository
	{
		private readonly EntranceScoresContext _context;
		public UserRepository(EntranceScoresContext context)
		{
			this._context = context;
		}
		public async Task<IEnumerable<User>> GetAll()
		{
			using (_context)
			{
				return await Task.Run(() => _context.users);
			}
		}

		public async Task<User> GetUserAsync(string username)
		{
			using (_context)
			{
				return await Task.Run(() => _context.users.SingleOrDefault(x => x.Username == username));
			}
		}

		public async Task<User> GetUserAsync(string username, string password)
		{
			using (_context)
			{
				return await Task.Run(() => _context.users.SingleOrDefault(x => x.Username==username && x.Password == password));
			}
		}
	}
}
