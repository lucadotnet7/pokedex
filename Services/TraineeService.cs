using Domain;
using System;

namespace Services
{
    public sealed class TraineeService
    {
        public int Add(Trainee trainee)
        {
            DataAccess data = new DataAccess();

            try
            {
                data.SetProcedure("InsertNew");
                data.SetParameter("@Email", trainee.Email);
                data.SetParameter("@Password", trainee.Password);
                return data.ExecuteScalarAction();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                data.CloseConnection();
            }

        }

        public void Update(Trainee trainee)
        {
            DataAccess data = new DataAccess();
            try
            {
                data.SetQuery("Update USERS set ProfileImage = @image, Firstanme = @firstname, Lastname = @lastname, BornDate = @bornDate Where Id = @id");
                data.SetParameter("@image", (object)trainee.ProfileImage ?? DBNull.Value);
                data.SetParameter("@firstname", trainee.Firstname);
                data.SetParameter("@lastname", trainee.Lastname);
                data.SetParameter("@bornData", trainee.BornDate);
                data.SetParameter("@id", trainee.Id);
                data.ExecuteAction();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                data.CloseConnection();
            }
        }

        public bool Login(Trainee trainee)
        {
            DataAccess data = new DataAccess();
            try
            {
                data.SetQuery("Select Id, Email, Password, IsAdmin, ProfileImage, Firstname, Lastname, BornDate from USERS Where Email = @email And Password = @pass");
                data.SetParameter("@email", trainee.Email);
                data.SetParameter("@pass", trainee.Password);
                data.ExecuteReader();
                if (data.Reader.Read())
                {
                    trainee.Id = (int)data.Reader["Id"];
                    trainee.IsAdmin = (bool)data.Reader["IsAdmin"];
                    if (!(data.Reader["imagenPerfil"] is DBNull))
                        trainee.ProfileImage = (string)data.Reader["ProfileImage"];
                    if (!(data.Reader["Firstname"] is DBNull))
                        trainee.Firstname = (string)data.Reader["Firstname"];
                    if (!(data.Reader["apellido"] is DBNull))
                        trainee.Lastname = (string)data.Reader["Lastname"];
                    if (!(data.Reader["fechaNacimiento"] is DBNull))
                        trainee.BornDate = DateTime.Parse(data.Reader["BornDate"].ToString());

                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                data.CloseConnection();
            }
        }
    }
}
