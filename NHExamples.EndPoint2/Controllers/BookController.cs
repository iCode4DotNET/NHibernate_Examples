using Microsoft.AspNetCore.Mvc;
using NHExamples.EndPoint2.Repository;
using NHibernate.Linq;

namespace NHExamples.EndPoint2.Controllers;

public class BookController : Controller
{
    private readonly BookRepository _bookRepository;

    private readonly NHibernate.ISession _session;

    public BookController(NHibernate.ISession session)
    {
        _session = session;
        _bookRepository = new BookRepository(_session);
    }

    public JsonResult Index()
    {
        var people = _bookRepository.Books.ToList();

        return Json(people);
    }

    public JsonResult Index2()
    {
        var books = _bookRepository.Books
                            .Where(b => b.Title.StartsWith("How to"))
                            .ToList();

        return Json(books);
    }

    public async Task<JsonResult> Index3()
    {
        var books = await _bookRepository.Books
                                  .Where(b => b.Title.StartsWith("How to"))
                                  .ToListAsync();
        return Json(books);
    }


    public async Task<JsonResult> TryEdit()
    {
        var book = await _bookRepository.Books.FirstOrDefaultAsync();
        book.Title += " (sold out)";
        await _bookRepository.Save(book);
        return Json(book);
    }


    public async Task<JsonResult> Edit()
    {
        try
        {
            _bookRepository.BeginTransaction();

            var book = await _bookRepository.Books.FirstOrDefaultAsync();
            book.Title += " (sold out)";

            await _bookRepository.Save(book);
            await _bookRepository.Commit();

            return Json(book);

        }
        catch
        {
            await _bookRepository.Rollback();
            return Json(BadRequest());
        }
        finally
        {
            _bookRepository.CloseTransaction();
        }
    }

    public async Task<JsonResult> Create()
    {
        try
        {
            _bookRepository.BeginTransaction();

            var book = new Models.Book
            {
                Title = $"Book No {new Random().Next()}"
            };
            
            await _bookRepository.Save(book);
            await _bookRepository.Commit();

            return Json(book);

        }
        catch
        {
            await _bookRepository.Rollback();
            return Json(BadRequest());
        }
        finally
        {
            _bookRepository.CloseTransaction();
        }
    }


    public async Task<JsonResult> Delete(int id)
    {
        try
        {
            _bookRepository.BeginTransaction();

            var book = _bookRepository.Books.Where(b => b.Id == id).SingleOrDefault();

            if (book == null)
                return Json(BadRequest());  

            await _bookRepository.Delete(book);
            await _bookRepository.Commit();

            return Json(book);

        }
        catch
        {
            await _bookRepository.Rollback();
            return Json(BadRequest());
        }
        finally
        {
            _bookRepository.CloseTransaction();
        }
    }
}