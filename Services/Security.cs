using Domain;

namespace Services
{
    public static class Security
    {
        public static bool ActiveSession(object user)
        {
            Trainee trainee = user != null ? (Trainee)user : null;
            if (trainee != null && trainee.Id != 0)
                return true;
            else
                return false;
        }

        public static bool IsAdmin(object user)
        {
            Trainee trainee = user != null ? (Trainee)user : null;
            return trainee != null ? trainee.IsAdmin : false;
        }
    }
}
