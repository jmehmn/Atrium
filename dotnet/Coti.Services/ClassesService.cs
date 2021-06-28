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
using Coti.Models;

namespace Coti.Services
{
    public class ClassesService : IClassesService
    {
        private IAuthenticationService<int> _authService;
        IDataProvider _data = null;
        public ClassesService(IAuthenticationService<int> authService, IDataProvider data)
        {
            _authService = authService;
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

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            }, delegate (IDataReader reader, short set) //Single record mapper
            {
                Class thisClass = MapClass(reader);

                thisClass.Member = reader.DeserializeObject< List<Member>>(18);

                ////Long version of NewtonSoft
                //string classListAsString = reader.GetString(startingIndex++);

                //if (!string.IsNullOrEmpty(classListAsString))
                //{
                //    thisClass.ClassList = JsonConvert.DeserializeObject<List<ClassList>>(classListAsString);
                //}

                //if (aClass == null)
                //{
                //    aClass = new Class();
                //}

                //aClass = thisClass;

                ////or doing this is the short and sweet version
                

                if (aClass == null)
                {
                    aClass = new Class();
                }

                aClass = thisClass;
            }


            );

            return aClass;
        }

        public Paged<MemberClass> GetByUser(int pageIndex, int pageSize, int userId) {

            Paged<MemberClass> pagedResult = null;

            List<MemberClass> result = null;

            int totalCount = 0;
            {

                string procName = "[dbo].[Classes_GetBy_User]";

                _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
                   {
                       paramCollection.AddWithValue("@pageIndex", pageIndex);
                       paramCollection.AddWithValue("@pageSize", pageSize);
                       paramCollection.AddWithValue("@user_Id", userId);
                   },
                   (reader, recordSetIndex) =>
                   {

                       MemberClass myClass = MapMemberClass(reader);
                       totalCount = reader.GetSafeInt32(18);


                       if (result == null)
                       {
                           result = new List<MemberClass>();
                       }

                       result.Add(myClass);
                   });

                if (result != null)
                {
                    pagedResult = new Paged<MemberClass>(result, pageIndex, pageSize, totalCount);
                }
            }

            return pagedResult;
        }

        public List<Class> GetAll()
        {

            List<Class> myClassList = null;

            string procaName = "[dbo].[Classes_GetAll_ClasslistCount]";

            _data.ExecuteCmd(procaName, inputParamMapper: null,
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    Class aClass = MapClass(reader);

                    //longer version using newtonsoft
                    //string classListAsString = reader.GetString(startingIndex++);

                    //if (!string.IsNullOrEmpty(classListAsString))
                    //{
                    //    aClass.Member = JsonConvert.DeserializeObject<List<Member>>(classListAsString);
                    //}

                    //or doing this is the short and sweet version
                    aClass.Member = reader.DeserializeObject<List<Member>>(18);

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

            aClass.Location = new Location();

            aClass.CoverImage = new CoverImage();

            int startingIndex = 0;

            aClass.Id = reader.GetSafeInt32(startingIndex++);
            aClass.Name = reader.GetSafeString(startingIndex++);
            aClass.Description = reader.GetSafeString(startingIndex++);
            aClass.DateTime = reader.GetSafeDateTime(startingIndex++);
            aClass.Location.Id = reader.GetSafeInt32(startingIndex++);
            aClass.Location.Name = reader.GetSafeString(startingIndex++);
            aClass.Location.LineOne = reader.GetSafeString(startingIndex++);
            aClass.Location.LineTwo = reader.GetSafeString(startingIndex++);
            aClass.Location.City = reader.GetSafeString(startingIndex++);
            aClass.Location.State = reader.GetSafeString(startingIndex++);
            aClass.Location.ZipCode = reader.GetSafeInt32(startingIndex++);
            aClass.CoverImage.Id = reader.GetSafeInt32(startingIndex++);
            aClass.CoverImage.ImgUrl = reader.GetSafeString(startingIndex++);
            aClass.IsActive = reader.GetSafeBool(startingIndex++);
            aClass.DateCreated = reader.GetSafeDateTime(startingIndex++);
            aClass.DateModified = reader.GetSafeDateTime(startingIndex++);
            aClass.CreatedBy = reader.GetSafeInt32(startingIndex++);
            aClass.ClassCount = reader.GetInt32(startingIndex++);
            

            return aClass;
        }

        private static MemberClass MapMemberClass(IDataReader reader)
        {
            MemberClass myClass = new MemberClass();

            myClass.Location = new Location();

            myClass.CoverImage = new CoverImage();

            int startingIndex = 0;

            myClass.Id = reader.GetSafeInt32(startingIndex++);
            myClass.Name = reader.GetSafeString(startingIndex++);
            myClass.Description = reader.GetSafeString(startingIndex++);
            myClass.DateTime = reader.GetSafeDateTime(startingIndex++);
            myClass.Location.Id = reader.GetSafeInt32(startingIndex++);
            myClass.Location.Name = reader.GetSafeString(startingIndex++);
            myClass.Location.LineOne = reader.GetSafeString(startingIndex++);
            myClass.Location.LineTwo = reader.GetSafeString(startingIndex++);
            myClass.Location.City = reader.GetSafeString(startingIndex++);
            myClass.Location.State = reader.GetSafeString(startingIndex++);
            myClass.Location.ZipCode = reader.GetSafeInt32(startingIndex++);
            myClass.CoverImage.Id = reader.GetSafeInt32(startingIndex++);
            myClass.CoverImage.ImgUrl = reader.GetSafeString(startingIndex++);
            myClass.IsActive = reader.GetSafeBool(startingIndex++);
            myClass.DateCreated = reader.GetSafeDateTime(startingIndex++);
            myClass.DateModified = reader.GetSafeDateTime(startingIndex++);
            myClass.CreatedBy = reader.GetSafeInt32(startingIndex++);
            myClass.ClassCount = reader.GetInt32(startingIndex++);

            return myClass;
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
