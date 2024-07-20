using System;
using System.Collections.Generic;

namespace DB;

public partial class Book
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public int AutorId { get; set; }

    public virtual Autor Autor { get; set; } = null!;
}
