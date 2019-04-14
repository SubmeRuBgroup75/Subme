using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SubMe.Models.DAL;

namespace SubMe.Models
{
    public class User
    {
        public int ID { get; set; }
        public string UserFBID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string UserImage { get; set; }        
        public string Email { get; set; }

        public User(string _firstname, string _lastName, string _gender, int _age, string _image, string _fbid, string _email)
        {
            FirstName = _firstname;
            LastName = _lastName;
            Gender = _gender;
            Age = _age;
            UserImage = _image;
            UserFBID = _fbid;
            Email = _email;
        }

        public User() { }

        public int insert()
        {
            DBservices dbs = new DBservices();
            int numAffected = dbs.Insert(this);
            return numAffected;
        }

        public int CheckUserExist(string fbid)
        {
            DBservices dbs = new DBservices();
            return dbs.CheckUserExist(fbid);
        }

    } //Closing the class - WRITE ONLY UP FROM HERE
}