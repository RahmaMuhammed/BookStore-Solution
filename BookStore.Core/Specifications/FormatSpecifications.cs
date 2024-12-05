﻿using BookStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Core.Specifications
{
    public class FormatSpecifications : BaseSpecifications<BookFormat>
    {
       public FormatSpecifications(int id) : base(N => N.Id == id) { }
    }
}
