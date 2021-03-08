using System;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WithOutEntityFramework.Models;
using System.Data.Odbc;
using Microsoft.Data.SqlClient;

namespace WithOutEntityFramework.Controllers
{
    public class MovieController : Controller
    {
        private readonly IConfiguration _configuration;

        public MovieController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        // GET: Movie
        public IActionResult Index()
        {
            
            DataTable dtb2 = new DataTable();


            using (OdbcConnection connection = new OdbcConnection(_configuration.GetConnectionString("OdbcCon")))
            {
                connection.Open();
                OdbcDataAdapter dat = new OdbcDataAdapter("MovieViewAll", connection);
                dat.SelectCommand.CommandType = CommandType.StoredProcedure;
                dat.Fill(dtb2);
            }

            /*  USING SQLCONNECTION
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("MovieViewAll", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.Fill(dtbl);
            }
            */
            return View(dtb2);
        }


        // GET: Movie/AddOrEdit/ 
        public IActionResult AddOrEdit(int? id)
        {
            MovieViewModel movieViewModel = new MovieViewModel();
            if (id > 0)
                movieViewModel = FetchMoviesByID(id);
            return View(movieViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit(int id, [Bind("MovieId,Title,Director")] MovieViewModel movieViewModel)
        {
            if (ModelState.IsValid)
            {

                using (OdbcConnection connection = new OdbcConnection(_configuration.GetConnectionString("OdbcCon")))
                {
                    connection.Open();
                    OdbcCommand ODBCCommand = new OdbcCommand("{call MovieAddOrEdit (?,?,?)}", connection);
                    ODBCCommand.CommandType = CommandType.StoredProcedure;
                    ODBCCommand.Parameters.AddWithValue("@MovieId", movieViewModel.MovieId);
                    ODBCCommand.Parameters.AddWithValue("@Title", movieViewModel.Title);
                    ODBCCommand.Parameters.AddWithValue("@Director", movieViewModel.Director);
                    ODBCCommand.ExecuteNonQuery();
                }

                /*
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCmd = new SqlCommand("MovieAddOrEdit", sqlConnection);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("MovieId", movieViewModel.MovieId);
                    sqlCmd.Parameters.AddWithValue("Title", movieViewModel.Title);
                    sqlCmd.Parameters.AddWithValue("Director", movieViewModel.Director);
                    sqlCmd.ExecuteNonQuery();
                }*/
                return RedirectToAction(nameof(Index));
            }
            return View(movieViewModel);
        }  

        // GET: Movie/Delete/5
        public IActionResult Delete(int? id)
        {
            MovieViewModel movieViewModel = FetchMoviesByID(id);
            return View(movieViewModel);
        }

        // POST: Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            OdbcCommand command = new OdbcCommand("Delete Movies WHERE MovieId = " +id);
            using (OdbcConnection connection = new OdbcConnection(_configuration.GetConnectionString("OdbcCon")))
            {
                command.Connection = connection;
                connection.Open();
                command.ExecuteNonQuery();
            }
         
            /* USING SQL CONNECTION
             * using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlCommand sqlCmd = new SqlCommand("MovieDeleteAllbyID", sqlConnection);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("MovieId", id);
                sqlCmd.ExecuteNonQuery();
            }*/
            return RedirectToAction(nameof(Index));
        }

        [NonAction]
        public MovieViewModel FetchMoviesByID(int? id)
        {
            
            MovieViewModel movieViewModel = new MovieViewModel();
            /*
            using (OdbcConnection connection = new OdbcConnection(_configuration.GetConnectionString("OdbcCon")))
            {
                DataTable dtb2 = new DataTable();
                connection.Open();
                OdbcDataAdapter dat = new OdbcDataAdapter("MovieViewAll", connection);
                dat.SelectCommand.CommandType = CommandType.StoredProcedure;
                
                OdbcCommand ODBCCommand = new OdbcCommand("{call MovieAddOrEdit (?,?,?)}", connection);
                ODBCCommand.CommandType = CommandType.StoredProcedure;
                ODBCCommand.Parameters.AddWithValue("@MovieId", movieViewModel.MovieId);

                dat.Fill(dtb2);

                if (dtb2.Rows.Count == 1)
                {

                    movieViewModel.MovieId = Convert.ToInt32(dtb2.Rows[0]["MovieId"].ToString());
                    movieViewModel.Title = dtb2.Rows[0]["Title"].ToString();
                    movieViewModel.Director = dtb2.Rows[0]["Director"].ToString();
                }
                return movieViewModel;

            }*/

              using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                DataTable dtbl = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("MovieViewAllbyID", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("MovieId", id);
                sqlDa.Fill(dtbl);
                if (dtbl.Rows.Count == 1)
                {

                    movieViewModel.MovieId = Convert.ToInt32(dtbl.Rows[0]["MovieId"].ToString());
                    movieViewModel.Title = dtbl.Rows[0]["Title"].ToString();
                    movieViewModel.Director = dtbl.Rows[0]["Director"].ToString();
                }
                return movieViewModel;
            }
            
        }

    }
}
