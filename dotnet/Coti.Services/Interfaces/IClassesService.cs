using Coti.Models;
using Coti.Models.Domain;
using Coti.Models.Requests.Classes;
using System.Collections.Generic;

namespace Coti.Services
{
    public interface IClassesService
    {
        int Add(ClassAddRequest model, int userId);
        Class Get(int id);

        Paged<Class> GetByUser(int pageIndex, int pageSize, int userId);
        List<Class> GetAll();
        void Update(ClassUpdateRequest model);
        void Delete(int id);
    }
}