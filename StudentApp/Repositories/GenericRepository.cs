
using StudentApp.Models;
using StudentApp.Models.Entity;
using System.Collections.Generic;
using System.Linq;

namespace StudentApp.Repositories
{
    public class GenericRepository<T> where T : class, new()
    {
        readonly StudentAppEntities c = new StudentAppEntities();

        public List<T> registrationsList()
        {
            return c.Set<T>().ToList();
        }

        public void StudentAdd(T p)
        {
            c.Set<T>().Add(p);
            c.SaveChanges();
        }

        public void StudentDelete(T p)
        {
            c.Set<T>().Remove(p);
            c.SaveChanges();
        }

        //public void StudentUpdate(T p)
        //{
        //    c.Set<T>().Update(p);
        //    c.SaveChanges();
        //}

        public void GetStudent(int id)
        {
            c.Set<T>().Find(id);
        }

        public void UploadFileDelete(T id)
        {
            c.Set<T>().Remove(id);
            c.SaveChanges();
        }
        public List<T> StudentList(string p)
        {
            return c.Set<T>().Include(p).ToList();
        }
    }
}
