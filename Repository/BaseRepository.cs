using System.Collections.Generic;

namespace InterviewTest.Repository;

public abstract class BaseRepository<T> : IRepository<T>
{
    protected const string DataSource = "./SqliteDB.db";

    public virtual List<T> Get() => throw new System.NotImplementedException();

    public virtual bool Add(List<T> items) => throw new System.NotImplementedException();

    public virtual List<T> Update(List<T> items) => throw new System.NotImplementedException();

    public virtual bool Delete(List<int> ids) => throw new System.NotImplementedException();
    public virtual bool Delete(List<string> names) => throw new System.NotImplementedException();
}