using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionalityMatrixApp.Services
{
    public interface ISearchable
    {
        [BindProperty(SupportsGet = true)]
        string SearchTerm { get; set; }
    }
}
