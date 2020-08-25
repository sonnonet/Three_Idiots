using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Three_Idiots_Project
{
    public interface ISQLite
    {
        SQLiteAsyncConnection GetConnection();
    }
}
