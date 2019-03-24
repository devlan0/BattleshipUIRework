using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;

namespace BattleshipClient
{
    public static class HttpBattleshipClient
    {
        private static readonly Uri uri = new Uri("http://79.196.243.149:8080");

        #region user login interface
            
        /// <summary>
        /// submits the user data and returns the status received from the server
        /// </summary>
        /// <param name="username">username of the user</param>
        /// <param name="email">email of the user</param>
        /// <param name="hashedPassword">hashed password of the user</param>
        /// <returns>Returns a tuple containing the token, status and message received from the server</returns>
        public async static Task<(string, string, string)> Register(string username, string email, byte[] hashedPassword)
        {

            //Local variables
            string status = "";
            string message = "";
            string token = "";

            //Create json object
            var obj = new
            {
                username,
                email,
                hashedPassword = Convert.ToBase64String(hashedPassword)
            };
            var json = new JavaScriptSerializer().Serialize(obj);

            //Post json object to server
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.PostAsync(uri + "/register", new StringContent(json, Encoding.UTF8, "application/json"));
                    response.EnsureSuccessStatusCode();
                    string server_json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Server json: " + server_json);
                    JObject jobj = JObject.Parse(server_json);
                    status = jobj.Property("status")?.Value?.ToString();
                    if (status.Equals("success"))
                    {
                        message = jobj.Property("message")?.Value?.ToString();
                        token = jobj.Property("token")?.Value?.ToString();
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Error at class 'HttpBattleshipClient' in method 'Register' ");
                    Console.WriteLine(e.Message);
                }
            }

            return (status, message, token);
        }
        
        /// <summary>
        /// submits the user data to the server and returns the token received from the server
        /// </summary>
        /// <param name="username">username of the user</param>
        /// <param name="hashedPassword">hashed password of the user</param>
        /// <returns>Returns a tuple containing the status, message and token received from the server</returns>
        public async static Task<(string, string, string)> Login(string username, byte[] hashedPassword)
        {
            string status = "";
            string message = "";
            string token = "";
            //Create json object
            var obj = new
            {
                username,
                hashedPassword = Convert.ToBase64String(hashedPassword)
            };
            var json = new JavaScriptSerializer().Serialize(obj);

            //Post json object to server
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.PostAsync(uri + "/login", new StringContent(json, Encoding.UTF8, "application/json"));
                    response.EnsureSuccessStatusCode();
                    string server_json = await response.Content.ReadAsStringAsync();
                    JObject jobj = JObject.Parse(server_json);
                    status = jobj.Property("status")?.Value?.ToString();
                    if (status.Equals("success"))
                    {
                        message = jobj.Property("message")?.Value?.ToString();
                        token = jobj.Property("token")?.Value?.ToString();
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Error at class 'HttpBattleshipClient' in method 'Login' ");
                    Console.WriteLine(e.Message);
                }
            }

            return (status, message, token);
        }

        #endregion

        #region client server communication

        /// <summary>
        /// places the user with the username in a match queue
        /// </summary>
        /// <param name="username">username of the user</param>
        /// <param name="token">token assigned to the user after login</param>
        /// <returns>Returns a string tuple consisting of the status and message received from the server</returns>
        public async static Task<(string, string)> QueueMatch(string username, string token)
        {

            string status = "";
            string message = "";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("username", username);
                client.DefaultRequestHeaders.Add("token", token);

                try
                {
                    HttpResponseMessage response = await client.GetAsync(uri + "/withVal/queueMatch");
                    response.EnsureSuccessStatusCode();
                    string server_json = await response.Content.ReadAsStringAsync();
                    JObject jobj = JObject.Parse(server_json);
                    status = jobj.Property("status")?.Value?.ToString();
                    message = jobj.Property("message")?.Value?.ToString();
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Error at class 'HttpBattleshipClient' in method 'QueueMatch' ");
                    Console.WriteLine(e.Message);
                }
            }
            return (status, message);
        }


        /// <summary>
        /// returns a tuple containing the matchid, the map, and the username of the opponent if a match has been found, else an empty tuple
        /// </summary>
        /// <returns>A tuple containing the matchId (string), map (int[]), opponent (string)</returns>
        public async static Task<(string, int[], string)> MatchFound(string username, string token)
        {
            string matchId = "";
            int[] map = null;
            string opponent = "";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("username", username);
                client.DefaultRequestHeaders.Add("token", token);
                try
                {
                    HttpResponseMessage response = await client.GetAsync(uri + "/withVal/matchFound");
                    response.EnsureSuccessStatusCode();
                    string server_json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Server json: " + server_json);
                    JObject jobj = JObject.Parse(server_json);
                    string status = jobj.Property("status")?.Value?.ToString();
                    if (status.Equals("success"))
                    {
                        matchId = jobj.Property("matchId")?.Value?.ToString();
                        map = jobj.Property("map")?.Value?.ToObject<int[]>() ?? throw new NullReferenceException("Empty map array!");
                        opponent = jobj.Property("opponent")?.Value?.ToString();
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Error at class 'HttpBattleshipClient' in method 'MatchFound' ");
                    Console.WriteLine(e.Message);
                }
            }
            return (matchId, map, opponent);
        }

        /// <summary>
        /// submits the position of the battleships to the server
        /// </summary>
        /// <param name="battleships"></param>
        /// <returns>Returns a string tuple consisting of the status and message received from the server</returns>
        public async static Task<(string, string)> SubmitBattleships(int[] battleships, string username, string token)
        {
            string status = "";

            string message = "";
            //Create json object
            var obj = new
            {
                battleships
            };
            var json = new JavaScriptSerializer().Serialize(obj);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("username", username);
                client.DefaultRequestHeaders.Add("token", token);

                try
                {
                    HttpResponseMessage response = await client.PostAsync(uri + "/withVal/submitBattleships", new StringContent(json, Encoding.UTF8, "application/json"));
                    response.EnsureSuccessStatusCode();
                    string server_json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Server json: " + server_json);
                    JObject jobj = JObject.Parse(server_json);
                    status = jobj.Property("status")?.Value?.ToString();
                    message = jobj.Property("message")?.Value?.ToString();

                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Error at class 'HttpBattleshipClient' in method 'SubmitBattleships' ");
                    Console.WriteLine(e.Message);
                }
            }
            return (status, message);
        }
        
        
        /// <summary>
        /// submits the shots fired 
        /// </summary>
        /// <param name="shotsFired"></param>
        /// <returns>Returns a string tuple consisting of the status and message received from the server</returns>
        public async static Task<(string, string)> ShotFired(int xValue, int yValue, string username, string token)
        {
            string status = "";
            string message = "";
            //Create json object
            var obj = new
            {
                xValue,
                yValue
            };
            var json = new JavaScriptSerializer().Serialize(obj);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("username", username);
                client.DefaultRequestHeaders.Add("token", token);

                try
                {
                    HttpResponseMessage response = await client.PostAsync(uri + "/withVal/shotFired", new StringContent(json, Encoding.UTF8, "application/json"));
                    response.EnsureSuccessStatusCode();
                    string server_json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Server json: " + server_json);
                    JObject jobj = JObject.Parse(server_json);
                    status = jobj.Property("status")?.Value?.ToString();
                    message = jobj.Property("message")?.Value?.ToString();

                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Error at class 'HttpBattleshipClient' in method 'ShotFired' ");
                    Console.WriteLine(e.Message);
                }
            }
            return (status, message);
        }
        
        /// <summary>
        /// Checks, if its the players turn. Returns a status string and a tuple array containing the shots fired by the opponent
        /// </summary>
        /// <returns>A tuple consisting of the status, message and shotsfired received from the server</returns>
        public async static Task<(string, string, int[])> CurrentTurn(string username, string token)
        {
            string status = "";
            string message = "";
            int[] shotsFired = null;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("username", username);
                client.DefaultRequestHeaders.Add("token", token);
                try
                {
                    HttpResponseMessage response = await client.GetAsync(uri + "/withVal/currentTurn");
                    response.EnsureSuccessStatusCode();
                    string server_json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Server json: " + server_json);
                    JObject jobj = JObject.Parse(server_json);
                    status = jobj.Property("status")?.Value?.ToString();
                    message = jobj.Property("message")?.Value?.ToString();
                    if (status.Equals("success"))
                    {
                        shotsFired = jobj.Property("map")?.Value?.ToObject<int[]>() ?? throw new NullReferenceException("Empty array!");
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Error at class 'HttpBattleshipClient' in method 'CurrentTurn' ");
                    Console.WriteLine(e.Message);
                }
            }
            return (status, message, shotsFired);
        }

        #endregion
        
    }

}
