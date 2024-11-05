using System;

namespace Domain
{
    public sealed class Trainee
    {
        public int Id { get; set; }
        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                if (value != "")
                    _email = value;
                else
                    throw new Exception("El email es vacío...");
            }
        }

        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime BornDate { get; set; }
        public string ProfileImage { get; set; }
        public bool IsAdmin { get; set; }
    }
}
