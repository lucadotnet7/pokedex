using Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Services
{
    public sealed class PokemonService
    {
        public List<Pokemon> List(string id = "")
        {
            List<Pokemon> list = new List<Pokemon>();
            SqlConnection connection = new SqlConnection();
            SqlCommand command = new SqlCommand();
            SqlDataReader reader;

            try
            {
                connection.ConnectionString = ConfigurationManager.AppSettings["cadenaconnection"];
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "Select Number, Name, P.[Description], ImageUrl, E.[Description] Type, D.[Description] Weakness, P.TypeId, P.WeaknessId, P.Id, P.Active From POKEMONS P, ELEMENTS E, ELEMENTS D Where E.Id = P.TypeId And D.Id = P.WeaknessId ";
                if (id != "")
                    command.CommandText += " and P.Id = " + id;

                command.Connection = connection;

                connection.Open();
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Pokemon pokemon = new Pokemon();
                    pokemon.Id = (int)reader["Id"];
                    pokemon.Number = reader.GetInt32(0);
                    pokemon.Name = (string)reader["Name"];
                    pokemon.Description = (string)reader["Description"];

                    if (!(reader["ImageUrl"] is DBNull))
                        pokemon.ImageUrl = (string)reader["ImageUrl"];

                    pokemon.Type = new Element();
                    pokemon.Type.Id = (int)reader["TypeId"];
                    pokemon.Type.Description = (string)reader["Type"];
                    pokemon.Weakness = new Element();
                    pokemon.Weakness.Id = (int)reader["WeaknessId"];
                    pokemon.Weakness.Description = (string)reader["Weakness"];

                    pokemon.Active = bool.Parse(reader["Active"].ToString());

                    list.Add(pokemon);
                }

                connection.Close();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<Pokemon> ListWithSP()
        {
            List<Pokemon> list = new List<Pokemon>();
            DataAccess data = new DataAccess();
            try
            {
                data.SetProcedure("StoredList");

                data.ExecuteReader();
                while (data.Reader.Read())
                {
                    Pokemon pokemon = new Pokemon();
                    pokemon.Id = (int)data.Reader["Id"];
                    pokemon.Number = data.Reader.GetInt32(0);
                    pokemon.Name = (string)data.Reader["Name"];
                    pokemon.Description = (string)data.Reader["Description"];
                    if (!(data.Reader["ImageUrl"] is DBNull))
                        pokemon.ImageUrl = (string)data.Reader["ImageUrl"];

                    pokemon.Type = new Element();
                    pokemon.Type.Id = (int)data.Reader["TypeId"];
                    pokemon.Type.Description = (string)data.Reader["Type"];
                    pokemon.Weakness = new Element();
                    pokemon.Weakness.Id = (int)data.Reader["WeaknessId"];
                    pokemon.Weakness.Description = (string)data.Reader["Weakness"];

                    pokemon.Active = bool.Parse(data.Reader["Active"].ToString());

                    list.Add(pokemon);
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Add(Pokemon pokemon)
        {
            DataAccess data = new DataAccess();

            try
            {
                data.SetQuery(
                        "Insert into POKEMONS (Number, Name, Description, Active, TypeId, WeaknessId, ImageUrl)" +
                        "values(" + pokemon.Number + ", '" + pokemon.Name + "', '" + pokemon.Description + "', 1, @typeId, @weaknessId, @imageUrl)");
                data.SetParameter("@typeId", pokemon.Type.Id);
                data.SetParameter("@weaknessId", pokemon.Weakness.Id);
                data.SetParameter("@imageUrl", pokemon.ImageUrl);
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

        public void AddWithSP(Pokemon pokemon)
        {
            DataAccess data = new DataAccess();

            try
            {
                data.SetProcedure("StoredNewPokemon");
                data.SetParameter("@number", pokemon.Number);
                data.SetParameter("@name", pokemon.Name);
                data.SetParameter("@description", pokemon.Description);
                data.SetParameter("@img", pokemon.ImageUrl);
                data.SetParameter("@typeId", pokemon.Type.Id);
                data.SetParameter("@weaknessId", pokemon.Weakness.Id);

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

        public void Update(Pokemon pokemon)
        {
            DataAccess data = new DataAccess();
            try
            {
                data.SetQuery("update POKEMONS set Number = @number, Name = @name, " +
                              "Description = @description, " +
                              "ImageUrl = @img, TypeId = @typeId, " +
                              "WeaknessId = @weaknessId Where Id = @id");

                data.SetParameter("@number", pokemon.Number);
                data.SetParameter("@name", pokemon.Name);
                data.SetParameter("@description", pokemon.Description);
                data.SetParameter("@img", pokemon.ImageUrl);
                data.SetParameter("@typeId", pokemon.Type.Id);
                data.SetParameter("@weaknessId", pokemon.Weakness.Id);
                data.SetParameter("@id", pokemon.Id);

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

        public void UpdateWithSP(Pokemon pokemon)
        {
            DataAccess data = new DataAccess();
            try
            {
                data.SetProcedure("storedModifyPokemon");
                data.SetParameter("@number", pokemon.Number);
                data.SetParameter("@name", pokemon.Name);
                data.SetParameter("@description", pokemon.Description);
                data.SetParameter("@img", pokemon.ImageUrl);
                data.SetParameter("@typeId", pokemon.Type.Id);
                data.SetParameter("@weaknessId", pokemon.Weakness.Id);
                data.SetParameter("@id", pokemon.Id);

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

        public List<Pokemon> Filter(string field, string criterion, string filter, string status)
        {
            List<Pokemon> list = new List<Pokemon>();
            DataAccess data = new DataAccess();
            try
            {
                string query = "Select Number, Name, P.[Description], ImageUrl, E.[Description] Type, " +
                               "D.[Description] Weakness, P.TypeId, P.WeaknessId, P.Id, " +
                               "P.Active From POKEMONS P, ELEMENTS E, ELEMENTS D Where E.Id = P.TypeId And D.Id = P.WeaknessId And ";

                if (field == "Number")
                {
                    switch (criterion)
                    {
                        case "Mayor a":
                            query += "Number > " + filter;
                            break;
                        case "Menor a":
                            query += "Number < " + filter;
                            break;
                        default:
                            query += "Number = " + filter;
                            break;
                    }
                }
                else if (field == "Name")
                {
                    switch (criterion)
                    {
                        case "Comienza con":
                            query += "Name like '" + filter + "%' ";
                            break;
                        case "Termina con":
                            query += "Name like '%" + filter + "'";
                            break;
                        default:
                            query += "Name like '%" + filter + "%'";
                            break;
                    }
                }
                else
                {
                    switch (criterion)
                    {
                        case "Comienza con":
                            query += "E.Description like '" + filter + "%' ";
                            break;
                        case "Termina con":
                            query += "E.Description like '%" + filter + "'";
                            break;
                        default:
                            query += "E.Description like '%" + filter + "%'";
                            break;
                    }
                }

                if (status == "Activo")
                    query += " and P.Active = 1";
                else if (status == "Inactivo")
                    query += " and P.Active = 0";

                data.SetQuery(query);
                data.ExecuteReader();
                while (data.Reader.Read())
                {
                    Pokemon pokemon = new Pokemon();
                    pokemon.Id = (int)data.Reader["Id"];
                    pokemon.Number = data.Reader.GetInt32(0);
                    pokemon.Name = (string)data.Reader["Name"];
                    pokemon.Description = (string)data.Reader["Description"];
                    if (!(data.Reader["ImageUrl"] is DBNull))
                        pokemon.ImageUrl = (string)data.Reader["ImageUrl"];

                    pokemon.Type = new Element();
                    pokemon.Type.Id = (int)data.Reader["TypeId"];
                    pokemon.Type.Description = (string)data.Reader["Type"];
                    pokemon.Weakness = new Element();
                    pokemon.Weakness.Id = (int)data.Reader["WeaknessId"];
                    pokemon.Weakness.Description = (string)data.Reader["Weakness"];

                    pokemon.Active = bool.Parse(data.Reader["Active"].ToString());

                    list.Add(pokemon);
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void PhysicDelete(int id)
        {
            try
            {
                DataAccess data = new DataAccess();
                data.SetQuery("delete from pokemons where id = @id");
                data.SetParameter("@id", id);
                data.ExecuteAction();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void LogicDelete(int id, bool active = false)
        {
            try
            {
                DataAccess data = new DataAccess();
                data.SetQuery("update POKEMONS set Active = @active Where id = @id");
                data.SetParameter("@id", id);
                data.SetParameter("@active", active);
                data.ExecuteAction();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
