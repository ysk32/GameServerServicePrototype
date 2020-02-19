using GApiProto.MasterMemoryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GApiProto.Service
{
    public class MasterMemoryService
    {
        public static byte[] CreateMasterMemory()
        {
            var builder = new DatabaseBuilder();
            builder.Append(new Person[]
            {
                new Person { PersonId = 0, Age = 13, Gender = Gender.Male,   Name = "Dana Terry" },
                new Person { PersonId = 1, Age = 17, Gender = Gender.Male,   Name = "Kirk Obrien" },
                new Person { PersonId = 2, Age = 31, Gender = Gender.Male,   Name = "Wm Banks" },
                new Person { PersonId = 3, Age = 44, Gender = Gender.Male,   Name = "Karl Benson" },
                new Person { PersonId = 4, Age = 23, Gender = Gender.Male,   Name = "Jared Holland" },
                new Person { PersonId = 5, Age = 27, Gender = Gender.Female, Name = "Jeanne Phelps" },
                new Person { PersonId = 6, Age = 25, Gender = Gender.Female, Name = "Willie Rose" },
                new Person { PersonId = 7, Age = 11, Gender = Gender.Female, Name = "Shari Gutierrez" },
                new Person { PersonId = 8, Age = 63, Gender = Gender.Female, Name = "Lori Wilson" },
                new Person { PersonId = 9, Age = 34, Gender = Gender.Female, Name = "Lena Ramsey" },
            });

            // build database binary(you can also use `WriteToStream` for save to file).
            return builder.Build();
        }
    }
}
