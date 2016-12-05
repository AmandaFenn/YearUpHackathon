using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using YearUpRestApi.Models;

namespace YearUpRestApi.Controllers
{
    [RoutePrefix("api/Master")]
    public class MasterController : ApiController
    {
        private SQLCommunication sqlTools = new SQLCommunication();

        [HttpPost]
        [ActionName("ByEmailAndPassword")]
        public string ByEmailAndPassword(string email, string password)
        {       
            /*
            var jsonObject = JObject.Parse(json);
            var email = jsonObject.SelectToken("email").Value<string>();
            var password = jsonObject.SelectToken("password").Value<string>();*/

            string response = sqlTools.GetUserByEmail(email, password);
            return response;   
        }  
        
        [HttpPost]
        [Route("ById")]
        public string ById(string id)
        {
            //var jsonObject = JObject.Parse(idJson);
            //var id = jsonObject.SelectToken("user_id").Value<int>();

            //string response = sqlTools.GetUserById(id);
            return id;
        }  

        [HttpPost]
        [Route("api/Master/SetForm")]
        public string SetForm(string json)
        {
            var jsonObject = JObject.Parse(json);
            var id = jsonObject.SelectToken("user_id").Value<int>();
            var email = jsonObject.SelectToken("email").Value<string>();
            var name = jsonObject.SelectToken("name").Value<string>();
            var user_type = jsonObject.SelectToken("user_type").Value<int>();
            var englishS = jsonObject.SelectToken("english").Value<string>();
            var mathS = jsonObject.SelectToken("math").Value<string>();
            var skypeS = jsonObject.SelectToken("skype").Value<string>();
            var location = jsonObject.SelectToken("city").Value<string>();
            var availability = jsonObject.SelectToken("availability");

            int math = GetBool(mathS);
            int english = GetBool(englishS);
            int skype = GetBool(skypeS);

            bool response = true;

            if (user_type == 1)
            {
                response = sqlTools.AddStudent(id, skype, location, english, math);
            }
            else if (user_type == 2)
            {
                response = sqlTools.AddTeacher(id, skype, location, english, math);
            }

            return response.ToString();
        }

        private int GetBool(string val)
        {
            if (val == "true")
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

    }
}
