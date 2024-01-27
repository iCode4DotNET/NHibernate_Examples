using NHExamples.EndPoint2.Models;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace NHExamples.EndPoint2.Mapping;

public class PersonMap : ClassMapping<Person>
{
    public PersonMap()
    {
        Id(x => x.Id, x =>
        {
            x.Generator(Generators.Identity);
            x.Type(NHibernateUtil.Int32);
            x.Column("ID");
            x.UnsavedValue(0);
        });

        Property(b => b.FirstName, x =>
        {
            x.Length(50);
            x.Type(NHibernateUtil.String);
            x.NotNullable(true);
        });

        Property(b => b.LastName, x =>
        {
            x.Length(50);
            x.Type(NHibernateUtil.String);
            x.NotNullable(true);
        });

        Table("Person");
    }
}