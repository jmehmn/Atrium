using Coti.Data.Providers;
using Coti.Models.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Coti.Data;
using Coti.Models.Requests.Classes;
using Newtonsoft.Json;

namespace Coti.Services
{
    public class ClassesService : IClassesService
    {
        IDataProvider _data = null;
        public ClassesService(IDataProvider data)
        {
            _data = data;
        }

        public int Add(ClassAddRequest model, int userId)
        {
            int id = 0;

            string procName = "[dbo].[Classes_Insert]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
              

                // and 1 Output
                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                col.Add(idOut);

            }, returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object oId = returnCollection["@Id"].Value;

                int.TryParse(oId.ToString(), out id);
            }
            );

            return id;
        }

        public void Update(ClassUpdateRequest model)
        {
            string procName = "[dbo].[Classes_Update]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", model.Id);
                AddCommonParams(model, col);

            }, returnParameters: null);
        }

        public Class Get(int id)
        {

           

            string procName = "[dbo].[Classes_GetWith_ClasslistById]";

            Class aClass = null;

            if(id > 1000)
            {
                throw new ArgumentOutOfRangeException("This is a simulated error");
            }

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            }, delegate (IDataReader reader, short set) //Single record mapper
            {
                aClass = MapClassById(reader);
            }


            );

            return aClass;
        }

        public List<Class> GetAll()
        {

            List<Class> myClassList = null;

            string procaName = "[dbo].[Classes_GetAll_ClasslistCount]";

            _data.ExecuteCmd(procaName, inputParamMapper: null,
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    Class aClass = new Class();

                    int startingIndex = 0;

                    aClass.Id = reader.GetSafeInt32(startingIndex++);
                    aClass.Name = reader.GetSafeString(startingIndex++);
                    aClass.Description = reader.GetSafeString(startingIndex++);
                    aClass.DateTime = reader.GetSafeDateTime(startingIndex++);
                    aClass.Location_Id = reader.GetSafeInt32(startingIndex++);
                    aClass.CoverImage_Id = reader.GetSafeInt32(startingIndex++);
                    aClass.IsActive = reader.GetSafeBool(startingIndex++);
                    aClass.DateCreated = reader.GetSafeDateTime(startingIndex++);
                    aClass.DateModified = reader.GetSafeDateTime(startingIndex++);
                    aClass.CreatedBy = reader.GetSafeInt32(startingIndex++);
                    aClass.ClassCount = reader.GetInt32(startingIndex++);

                    //longer version using newtonsoft
                    string classListAsString = reader.GetString(startingIndex++);

                    if (!string.IsNullOrEmpty(classListAsString))
                    {
                        aClass.ClassList = JsonConvert.DeserializeObject<List<ClassList>>(classListAsString);
                    }

                    //or doing this is the short and sweet version
                    //aClass.ClassList = reader.DeserializeObject<List<ClassList>>(startingIndex++);

                    if (myClassList == null)
                    {
                        myClassList = new List<Class>();
                    }

                    myClassList.Add(aClass);
                });

            return myClassList;
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[Classes_DeleteById]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            }, returnParameters: null);
                
        }

        private static Class MapClass(IDataReader reader)
        {
            Class aClass = new Class();

            int startingIndex = 0;

            aClass.Id = reader.GetSafeInt32(startingIndex++);
            aClass.Name = reader.GetSafeString(startingIndex++);
            aClass.Description = reader.GetSafeString(startingIndex++);
            aClass.DateTime = reader.GetSafeDateTime(startingIndex++);
            aClass.Location_Id = reader.GetSafeInt32(startingIndex++);
            aClass.CoverImage_Id = reader.GetSafeInt32(startingIndex++);
            aClass.IsActive = reader.GetSafeBool(startingIndex++);
            aClass.DateCreated = reader.GetSafeDateTime(startingIndex++);
            aClass.DateModified = reader.GetSafeDateTime(startingIndex++);
            aClass.CreatedBy = reader.GetSafeInt32(startingIndex++);
            aClass.ClassCount = reader.GetInt32(startingIndex++);

            return aClass;
        }

        private static Class MapClassById(IDataReader reader)
        {
            Class aClass = new Class();

            int startingIndex = 0;

            aClass.Id = reader.GetSafeInt32(startingIndex++);
            aClass.Name = reader.GetSafeString(startingIndex++);
            aClass.Description = reader.GetSafeString(startingIndex++);
            aClass.DateTime = reader.GetSafeDateTime(startingIndex++);
            aClass.Location_Id = reader.GetSafeInt32(startingIndex++);
            aClass.CoverImage_Id = reader.GetSafeInt32(startingIndex++);
            aClass.IsActive = reader.GetSafeBool(startingIndex++);
            aClass.DateCreated = reader.GetSafeDateTime(startingIndex++);
            aClass.DateModified = reader.GetSafeDateTime(startingIndex++);
            aClass.CreatedBy = reader.GetSafeInt32(startingIndex++);
            aClass.ClassCount = reader.GetInt32(startingIndex++);

            return aClass;
        }

        private static void AddCommonParams(ClassAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Name", model.Name);
            col.AddWithValue("@Description", model.Description);
            col.AddWithValue("@DateTime", model.DateTime);
            col.AddWithValue("@Location_Id", model.Location_Id);
            col.AddWithValue("@CoverImage_Id", model.CoverImage_Id);
            col.AddWithValue("@IsActive", model.IsActive);
            col.AddWithValue("@DateCreated", model.DateCreated);
            col.AddWithValue("@DateModified", model.DateModified);
            col.AddWithValue("@CreatedBy", model.CreatedBy);
        }

        private static void AddCommonParams(ClassUpdateRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Name", model.Name);
            col.AddWithValue("@Description", model.Description);
            col.AddWithValue("@DateTime", model.DateTime);
            col.AddWithValue("@Location_Id", model.Location_Id);
            col.AddWithValue("@CoverImage_Id", model.CoverImage_Id);
            col.AddWithValue("@IsActive", model.IsActive);
        }
    }
}
