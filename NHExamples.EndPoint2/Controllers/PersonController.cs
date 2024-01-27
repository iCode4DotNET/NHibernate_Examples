using Microsoft.AspNetCore.Mvc;
using NHExamples.EndPoint2.Repository;
using NHibernate.Linq;

namespace NHExamples.EndPoint2.Controllers;

public class PersonController : Controller
{
    private readonly PersonRepository _personRepository;

    private readonly NHibernate.ISession _session;

    public PersonController(NHibernate.ISession session)
    {
        _session = session;
        _personRepository = new PersonRepository(_session);
    }

    public JsonResult Index()
    {
        var people = _personRepository.People.ToList();

        return Json(people);
    }

    public async Task<JsonResult> Edit()
    {
        try
        {
            _personRepository.BeginTransaction();

            var person = await _personRepository.People.FirstOrDefaultAsync();
            person.FirstName = "new name";
            person.LastName = "new family";

            await _personRepository.Save(person);
            await _personRepository.Commit();

            return Json(person);

        }
        catch
        {
            // log exception here
            await _personRepository.Rollback();
            return Json(BadRequest());
        }
        finally
        {
            _personRepository.CloseTransaction();
        }
    }


    public async Task<JsonResult> Create()
    {
        try
        {
            _personRepository.BeginTransaction();

            var person = new Models.Person
            {
                 FirstName = $"First Name {new Random().Next()}",
                 LastName = $"Last Name {new Random().Next()}",
            };

            await _personRepository.Save(person);
            await _personRepository.Commit();

            return Json(person);

        }
        catch
        {
            await _personRepository.Rollback();
            return Json(BadRequest());
        }
        finally
        {
            _personRepository.CloseTransaction();
        }
    }


    public async Task<JsonResult> Delete(int id)
    {
        try
        {
            _personRepository.BeginTransaction();

            var book = _personRepository.People.Where(b => b.Id == id).SingleOrDefault();

            if (book == null)
                return Json(BadRequest());

            await _personRepository.Delete(book);
            await _personRepository.Commit();

            return Json(book);

        }
        catch
        {
            await _personRepository.Rollback();
            return Json(BadRequest());
        }
        finally
        {
            _personRepository.CloseTransaction();
        }
    }
}