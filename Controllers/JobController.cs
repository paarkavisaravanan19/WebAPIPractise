using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;
using WebAPIPractice.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPIPractice.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class JobController : ControllerBase
	{
		// GET: api/<JobController>
		private readonly IConfiguration _configuration;
		public JobController(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		private SqlConnection _connection;
		private List<JobModel> _JobList = new List<JobModel>();
		[HttpGet]
		public IActionResult Get()
		{
			try
			{
				string conn = _configuration.GetConnectionString("JobDB");
				_connection = new SqlConnection(conn);
				_connection.Open();
				string selectQuery = "SELECT * FROM JobDetailsTable";
				using (SqlCommand cmd = new SqlCommand(selectQuery, _connection))
				{
					SqlDataReader reader = cmd.ExecuteReader();
					while (reader.Read())
					{
						JobModel model = new JobModel();
						model.JobID = (int)reader[0];
						model.JobTitle = (string)reader[1];
						model.salary = (decimal)reader[2];
						model.JobDescription = (string)reader[3];
						model.CompanyName = (string)reader[4];
						_JobList.Add(model);
					}

					_connection.Close();
					return Ok(_JobList);
				}

			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return NotFound();
		}


		// GET api/<JobController>/5
		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			try
			{
				string conn = _configuration.GetConnectionString("JobDB");
				_connection = new SqlConnection(conn);
				_connection.Open();
				string selectQuery = $"SELECT * FROM JobDetailsTable where JobID ={id}";
				using (SqlCommand cmd = new SqlCommand(selectQuery, _connection))
				{
					JobModel model = new JobModel();
					SqlDataReader reader = cmd.ExecuteReader();
					while (reader.Read())
					{
						model.JobID = (int)reader[0];
						model.JobTitle = (string)reader[1];
						model.salary = (decimal)reader[2];
						model.JobDescription = (string)reader[3];
						model.CompanyName = (string)reader[4];

					}

					_connection.Close();
					return Ok(model);
				}

			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return NotFound();
		}

		// POST api/<JobController>
		[HttpPost]
		public IActionResult Post([FromBody] JobModel model)
		{
			try
			{
				string conn = _configuration.GetConnectionString("JobDB");
				_connection = new SqlConnection(conn);
				_connection.Open();
				SqlCommand command = new SqlCommand("Insertion", _connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.Add("@JobTitle", SqlDbType.Text).Value = model.JobTitle;
				command.Parameters.Add("@Salary", SqlDbType.Decimal).Value = model.salary;
				command.Parameters.Add("@JobDescription", SqlDbType.Text).Value = model.JobDescription;
				command.Parameters.Add("@CompanyName", SqlDbType.Text).Value = model.CompanyName;

				command.ExecuteNonQuery();
				_connection.Close();
				return Ok(new
				{
					status = true,
					message = "new row created"
				});

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return BadRequest(ex.Message);
			}
		}

		// PUT api/<JobController>/5
		[HttpPut("{id}")]
		public IActionResult Put(int id, [FromBody] JobModel model)
		{
			try
			{
				string conn = _configuration.GetConnectionString("JobDB");
				_connection = new SqlConnection(conn);
				_connection.Open();
				SqlCommand command = new SqlCommand("Updation", _connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.Add("@JobID", SqlDbType.Int).Value = model.JobID;
				command.Parameters.Add("@JobTitle", SqlDbType.Text).Value = model.JobTitle;
				command.Parameters.Add("@Salary", SqlDbType.Decimal).Value = model.salary;
				command.Parameters.Add("@JobDescription", SqlDbType.Text).Value = model.JobDescription;
				command.Parameters.Add("@CompanyName", SqlDbType.Text).Value = model.CompanyName;
				command.ExecuteNonQuery();
				_connection.Close();
				return Ok(new
				{
					status = true,
					message = "1 row(s) updated"
				});

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return BadRequest(ex.Message);
			}
		}

		// DELETE api/<JobController>/5
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{

			try
			{
				string conn = _configuration.GetConnectionString("JobDB");
				_connection = new SqlConnection(conn);
				_connection.Open();
				string deleteQuery = $"delete from dbo.JobDetailsTable where JobID={id};";
				using (SqlCommand cmd = new SqlCommand(deleteQuery, _connection))
				{
					cmd.ExecuteNonQuery();
					_connection.Close();
					return Ok(new
					{
						status = true,
						message = "1 row(s) deleted"
					});

				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return BadRequest(ex.Message);
			}


		}
	}
}
			

