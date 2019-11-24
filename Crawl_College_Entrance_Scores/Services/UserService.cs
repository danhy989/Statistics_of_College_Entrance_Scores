using Crawl_College_Entrance_Scores.entity;
using Crawl_College_Entrance_Scores.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawl_College_Entrance_Scores.Services
{
	public interface IUserService
	{
		Task<String> Authenticate(string username, string password);
		Task<IEnumerable<User>> GetAll();

		Task<User> GetUserAsync(string username);
	}
	public class UserService : IUserService
	{
		// users hardcoded for simplicity, store in a db with hashed passwords in production applications
		//private List<User> _users = new List<User>
		//{
		//	new User { Id = 1, FirstName = "Test", LastName = "User", Username = "test", Password = "test" },
		//	new User { Id = 2, FirstName = "admin", LastName = "admin", Username = "admin", Password = "admin" }
		//};

		private readonly IUserRepository _userRepository;

		public UserService(IUserRepository userRepository)
		{
			this._userRepository = userRepository;
		}

		public async Task<String> Authenticate(string username, string password)
		{
			var user = await Task.Run(() => this._userRepository.GetUserAsync(username, password));

			// return null if user not found
			if (user == null)
				return null;

			string svcCredentials = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));

			return svcCredentials;
		}

		public async Task<User> GetUserAsync(string username)
		{
			var user = await Task.Run(() => _userRepository.GetUserAsync(username));
			if (user == null)
				return null;
			return user;
		}

		public async Task<IEnumerable<User>> GetAll()
		{
			// return users without passwords
			IEnumerable<User> users = await Task.Run(() => _userRepository.GetAll());
			return users.Select(x => {
				x.Password = null;
				return x;
			});
		}
	}
}
