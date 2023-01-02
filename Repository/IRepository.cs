using System.Collections.Generic;

namespace InterviewTest.Repository;

public interface IRepository<T>
{
    List<T> Get();

    bool Add(List<T> items);

    List<T> Update(List<T> items);

    bool Delete(List<int> ids);
}