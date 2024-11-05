using Domain;
using System;
using System.Collections.Generic;

namespace Services
{
    public sealed class ElementService
    {
        public List<Element> List()
        {
            List<Element> list = new List<Element>();
            DataAccess data = new DataAccess();

            try
            {
                data.SetQuery("Select Id, Description From ELEMENTS");
                data.ExecuteReader();

                while (data.Reader.Read())
                {
                    Element element = new Element();
                    element.Id = (int)data.Reader["Id"];
                    element.Description = (string)data.Reader["Descripcion"];

                    list.Add(element);
                }

                return list;
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
